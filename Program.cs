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
        private const string connectionString = "Data Source = DataBase.db";

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
                        Console.WriteLine("\nUSE INPUT INVALID. FOR INSTRUCTIONS TYPE 0.");
                        break;
                }
            }
        }

        public static void HelpText() // Prints initial text, with all the options available
        {
            Console.WriteLine("\n---- TASKMANAGER ----");
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

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(createTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }



        public static int AddTask() // Adds a task to the database
        {
            string addTask = @"INSERT INTO Tasks (Title, Description, IsCompleted)
                                           VALUES (@TitleParam, @DescriptionParam, @IsCompletedParam)";

            string? title;

            while (true)
            {
                Console.Write("Title: ");
                title = Console.ReadLine();

                if (CheckTaskTitle(title) == true)
                {
                    Console.WriteLine("THIS TASK'S NAME IS ALREADY IN USE. PLEASE CHOOSE ANOTHER.");
                }
                else if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Title cannot be empty.");
                }
                else
                {
                    break;
                }
            }

            Console.Write("Description: ");
            string? description = Console.ReadLine();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(addTask, connection))
                {
                    command.Parameters.AddWithValue("@TitleParam", title);
                    command.Parameters.AddWithValue("@DescriptionParam", description);
                    command.Parameters.AddWithValue("@IsCompletedParam", 0);

                    command.ExecuteNonQuery();

                    Console.WriteLine("\n---- NEW TASK ADDED ----");
                    Console.WriteLine($"Title: {title}\nDescription: {description}");
                }
            }
            return 0;
        }



        public static int UpdateTask() // Updates the condition of the task (turns the Is Completed to 1)
        {
            Console.Write("Which task do you want to update? ");
            string? taskTitle = Console.ReadLine();

            if (CheckTaskTitle(taskTitle) == false)
            {
                Console.WriteLine("INVALID TASK TITLE.");
                return 1;
            }

            string updateTask = "Update Tasks SET IsCompleted = 1 WHERE Title = @TitleParam";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(updateTask, connection))
                {
                    command.Parameters.AddWithValue("@TitleParam", taskTitle);

                    command.ExecuteNonQuery();

                    Console.WriteLine($"Task: {taskTitle} Updated");

                }
            }
            return 0;
        }

        public static bool CheckTaskTitle(string? title) // Helps the other functions check the validity of the user input
        {
            string checkTasktitle = "SELECT EXISTS (SELECT 1 FROM Tasks WHERE Title = @TitleParam)";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(checkTasktitle, connection))
                {
                    command.Parameters.AddWithValue("@TitleParam", title);

                    object? result = command.ExecuteScalar();

                    return Convert.ToBoolean(result);
                }
            }
        }



        public static void ViewTasks() // Prints all the tasks in the database
        {

            string viewTasks = "SELECT Title, Description, IsCompleted FROM Tasks;";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(viewTasks, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"\nTitle: {reader["Title"]}");
                            Console.WriteLine($"Description: {reader["Description"]}");
                            Console.WriteLine($"Is Completed: {((long) reader["IsCompleted"] == 1 ? "Yes" : "No")}\n");
                        }
                    }
                }
            }
        }
        



        public static int DeleteTasks() // Deletes task from the database
        {
            Console.Write("Which task do you want to delete? ");
            string? taskTitle = Console.ReadLine();

            if (CheckTaskTitle(taskTitle) == false)
            {
                Console.WriteLine("INVALID TASK TITLE.");
                return 1;
            }

            string deleteTasks = "DELETE FROM Tasks WHERE Title = @TitleParam";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(deleteTasks, connection))
                {
                    command.Parameters.AddWithValue("@TitleParam", taskTitle);

                    command.ExecuteNonQuery();

                    Console.WriteLine($"The task {taskTitle} has been removed from the database.");
                }
            }
            return 0;
        }
    }
}


