using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace TaskManager
{
    class Program
    {
        public static void Main(string[] args)
        {
            const string ConnectionString = "Data Source = DataBase.db";

            string createTable = @"CREATE TABLE IF NOT EXISTS Task(Id INTEGER PRIMARY KEY, 
                                                                       Description TEXT,
                                                                       IsCompleted INTEGER)";

            using (SqliteConnection SQLiteConnection = new SqliteConnection(ConnectionString))
            {
                SQLiteConnection.Open();

                using (SqliteCommand command = new SqliteCommand(createTable, SQLiteConnection))
                {
                    command.ExecuteNonQuery();
                }


                Console.WriteLine(SQLiteConnection.State);

                Program.InsertData();

            }




        }
        public static void InsertData(SqliteConnection connection)
        {
            Console.WriteLine("Hello, World");
        }
    }
}