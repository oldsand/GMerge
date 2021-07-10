using Microsoft.Data.Sqlite;

namespace GCommon.Core.Utilities
{
    public class SqliteHelper
    {
        private bool DatabaseIsValid(string filename)
        {
            using var connection = new SqliteConnection(@"Data Source=" + filename + ";FailIfMissing=True;");
            
            try
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                transaction.Rollback();
            }
            catch (SqliteException)
            {
                return false;
            }

            return true;
        }
    }
}