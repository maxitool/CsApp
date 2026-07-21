using CsApp.DB.Models;
using CsvHelper;
using System.Runtime.Serialization;

namespace CsApp.DB.Queries.Core
{
    public class ValuesCore : ParentCore
    {
        public static string Insert(Values value)
        {
            return $@"INSERT INTO {"\"Values\""} (id_file, date, execution_time, value) 
            VALUES ({value.id_file}, '{value.date.ToString(DATE_TIME_FORMAT)}', {value.execution_time}, {value.value}) 
            RETURNING id";
        }

        public static string Delete(int id_file)
        {
            return @$"DELETE FROM {"\"Values\""}
                WHERE id_file = {id_file}";
        }
    }
}
