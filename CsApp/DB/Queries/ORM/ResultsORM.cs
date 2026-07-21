using Npgsql;
using CsApp.DB.Models;
using Results = CsApp.DB.Models.Results;
using CsApp.DB.Queries.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CsApp.DB.Queries.ORM
{
    public class ResultsORM : ParentORM
    {
        public ResultsORM(NpgsqlConnection connection) : base(connection){}

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
