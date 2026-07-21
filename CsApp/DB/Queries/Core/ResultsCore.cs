using CsApp.DB.Models;
using CsApp.DB.Queries.Core.Filters;
using CsApp.Parsers;

namespace CsApp.DB.Queries.Core
{
    public class ResultsCore : ParentCore
    {
        public static string GetAll()
        {
            return "SELECT * FROM \"Results\"";
        }

        public static string GetWithFilters(ParentFilter filter)
        {
            string filterData = filter.Build();
            return $@"SELECT * FROM {"\"Results\""}
                    WHERE {filterData}";
        }

        public static string GetIdFromFilename()
        {
            return $"SELECT id FROM \"Results\"";
        }

        public static string GetId(string filename)
        {
            return @$"Select id From {"\"Results\""} 
                WHERE filename = '{filename}'";
        }

        public static string Insert(Models.Results result)
        {
            return $@"INSERT INTO {"\"Results\""} 
                (filename, delta_date, min_date, avg_execution_time, avg_value, median_value, max_value, min_value) 
                VALUES ('{result.filename}', '{result.delta_date.ToString()}', '{result.min_date.ToString(DateTimeParser.BASE_DATA_FORMAT)}', {result.avg_execution_time}, {result.avg_value}, {result.median_value}, {result.max_value}, {result.min_value}) 
                RETURNING id";
        }

        public static string UpdateAll(Models.Results result)
        {
            return @$"UPDATE {"\"Results\""} SET 
                filename = '{result.filename}', 
                delta_date = '{result.delta_date.ToString()}',
                min_date = '{result.min_date.ToString(DateTimeParser.BASE_DATA_FORMAT)}',
                avg_execution_time = {result.avg_execution_time},
                avg_value = {result.avg_value},
                median_value = {result.median_value},
                max_value = {result.max_value},
                min_value = {result.min_value}
                WHERE id = {result.id}
                RETURNING id";
        }

        public string filename { get; set; }
        public TimeSpan delta_date { get; set; } = new();
        public DateTime min_date { get; set; } = new();
        public decimal avg_execution_time { get; set; }
        public decimal avg_value { get; set; }
        public decimal median_value { get; set; }
        public decimal max_value { get; set; }
        public decimal min_value { get; set; }
        public List<Values> values { get; set; } = new();

        public static string Delete(int id_file)
        {
            return @$"DELETE FROM {"\"Results\""}
                WHERE id_file = {id_file}";
        }
    }
}
