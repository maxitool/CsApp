using CsApp.DB.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CsApp.DB.Queries.Core
{
    public class ResultsCore : ParentCore
    {
        public static string GetAll()
        {
            return "SELECT * FROM \"Results\"";
        }

        public static string GetIdFromFilename()
        {
            return $"SELECT id FROM \"Results\"";
        }

        public static string Insert(Models.Results result)
        {
            return $@"INSERT INTO {"\"Results\""} 
                (id_file, delta_date, min_date, avg_execution_time, avg_value, median_value, max_value, min_value) 
                VALUES ({result.id_file}, '{result.delta_date.ToString()}', '{result.min_date.ToString(DATE_TIME_FORMAT)}', {result.avg_execution_time}, {result.avg_value}, {result.median_value}, {result.max_value}, {result.min_value}) 
                RETURNING id";
        }

        public static string Delete(int id_file)
        {
            return @$"DELETE FROM {"\"Results\""}
                WHERE id_file = {id_file}";
        }
    }
}
