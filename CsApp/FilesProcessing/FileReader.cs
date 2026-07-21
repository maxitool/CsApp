using CsvHelper;
using System.Globalization;

namespace CsApp.Csv
{
    public class FileReader
    {
        public IFormFile? File { get { return _file;  } }
        protected StreamReader? _reader;
        protected IFormFile? _file;

        public FileReader(IFormFile file)
        {
            if (file == null || file.Length == 0 || !file.FileName.EndsWith(".csv"))
            {
                // inf
                return;
            }
            this._file = file;
        }

        public bool OpenReaderStream()
        {
            if (_file == null)
                return false;
            if (_reader != null)
            {
                _reader.Close();
                _reader = null;
            }
            try 
            {
                _reader= new StreamReader(_file.OpenReadStream());
                return true;
            } catch (Exception e)
            {
                // inf
                return false;
            }
        }

        public void CloseReaderStream()
        {
            if (_reader == null)
                return;
            _reader.Close();
            _reader = null;
        }

        public async Task<string?> GetData()
        {
            if (_reader == null)
            {
                Console.WriteLine("Reader of the file didn't open.");
                return null;
            }
            return await _reader.ReadLineAsync();
        }
    }
}
