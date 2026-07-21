using CsApp.DB.Models;
using CsApp.Parsers;
using CsvHelper;
using System.Runtime.Serialization;

namespace CsApp.DB.Queries.Core
{
    public class ValuesCore : ParentCore
    {
        public static string Insert(Values value)
        {
            return $@"INSERT INTO {"\"Values\""} (result_id, date, execution_time, value) 
            VALUES ({value.result_id}, '{value.date.ToString(DateTimeParser.BASE_DATA_FORMAT)}', {value.execution_time}, {value.value}) 
            RETURNING id";
        }

        public static string Delete(int result_id)
        {
            return @$"DELETE FROM {"\"Values\""}
                WHERE result_id = {result_id}";
        }

        public static string Get10ValuesWithLimit(string filename, int limit)
        {
            return $@"SELECT values_data.* FROM {"\"Values\""} values_data
                INNER JOIN (
                SELECT * FROM {"\"Results\""}
                WHERE filename='{filename}') results_data ON results_data.id = values_data.result_id
                ORDER BY values_data.date DESC
                LIMIT {limit}";
        }
    }
}
