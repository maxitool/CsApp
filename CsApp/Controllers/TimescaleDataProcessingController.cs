using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using CsApp.Csv;
using CsApp.DB.Models;
using CsApp.FilesProcessing;
using Npgsql;
using CsApp.DB.Queries.ORM;
using GetValueResponse = CsApp.FilesProcessing.TimescaleCsvReader.GetValueResponse;
using CsApp.Calculations;
using CsApp.Validators;
using System.Diagnostics;
using CsApp.DB;

namespace CsApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimescaleDataProcessingController : Controller
    {
        private static readonly int MAX_COUNT_FILE_LINES = 10000;

        [HttpPost(Name = "SaveToDB")]
        public async Task<IActionResult> SaveToDB(IFormFile file)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TimescaleCsvReader reader = new(file);
            if (reader.File == null || file.Length == 0)
                return BadRequest("Didn't send the file or the file format is bad, require .csv format.");
            if (file.Length == 0)
                return BadRequest("The file is empty.");
            using NpgsqlConnection connection = new NpgsqlConnection(CsAppDBContext.CONNECTION_STRINGS);
            FilesORM filesORM = new(connection);
            int fileId = await filesORM.GetId(file.FileName);
            bool isAddFile = fileId >= 0;
            using NpgsqlTransaction transaction = connection.BeginTransaction();
            bool answer;
            ValuesORM valuesORM = new(connection);
            ResultsORM resultsORM = new(connection);
            if (fileId < 0)
            {
                // insert file
                Files filedb = new();
                filedb.filename = file.FileName;
                fileId = await filesORM.InsertData(transaction, filedb);
                if (fileId < 0)
                {
                    transaction.Rollback();
                    return BadRequest("Can't add file to Files table (db problem).");
                }
            }
            else
            {
                // delete all data connected with file
                answer = await valuesORM.DeleteData(transaction, fileId);
                if (!answer)
                {
                    transaction.Rollback();
                    return BadRequest("Can't delete data from Values table (db problem).");
                }
                answer = await resultsORM.DeleteData(transaction, fileId);
                if (!answer)
                {
                    transaction.Rollback();
                    return BadRequest("Can't delete data from Results table (db problem).");
                }
            }
            if (!reader.OpenReaderStream())
            {
                transaction.Rollback();
                return BadRequest("Can't open reader of the file.");
            }
            GetValueResponse response = await reader.GetValueClass();
            CalculateResults calculateResults = new();
            bool isGood = true, canAddToDb = true; int curId; 
            while (response.State)
            {
                response.Value.id_file = fileId;
                if (!ValueValidator.ValidateValue(response.Value))
                {
                    isGood = false;
                    break;
                }
                calculateResults.AddValue(response.Value);
                curId = await valuesORM.InsertData(transaction, response.Value);
                if (curId < 0)
                {
                    canAddToDb = false;
                    break;
                }
                response = await reader.GetValueClass();
            }
            reader.CloseReaderStream();
            // check all conditions
            if (!isGood)
            {
                transaction.Rollback();
                return BadRequest($"Element on position {calculateResults.CountElements + 1} didn't pass validations.");
            }
            if (!canAddToDb)
            {
                transaction.Rollback();
                return BadRequest($"Can't insert Value to Values table (db error).");
            }
            if (calculateResults.CountElements <= 0 || calculateResults.CountElements > MAX_COUNT_FILE_LINES)
            {
                transaction.Rollback();
                return BadRequest($"Count of lines in the file more then {MAX_COUNT_FILE_LINES}.");
            }
            if (response.ErrorState != GetValueResponse.ErrorStates.FileEnds)
            {
                transaction.Rollback();
                return BadRequest($"Element on position {calculateResults.CountElements + 1} didn't pass checks.");
            }
            DB.Models.Results result = calculateResults.CalculateResultsFromValue(fileId);
            curId = await resultsORM.InsertData(transaction, result);
            if (curId < 0)
            {
                transaction.Rollback();
                return BadRequest($"Can't insert Result to Results table (db error).");
            }
            transaction.Commit();
            stopwatch.Stop();
            if (isAddFile)
                return Ok($"Success! Method did change {calculateResults.CountElements} lines within {stopwatch.Elapsed}.");
            return Ok($"Success! Method did add {calculateResults.CountElements} lines within {stopwatch.Elapsed}.");
        }
    }
}
