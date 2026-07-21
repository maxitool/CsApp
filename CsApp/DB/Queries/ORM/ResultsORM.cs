using Npgsql;
using CsApp.DB.Models;
using Results = CsApp.DB.Models.Results;
using CsApp.DB.Queries.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.IO;
using CsApp.DB.Queries.Core.Filters;

namespace CsApp.DB.Queries.ORM
{
    public class ResultsORM : ParentORM
    {
        public ResultsORM(NpgsqlConnection connection) : base(connection){}

        public async Task<int> GetId(string filename)
        {
            if (!await CheckConnection() || filename == null)
                return -1;
            try
            {
                using var command = new NpgsqlCommand(ResultsCore.GetId(filename), _connection);
                using var reader = await command.ExecuteReaderAsync();
                if (reader.FieldCount == 0)
                    return -1;
                reader.Read();
                return reader.GetInt32(0);
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public async Task<List<Results>?> GetAll()
        {
            if (!await CheckConnection())
                return null;
            try
            {
                using var command = new NpgsqlCommand(ResultsCore.GetAll(), _connection);
                using var reader = await command.ExecuteReaderAsync();
                if (reader.FieldCount == 0)
                    return null;
                List<Results> results = new(); Results result;
                while (reader.Read())
                {
                    result = new();
                    result.id = reader.GetInt32(0);
                    result.filename = reader.GetString(1);
                    result.delta_date = reader.GetTimeSpan(2);
                    result.min_date = reader.GetDateTime(3);
                    result.avg_execution_time = reader.GetDecimal(4);
                    result.avg_value = reader.GetDecimal(5);
                    result.median_value = reader.GetDecimal(6);
                    result.max_value = reader.GetDecimal(7);
                    result.min_value = reader.GetDecimal(8);
                    results.Add(result);
                }
                return results;
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't get Results data from reader. Is format changed? (db error) in ResultsORM.GetAll .");
            }
            return null;
        }

        public async Task<List<Results>?> GetAllWithFilter(ParentFilter filter)
        {
            if (!await CheckConnection() || filter == null)
                return null;
            try
            {
                using var command = new NpgsqlCommand(ResultsCore.GetWithFilters(filter), _connection);
                using var reader = await command.ExecuteReaderAsync();
                if (reader.FieldCount == 0)
                    return null;
                List<Results> results = new(); Results result;
                while (reader.Read())
                {
                    result = new();
                    result.id = reader.GetInt32(0);
                    result.filename = reader.GetString(1);
                    result.delta_date = reader.GetTimeSpan(2);
                    result.min_date = reader.GetDateTime(3);
                    result.avg_execution_time = reader.GetDecimal(4);
                    result.avg_value = reader.GetDecimal(5);
                    result.median_value = reader.GetDecimal(6);
                    result.max_value = reader.GetDecimal(7);
                    result.min_value = reader.GetDecimal(8);
                    results.Add(result);
                }
                return results;
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't get Results data from reader. Is format changed? (db error) in ResultsORM.GetAllWithFilter .");
            }
            return null;
        }

        public async Task<int> InsertData(NpgsqlTransaction transaction, Results result)
        {
            if (!await CheckConnection() || result == null)
                return -1;
            try
            {
                var command = new NpgsqlCommand(ResultsCore.Insert(result), _connection, transaction);
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            } catch (Exception e)
            {
                Console.WriteLine($"Can't do insert to Results table query: {e.Message}");
            }
            return -1;
        }
        public async Task<int> InsertData(Results result)
        {
            using var transaction = _connection.BeginTransaction();
            int data = await InsertData(transaction, result);
            if (data < 0)
            {
                transaction.Rollback();
                return data;
            }
            transaction.Commit();
            return data;
        }

        public async Task<int> UpdateAllData(NpgsqlTransaction transaction, Results result)
        {
            if (!await CheckConnection() || result == null)
                return -1;
            try
            {
                var command = new NpgsqlCommand(ResultsCore.UpdateAll(result), _connection, transaction);
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't do update data in Results table query: {e.Message}");
            }
            return -1;
        }
        public async Task<int> UpdateAllData(Results result)
        {
            using var transaction = _connection.BeginTransaction();
            int data = await InsertData(transaction, result);
            if (data < 0)
            {
                transaction.Rollback();
                return data;
            }
            transaction.Commit();
            return data;
        }

        public async Task<bool> DeleteData(NpgsqlTransaction transaction, int id_file)
        {
            if (!await CheckConnection())
                return false;
            try
            {
                var command = new NpgsqlCommand(ResultsCore.Delete(id_file), _connection, transaction);
                await command.ExecuteScalarAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't do delete in Results table query: {e.Message}");
            }
            return false;
        }
        public async Task<bool> DeleteData(int id_file)
        {
            using var transaction = _connection.BeginTransaction();
            bool data = await DeleteData(transaction, id_file);
            if (!data)
            {
                transaction.Rollback();
                return data;
            }
            transaction.Commit();
            return data;
        }
    }
}
