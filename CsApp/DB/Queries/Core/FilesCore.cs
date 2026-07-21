using Files = CsApp.DB.Models.Files;

namespace CsApp.DB.Queries.Core
{
    public class FilesCore : ParentCore
    {
        public static string Insert(Files file)
        {
            return $@"INSERT INTO {"\"Files\""}
                (filename) 
                VALUES ('{file.filename}') 
                RETURNING id";
        }

        public static string GetId(string filename)
        {
            return @$"Select id From {"\"Files\""} 
                WHERE filename = '{filename}'";
        }
    }
}
