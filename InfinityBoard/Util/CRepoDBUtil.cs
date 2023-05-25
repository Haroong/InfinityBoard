using MySqlConnector;

namespace BigglzPetJson.Dll.Util
{
    public class CRepoDBUtil
    {
        /// <summary>
        /// Mysql connect
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static MySqlConnection GetOpenMySqlConnection(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
