using CsApp.Csv;
using CsApp.DB.Models;
using CsApp.Parsers;

namespace CsApp.FilesProcessing
{
    public class TimescaleCsvReader : FileReader
    {
        public static readonly int COUNT_FIELDS = 3;
        public TimescaleCsvReader(IFormFile file) : base(file) 
        {
            if (!file.FileName.EndsWith(".csv"))
            {
                Console.WriteLine($"File {file.FileName} has bad format, require .csv format");
                this._file = null;
            }
        }

        public async Task<GetValueResponse> GetValueClass()
        {
            string? data = await GetData();
            if (data == null)
                return new(GetValueResponse.ErrorStates.FileEnds);
            return StringToResults(data);
        }
        protected GetValueResponse StringToResults(string data)
        {
            string[] dataArr = data.Split(';');
            if (dataArr.Length < COUNT_FIELDS)
            {
                Console.WriteLine($"Count of file rows less then {COUNT_FIELDS}, maybe it split with ',' char");
                return new(GetValueResponse.ErrorStates.LessCountFields);
            }
            DateTimeParser dateTimeParser = new();
            DateTime? datetime = dateTimeParser.StringToDateTime(dataArr[0]);
            if (datetime == null)
                return new(GetValueResponse.ErrorStates.CantConvert);
            try
            {
                DB.Models.Values value = new();
                value.date = (DateTime)datetime;
                value.execution_time = int.Parse(dataArr[1]);
                value.value = decimal.Parse(dataArr[2].Replace(',', '.'));
                return new(value);
            } catch (Exception e)
            {
                Console.WriteLine($"Can't convert string to Value class: {e.Message}");
            }
            return new(GetValueResponse.ErrorStates.CantConvert);
        }
        public class GetValueResponse
        {
            public enum ErrorStates
            {
                None,
                FileEnds,
                LessCountFields,
                CantConvert
            }
            public readonly bool State;
            public readonly Values? Value;
            public readonly ErrorStates ErrorState = ErrorStates.None;

            public GetValueResponse(Values value)
            {
                this.State = true;
                this.Value = value;
            }
            public GetValueResponse(ErrorStates errorState)
            {
                this.ErrorState = errorState;
            }
        }
    }
}