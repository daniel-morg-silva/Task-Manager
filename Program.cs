using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.ComponentModel;

namespace TaskManager
{
    class Program
    {
        const string ConnectionString = "Data Source = DataBase.db";

        public static void Main(string[] args)
        {
            InitialiseDatabase();

            bool isRunning = true;

            while (isRunning)
            {
                HelpText();

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        HelpText();
                        break;
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        UpdateTask();
                        break;
                    case "3":
                        ViewTasks();
                        break;
                    case "4":
                        DeleteTasks();
                        break;
                    case "5":
                        isRunning = false;
                        Console.WriteLine("\nExiting application...\n");
                        break;
                    default:
                        Console.WriteLine("\nUser input invalid. For instructions type 0.");
                        break;
                }
            }
        }

        public static void HelpText() // Prints initial text, with all the options available
        {
            Console.WriteLine("\n---- TaskManager ----");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Update Tasks");
            Console.WriteLine("3. View Tasks");
            Console.WriteLine("4. Delete Tasks");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
        }



        public static void InitialiseDatabase() // Initializes the database, if it doesn't exist creates it
        {
            string createTable = @"CREATE TABLE IF NOT EXISTS Tasks (Id INTEGER PRIMARY KEY,
                                                                              Title TEXT,
                                                                              Description TEXT,
                                                                              IsCompleted INTEGER)";

            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }



        public static void AddTask() // Adds a task to the database
        {
            string addTask = @"INSERT INTO Tasks (Title, Description, IsCompleted)
                                           VALUES (@TitleParam, @DescriptionParam, @IsCompletedParam)";

            Console.Write("Title: ");
            string? title = Console.ReadLine();

            Console.Write("Description: ");
            string? description = Console.ReadLine();

            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(addTask, connection))
                {
                    command.Parameters.AddWithValue("@TitleParam", title);
                    command.Parameters.AddWithValue("@DescriptionParam", description);
                    command.Parameters.AddWithValue("@IsCompletedParam", 0);

                    command.ExecuteNonQuery();

                    Console.WriteLine("\n---- New Task Added ----");
                    Console.WriteLine($"Title: {title}\nDescription: {description}");
                }
            }
        }



        public static void UpdateTask() // Updates the condition of the task (turns the Is Completed to 1)
        {
            Console.WriteLine("Update");
        }



        public static void ViewTasks() // Prints all the tasks in the database
        {
            Console.WriteLine("View");
        }



        public static void DeleteTasks() // Deletes task(s) from the database
        {
            Console.WriteLine("Delete");
        }
    }
}


