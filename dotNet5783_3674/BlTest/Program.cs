using BO;
using DalApi;
using DalTest;


namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        static void Main(string[] args)
        {
            
            try
            {
                //entity option
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Task");
                Console.WriteLine("2. Engineer");
                Console.WriteLine("3. initialization all data");
                Console.WriteLine("4. create schedule");
                Console.WriteLine("0. Exit");

                int choice = tryParseIntFunction();

                //Continue displaying the menu as long as exit is not clicked
                while (choice != 0)
                {
                    if (choice == 4)
                    {
                        Console.Write("Would you like to create Initial data? (Y/N)");
                        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                        if (ans == "Y")
                        {
                            s_bl.Reset();
                            Initialization.Do();
                            Console.WriteLine("The data has been initialized");
                        }

                    }
                    if(choice == 5)
                    {
                        s_bl.CreateProject();
                    }

                    if (choice != 4 && choice != 5)
                    {
                        PerformOperation(choice);
                    }

                    Console.WriteLine("\nMain Menu:");
                    Console.WriteLine("1. Task");
                    Console.WriteLine("2. Engineer");
                    Console.WriteLine("3. initialization all data");
                    Console.WriteLine("4. create schedule");
                    Console.WriteLine("0. Exit");

                    choice = tryParseIntFunction();
                }

                Console.WriteLine("Exiting program. Goodbye!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }


        /// <summary>
        /// function that uses tryParse on an Int
        /// </summary>
        /// <returns></returns>
        static int tryParseIntFunction()
        {
            int choice;

            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                Console.Write("Enter your choice: ");
            }
            return choice;
        }

        /// <summary>
        /// function that uses tryParse on an TimeSpan
        /// </summary>
        /// <returns></returns>
        static DateTime tryParseDateTimeFunction()
        {
            DateTime choice;

            while (!DateTime.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid date and time format.");
                Console.Write("Enter a date and time (yyyy-MM-dd HH:mm:ss): ");
            }

            return choice;
        }

        /// <summary>
        /// function that uses tryParse on an DateTime
        /// </summary>
        /// <returns></returns>
        static TimeSpan TryParseTimeSpanFunction()
        {
            TimeSpan choice;

            while (!TimeSpan.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid time span format.");
                Console.Write("Enter a time span (d.hh:mm:ss): ");
            }

            return choice;
        }

        /// <summary>
        /// the actions of any entity
        /// </summary>
        /// <param name="choice"></param>
        static void PerformOperation(int choice)
        {
            Console.WriteLine("\nEntity Menu:");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Read");
            Console.WriteLine("3. ReadAll");
            Console.WriteLine("4. Update");
            Console.WriteLine("5. Delete");
            //Console.WriteLine("6. Reset");
            Console.WriteLine("0. Back to Main Menu");

            int operationChoice = tryParseIntFunction();

            //Continue displaying the actions as long as exit is not clicked
            while (operationChoice != 0)
            {
                Organize(choice, operationChoice);

                Console.WriteLine("\nEntity Menu:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read");
                Console.WriteLine("3. ReadAll");
                Console.WriteLine("4. Update");
                Console.WriteLine("5. Delete");
                //Console.WriteLine("6. Reset");
                Console.WriteLine("0. Back to Main Menu");

                operationChoice = tryParseIntFunction();
            }
        }

        /// <summary>
        /// Division into actions according to the choice of an action
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="operationChoice"></param>
        static void Organize(int choice, int operationChoice)
        {
            //Sending the choice so they know which entity to perform the action on
            switch (operationChoice)
            {
                case 1:
                    CreateEntity(choice);
                    break;
                case 2:
                    ReadEntity(choice);
                    break;
                case 3:
                    ReadAllEntity(choice);
                    break;
                case 4:
                    UpdateEntity(choice);
                    break;
                case 5:
                    DeleteEntity(choice);
                    break;
                //case 6:
                //    ResetEntity(choice);
                //    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        /// <summary>
        /// CreateEntity function
        /// </summary>
        /// <param name="choice"></param>
        static void CreateEntity(int choice)
        {
            try
            {
                int? id;
                switch (choice)
                {
                    case 1:
                        {
                            //receiving the task values

                            //receiving the task the it depend on them
                            Console.WriteLine("Enter the number of tasks that depend on them");
                            int num = tryParseIntFunction();
                            Console.WriteLine("enter the tasks ID");
                            List<TaskInList> dependencies = new List<TaskInList>();
                            for(int i = 0; i< num; i++)
                            {
                                int help = tryParseIntFunction();
                                TaskInList temp = new TaskInList { Id = help, Description = null, Alias = null, Status = null };

                                // Add the temp object to the dependencies list
                                dependencies.Add(temp);
                            }

                          


                            //receiving the rest of the data
                            Console.WriteLine("Enter values in the following format:");
                            Console.WriteLine("Description, Alias, Deliverables, Remarks, Complexity, RequiredEffortTime ");
                            BO.Task task = new BO.Task()
                            {
                                Description = Console.ReadLine(),
                                Alias = Console.ReadLine(),
                                createAtDate = DateTime.Now,
                                Status = 0,
                                Dependencies = dependencies,
                                Deliverables = Console.ReadLine(),
                                Remarks = Console.ReadLine(),
                                Complexity = (BO.EngineerExperience)(tryParseIntFunction()),
                                RequiredEffortTime = TryParseTimeSpanFunction(),
                            };
                            id = s_bl?.Task.Create(task);
                        }
                        break;
                    case 2:
                        {
                            
                            //receiving the rest of the data
                            Console.WriteLine("Enter values in the following format:");
                            Console.WriteLine("ID, Level, Name, Email, Cost, Task");
                            Engineer eng = new Engineer()
                            {
                                Id = tryParseIntFunction(),
                                Level = (BO.EngineerExperience)(tryParseIntFunction()),
                                Name = Console.ReadLine(),
                                Email = Console.ReadLine(),
                                Cost = tryParseIntFunction(),

                            };
                            id = s_bl?.Engineer.Create(eng);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// ReadEntity function
        /// </summary>
        /// <param name="choice"></param>
        static void ReadEntity(int choice)
        {
            try
            {
                switch (choice)
                {
                    case 1:
                        {
                            //Printing the requested task
                            Console.WriteLine("Enter id");
                            int id = tryParseIntFunction();
                            BO.Task? ts = s_bl?.Task.Read(id);
                            Console.WriteLine(ts);
                        }
                        break;
                    case 2:
                        {
                            //Printing the requested engineer
                            Console.WriteLine("Enter id");
                            int id = tryParseIntFunction();
                            Engineer? eng = s_bl?.Engineer.Read(id);
                            Console.WriteLine(eng);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// ReadAllEntity function
        /// </summary>
        /// <param name="choice"></param>
        static void ReadAllEntity(int choice)
        {
            switch (choice)
            {
                case 1:
                    {
                        //Print all tasks
                        IEnumerable<BO.Task?> allTask = s_bl.Task.ReadAll();
                        Console.WriteLine("All Tasks");
                        foreach (var item in allTask)
                            Console.WriteLine(item);
                    }
                    break;
                case 2:
                    {
                        //Print all engineers
                        IEnumerable<Engineer?> alleEng = s_bl.Engineer.ReadAll();
                        Console.WriteLine("All Engineers");
                        foreach (var item in alleEng)
                            Console.WriteLine(item);
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        /// <summary>
        /// UpdateEntity function
        /// </summary>
        /// <param name="choice"></param>
        static void UpdateEntity(int choice)
        {
            try
            {
                switch (choice)
                {
                    case 1:
                        {
                            //receiving the update task values

                            //receiving the task the it depend on them
                            Console.WriteLine("Enter the number of tasks that depend on them");
                            int num = tryParseIntFunction();
                            Console.WriteLine("enter the tasks ID");
                            List<TaskInList> dependencies = new List<TaskInList>();
                            for (int i = 0; i < num; i++)
                            {
                                int help = tryParseIntFunction();
                                TaskInList temp = new TaskInList { Id = help, Description = null, Alias = null, Status = null };

                                // Add the temp object to the dependencies list
                                dependencies.Add(temp);
                            }

                            //receiving the belong engineer
                            Console.WriteLine("insert the belong engineer ID, if there is not enter 0");
                            int engId = tryParseIntFunction();
                            BO.Engineer? findEng = s_bl.Engineer.Read(engId);
                            BO.EngineerInTask? engineerInTask = null;
                            if (findEng != null)
                                engineerInTask = new EngineerInTask { Id = engId, Name = findEng.Name };
                            else
                                Console.WriteLine("there is not engineer with this ID, the engineer will be null by defult");

                            //receiving the rest of the data
                            Console.WriteLine("Enter values in the following format to update the preferred task:");
                            Console.WriteLine("Id, Description, Alias, createAtDate, Status, /*Milestone*/, StartDate, ScheduledDate, ForecastDate, DeadlineDate, Description, Deliverables, Remarks, Engineer,Complexity ");
                            
                            BO.Task task = new BO.Task
                            {
                                Id = tryParseIntFunction(),
                                Description = Console.ReadLine(),
                                Alias = Console.ReadLine(),
                                createAtDate = tryParseDateTimeFunction(),
                                Status = (BO.Status)tryParseIntFunction(),
                                Dependencies = dependencies,
                                //Milestone
                                StartDate = tryParseDateTimeFunction(),
                                ScheduledDate = tryParseDateTimeFunction(),
                                ForecastDate = tryParseDateTimeFunction(),
                                DeadlineDate = tryParseDateTimeFunction(),
                                CompleteDate = tryParseDateTimeFunction(),
                                Deliverables = Console.ReadLine(),
                                Remarks = Console.ReadLine(),
                                Engineer = engineerInTask,
                                Complexity = (BO.EngineerExperience)(tryParseIntFunction()),
                                RequiredEffortTime = TryParseTimeSpanFunction(),
                            };
                            s_bl?.Task.Update(task);
                        }
                        break;
                    case 2:
                        {
                            //receiving the update engineer values

                            //receiving the engineer task(if there is one)
                            Console.WriteLine("Enter ID of belong task, if there is not enter 0");
                            int taskId = tryParseIntFunction();
                            BO.Task? findTask = s_bl.Task.Read(taskId);
                            BO.TaskInEngineer? taskInEngineer = null;
                            if (findTask != null)
                                taskInEngineer = new BO.TaskInEngineer { Id = taskId, Alias = findTask.Alias };
                            else
                                Console.WriteLine("there is not task with this ID, the task will be null by defult");


                            //receiving the rest of the data
                            Console.WriteLine("Enter values in the following format to update the preferred engineer:");
                            Console.WriteLine("Id, Level, Name, Email, Cost");
                            int id = tryParseIntFunction();
                            Engineer eng = new Engineer
                            {
                                Id = id,
                                Level = (EngineerExperience)(tryParseIntFunction()),
                                Name = Console.ReadLine(),
                                Email = Console.ReadLine(),
                                Cost = tryParseIntFunction(),
                                Task = taskInEngineer
                            };
                            s_bl?.Engineer.Update(eng);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// DeleteEntity function
        /// </summary>
        /// <param name="choice"></param>
        static void DeleteEntity(int choice)
        {
            try
            {
                switch (choice)
                {
                    case 1:
                        {
                            //deleting the requested task
                            Console.WriteLine("Enter task id");
                            int id = tryParseIntFunction();
                            s_bl?.Task.Delete(id);
                        }
                        break;
                    case 2:
                        {
                            //deleting the requested engineer
                            Console.WriteLine("Enter engineer id");
                            int id = tryParseIntFunction();
                            s_bl?.Engineer.Delete(id);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        ///// <summary>
        ///// ResetEntity function
        ///// </summary>
        ///// <param name="choice"></param>
        //static void ResetEntity(int choice)
        //{
        //    try
        //    {
        //        switch (choice)
        //        {
        //            case 1:
        //                //Deleting all tasks
        //                t_dal?.Task.Reset();
        //                break;
        //            case 2:
        //                //Deleting all dependencies
        //                t_dal?.Dependency.Reset();
        //                break;
        //            case 3:
        //                //Deleting all engineers
        //                t_dal?.Engineer.Reset();
        //                break;
        //            default:
        //                Console.WriteLine("Invalid choice. Please try again.");
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}





        //        /// <summary>
        //        /// function to get the last option that was before the change at task
        //        /// </summary>
        //        /// <param name="id"></param>
        //        /// <param name="typeS"></param>
        //        /// <returns></returns>
        //        static string? getLastStringTask(int id, string typeS)
        //        {
        //            //function to get the last option that was before the change at task
        //            string? choice;
        //            choice = Console.ReadLine();

        //            //in case that this engineer is exist and no input was returned
        //            if (choice == "" && s_bl.Task.Read(id) != null)
        //            {
        //                //check which attribute of the task
        //                if (typeS == "Description")
        //                    choice = s_bl.Task.Read(id)!.Description;
        //                else if (typeS == "Deliverables")
        //                    choice = s_bl.Task.Read(id)!.Deliverables;
        //                else if (typeS == "Alias")
        //                    choice = s_bl.Task.Read(id)!.Alias;
        //                else
        //                    choice = s_bl.Task.Read(id)!.Remarks;
        //            }

        //            return choice;
        //        }

        //        /// <summary>
        //        /// function to get the last option that was before the change at engineer
        //        /// </summary>
        //        /// <param name="id"></param>
        //        /// <param name="typeS"></param>
        //        /// <returns></returns>
        //        static string? getLastStringEngineer(int id, string typeS)
        //        {
        //            string? choice;
        //            choice = Console.ReadLine();

        //            //in case that this engineer is exist and no input was returned
        //            if (choice == "" && s_bl.Engineer.Read(id) != null)
        //            {
        //                //check which attribute of the engineer
        //                if (typeS == "Name")
        //                    choice = s_bl.Engineer.Read(id)!.Name;
        //                else
        //                    choice = s_bl.Engineer.Read(id)!.Email;
        //            }

        //            return choice;
        //        }

    }
}