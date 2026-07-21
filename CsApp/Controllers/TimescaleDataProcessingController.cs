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
using CsApp.FrontFilters;
using CsApp.DB.Queries.Core.Filters;
using System.Text;

namespace CsApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimescaleDataProcessingController : Controller
    {
        private static readonly int MAX_COUNT_FILE_LINES = 10000;
        private static readonly int LIMIT = 10;

        [HttpPost("SaveToDB")]
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
            ResultsORM resultsORM = new(connection);
            int resultId = await resultsORM.GetId(file.FileName);
            bool isChangeFile = resultId >= 0;
            using NpgsqlTransaction transaction = connection.BeginTransaction();
            bool answer;
            ValuesORM valuesORM = new(connection);
            if (resultId < 0)
            {
                // insert file
                DB.Models.Results result = new();
                result.filename = file.FileName;
                resultId = await resultsORM.InsertData(transaction, result);
                if (resultId < 0)
                {
                    transaction.Rollback();
                    return BadRequest("Can't add file to Results table (db problem). Check console.");
                }
            }
            else
            {
                // delete all data connected with file
                answer = await valuesORM.DeleteData(transaction, resultId);
                if (!answer)
                {
                    transaction.Rollback();
                    return BadRequest("Can't delete data from Values table (db problem). Check console.");
                }
            }
            if (!reader.OpenReaderStream())
            {
                transaction.Rollback();
                return BadRequest("Can't open reader of the file. Check console.");
            }
            GetValueResponse response = await reader.GetValueClass();
            CalculateResults calculateResults = new();
            bool isGood = true, canAddToDb = true; int curId; 
            while (response.State)
            {
                response.Value.result_id = resultId;
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
                return BadRequest($"Element on position {calculateResults.CountElements + 1} didn't pass validations. Check console.");
            }
            if (!canAddToDb)
            {
                transaction.Rollback();
                return BadRequest($"Can't insert Value to Values table (db error). Check console.");
            }
            if (calculateResults.CountElements <= 0 || calculateResults.CountElements > MAX_COUNT_FILE_LINES)
            {
                transaction.Rollback();
                return BadRequest($"Count of lines in the file more then {MAX_COUNT_FILE_LINES}. Check console.");
            }
            if (response.ErrorState != GetValueResponse.ErrorStates.FileEnds)
            {
                transaction.Rollback();
                return BadRequest($"Element on position {calculateResults.CountElements + 1} didn't pass checks. Check console.");
            }
            DB.Models.Results lastResult = calculateResults.CalculateResultsFromValue(file.FileName);
            lastResult.id = resultId;
            curId = await resultsORM.UpdateAllData(transaction, lastResult);
            if (curId < 0)
            {
                transaction.Rollback();
                return BadRequest($"Can't insert Result to Results table (db error). Check console.");
            }
            transaction.Commit();
            stopwatch.Stop();
            if (isChangeFile)
                return Ok($"Success! Method did change {calculateResults.CountElements} lines within {stopwatch.Elapsed}.");
            return Ok($"Success! Method did add {calculateResults.CountElements} lines within {stopwatch.Elapsed}.");
        }

        [HttpPost("GetResultsWithFilters")]
        public async Task<IActionResult> GetResultsWithFilters([FromBody] APIFrontFilters? apiFilters)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ParentFilter? filter = null;
            if (apiFilters != null)
                filter = apiFilters.GetFilter();
            using NpgsqlConnection connection = new NpgsqlConnection(CsAppDBContext.CONNECTION_STRINGS);
            ResultsORM resultsORM = new(connection);
            if (filter == null)
            {
                List<DB.Models.Results>? results1 = await resultsORM.GetAll();
                if (results1 == null)
                    return BadRequest($"Can't get all data from Results table (db error). Check console.");
                StringBuilder stringBuilder1 = new("Get data without filters:\n");
                if (results1.Count <= 0)
                    stringBuilder1.Append("\nNo data for the filters.");
                else
                    foreach (var item in results1)
                        stringBuilder1.Append('\n').Append(item.ToString());
                stringBuilder1.Append("\n\nDid you specify filters?\n They may have been specified in the wrong format; check the console output.");
                stopwatch.Stop();
                stringBuilder1.Append($"\nSuccess! Method done within {stopwatch.Elapsed}.");
                return Ok(stringBuilder1.ToString());
            }
            List<DB.Models.Results>? results2 = await resultsORM.GetAllWithFilter(filter);
            if (results2 == null)
                return BadRequest($"Can't get all data with filters from Results table (db error). Check console.");
            StringBuilder stringBuilder2 = new("Get data with filters:\n\n");
            if (results2.Count <= 0)
                stringBuilder2.Append("\nNo data for the filters.");
            else
                foreach (var item in results2)
                    stringBuilder2.Append('\n').Append(item.ToString());
            stringBuilder2.Append('\n').Append('\n').Append(apiFilters.WorkFilters);
            stringBuilder2.Append("\n\nDidn't work all filters? \n They may have been specified in the wrong format; check the console output.");
            stopwatch.Stop();
            stringBuilder2.Append($"\nSuccess! Method done within {stopwatch.Elapsed}.");
            return Ok(stringBuilder2.ToString());
        }
        [HttpGet("Get10Values")]
        public async Task<IActionResult> GetResultsWithFilters(string filename)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using NpgsqlConnection connection = new NpgsqlConnection(CsAppDBContext.CONNECTION_STRINGS);
            ValuesORM valuesORM = new(connection);
            List<Values> values = await valuesORM.GetWithFilenameAndLimit(filename, LIMIT);
            if (values == null || values.Count <= 0)
            {
                stopwatch.Stop();
                return Ok($"Didn't find Values with the filename.\nIf error check Console.\nSuccess! Method done within {stopwatch.Elapsed}.");
            }
            StringBuilder stringBuilder = new("Data:\n");
            foreach (var item in values)
                stringBuilder.Append('\n').Append(item.ToString());
            stopwatch.Stop();
            stringBuilder.Append($"\n\nSuccess! Method done within {stopwatch.Elapsed}.");
            return Ok(stringBuilder.ToString());
        }
    }
}
