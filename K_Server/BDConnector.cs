using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace K_Server
{
    static class BDConnector
    {
        public static void InitDB()
        {
            
            string ConnectionString = @"Data Source=.\SQLEXPRESS;
                                      AttachDbFilename=Database.mdf;
                                      Integrated Security=True;
                                      Connect Timeout=30;
                                      User Instance=True";

        }
        public static void CreateCommand(string queryString,
        string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
