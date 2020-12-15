using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;

namespace K_Server
{
    static class BDConnector
    {
        private static SqliteConnection connection = null;

        public static void InitDB()
        {
            if (File.Exists("General.db"))
            {
                Console.WriteLine("DB: General.db was finded");
            }
            else
            {
                Console.WriteLine("DB: (ERR) file General.db not found, exit");
                Environment.Exit(1);
            }

            try
            {
                using (var fs = File.Open("General.db", FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    Console.WriteLine("DB: file is free");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("DB: file is busy, exit");
                Environment.Exit(1);
            }


            connection = new SqliteConnection("Data Source=General.db");
            Console.WriteLine("DB: Connected");
        }
        public static string[] CreateCommand(string queryString, int _clmn)
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
                        var st = reader.GetString(_clmn);
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
            var r = CreateCommand("SELECT facults FROM usergroups WHERE groupname = \""+ _group  + "\"", 0);
            if (r == null) return null;
            if (r.Length <= 0) return null;
            return r[0];
        }

        public static string getPasswd(string _user)
        {
            var r = CreateCommand("SELECT passwd FROM users WHERE username = \"" + _user + "\"", 0);
            if (r == null) return null;
            if (r.Length <= 0) return null;
            return r[0];
        }

        public static string getGroup(string _user)
        {
            var r = CreateCommand("SELECT ugroup FROM users WHERE username = \"" + _user + "\"", 0);
            if (r == null) return null;
            if (r.Length <= 0) return null;
            return r[0];
        }

        public static string[] getChatsFlow(string _flow)
        {
            var r = CreateCommand("SELECT groupname FROM usergroups WHERE facults = \"" + _flow + "\"", 0);
            if (r == null) return null;
            if (r.Length <= 0) return null;
            return r;
        }

        public static string[] getLastMsgGrp(string _grp)
        {
            List<string> ans = new List<string>();

            try
            {
                connection.Open();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("ERR: (open db) " + e.Message);
                return null;
            }

            var command = connection.CreateCommand();

            command.CommandText = "SELECT id,ugroup,username,time,message FROM chats WHERE ugroup = \"" + _grp + "\" ORDER BY id ASC LIMIT 10;";

            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var st = reader.GetString(0) + ","
                            + reader.GetString(1) + ","
                            + reader.GetString(2) + ","
                            + reader.GetString(3) + ","
                            + reader.GetString(4);
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

        public static void sendMsg(string _flw, string _grp, string _un, string _msg)
        {
            DateTime localDate = DateTime.Now;            

            try
            {
                connection.Open();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("ERR: (open db) " + e.Message);
            }

            var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO chats (flow, ugroup, username, time, message) VALUES (\"" + _flw + "\", \"" + _grp + "\", \"" + _un + "\", \"" + localDate.ToString("G") + "\", \"" + _msg + "\");";

            try
            {
                var reader = command.ExecuteReader();
                connection.Close();
            }
            catch (SqliteException e)
            {
                Console.WriteLine("ERR: (exec db) " + e.Message);
            }
        }
    }
}
