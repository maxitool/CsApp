using CsApp.DB.Models;
using CsvHelper;

namespace CsApp.DB.Queries.Core
{
    public class ValuesCore
    {
        public static string Insert(Values value)
        {
            return $@"INSERT INTO Values (id_file, date, execution_time, value) 
            VALUES ({value.id_file}, '{value.date.ToString()}', {value.execution_time}, {value}) 
            RETURNING id";
        }

        public static string Delete(int id_file)
        {
            return @$"DELETE FROM Values 
                WHERE id_file = {id_file}";
        }
    }
}
