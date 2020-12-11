using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace K_Server
{
    static class BDConnector
    {
        private static SqliteConnection connection = null;

        public static void InitDB()
        {
            connection = new SqliteConnection("Data Source=General.db");
            Console.WriteLine("Connect to DB");
        }
        public static string[] CreateCommand(string queryString)
        {
            List<string> ans = new List<string>();

            try
            {
                connection.Open();
            }
            catch(SqliteException e)
            {
                Console.WriteLine("ERR: (open db) " + e.Message);
                return null;
            }

            var command = connection.CreateCommand();

            command.CommandText = queryString;

            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var st = reader.GetString(0);
                        ans.Add(st);
                    }
                }

                connection.Close();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("ERR: (exec db) " + e.Message);
                return null;
            }

            return ans.ToArray();
        }

        public static string getFacult(string _group)
        {
            var r = CreateCommand("SELECT facults FROM usergroups WHERE groupname = \""+ _group  + "\"");
            if (r == null) return null;
            if (r.Length <= 0) return null;
            return r[0];
        }

        public static string getPasswd(string _user)
        {
            var r = CreateCommand("SELECT passwd FROM users WHERE username = \"" + _user + "\"");
            if (r == null) return null;
            if (r.Length <= 0) return null;
            return r[0];
        }

        public static string getGroup(string _user)
        {
            var r = CreateCommand("SELECT ugroup FROM users WHERE username = \"" + _user + "\"");
            if (r == null) return null;
            if (r.Length <= 0) return null;
            return r[0];
        }
    }
}
