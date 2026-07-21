using ValuesCore = CsApp.DB.Queries.Core.ValuesCore;
using Npgsql;
using Values = CsApp.DB.Models.Values;
using CsApp.DB.Queries.Core;

namespace CsApp.DB.Queries.ORM
{
    public class ValuesORM : ParentORM
    {
        public ValuesORM(NpgsqlConnection connection) : base(connection)
        {
        }

        public async Task<List<Values>?> GetWithFilenameAndLimit(string filename, int limit)
        {
            if (!await CheckConnection())
                return null;
            try
            {
                using var command = new NpgsqlCommand(ValuesCore.Get10ValuesWithLimit(filename, limit), _connection);
                using var reader = await command.ExecuteReaderAsync();
                if (reader.FieldCount == 0)
                    return null;
                List<Values> values = new(); Values value;
                while (reader.Read())
                {
                    value = new();
                    value.id = reader.GetInt32(0);
                    value.result_id = reader.GetInt32(1);
                    value.date = reader.GetDateTime(2);
                    value.execution_time = reader.GetInt32(3);
                    value.value = reader.GetDecimal(4);
                    values.Add(value);
                }
                return values;
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't get Values data from reader. Is format changed? (db error) in ValuesORM.GetWithFilenameAndLimit .");
            }
            return null;
        }
        public async Task<int> InsertData(NpgsqlTransaction transaction, Values value)
        {
            if (!await CheckConnection() || value == null)
                return -1;
            try
            {
                var command = new NpgsqlCommand(ValuesCore.Insert(value), _connection, transaction);
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't do insert to Values tabble query: " + e.Message);
            }
            return -1;
        }
        public async Task<int> InsertData(Values value)
        {
            using var transaction = _connection.BeginTransaction();
            int data = await InsertData(transaction, value);
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
                var command = new NpgsqlCommand(ValuesCore.Delete(id_file), _connection, transaction);
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
