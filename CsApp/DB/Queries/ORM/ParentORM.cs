using Npgsql;
using System.Data;

namespace CsApp.DB.Queries.ORM
{
    public abstract class ParentORM
    {
        protected NpgsqlConnection _connection;
        public ParentORM(NpgsqlConnection connection)
        {
            this._connection = connection;
        }
        protected async Task<bool> CheckConnection()
        {
            if (_connection == null)
                return false;
            if (_connection.State == ConnectionState.Open)
                return true;
            try
            {
                await _connection.OpenAsync();
                return true;
            } catch (Exception e)
            {
                Console.WriteLine("Can't open connection to bd. Maybe bad CONNECTION_STRINGS in CsAppDBContext class.");
            }
            return false;
        }
    }
}
