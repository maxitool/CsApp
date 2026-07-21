using CsApp.DB.Queries.Core;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Files = CsApp.DB.Models.Files;

namespace CsApp.DB.Queries.ORM
{
    public class FilesORM : ParentORM
    {
        public FilesORM(NpgsqlConnection connection) : base(connection)
        {
        }

        public async Task<int> InsertData(NpgsqlTransaction transaction, Files file)
        {
            if (!await CheckConnection() || file == null)
                return -1;
            try
            {
                var command = new NpgsqlCommand(FilesCore.Insert(file), _connection, transaction);
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't do insert to Results tabble query: " + e.Message);
            }
            return -1;
        }
        public async Task<int> InsertData(Files file)
        {
            using var transaction = _connection.BeginTransaction();
            int data = await InsertData(transaction, file);
            if (data < 0)
            {
                transaction.Rollback();
                return data;
            }
            transaction.Commit();
            return data;
        }

        public async Task<int> GetId(string filename)
        {
            if (!await CheckConnection() || filename == null)
                return -1;
            try
            {
                using var command = new NpgsqlCommand(FilesCore.GetId(filename), _connection);
                using var reader = await command.ExecuteReaderAsync();
                if (reader.FieldCount == 0)
                    return -1;
                reader.Read();
                return reader.GetInt32(0);
            } catch (Exception e)
            {
                Console.WriteLine($"Can't Get id from Files table with the filename: {e.Message}");
            }
            return -1;
        }
    }
}
