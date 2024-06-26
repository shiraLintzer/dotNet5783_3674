﻿using Dal;
using DalApi;
using DO;
using System.Reflection.Emit;
using System.Threading.Channels;
using System.Xml.Linq;

namespace DalTest
{
    internal class Program
    {

        //private static IEngineer? t_dalEngineer = new EngineerImplementation();//stage 1
        //private static ITask? t_dalTask = new TaskImplementation();//stage 1
        //private static IDependency? t_dalDependency = new DependencyImplementation();//stage 1

        /*static readonly IDal t_dal = new DalList(); *///stage 2

        /*static readonly IDal t_dal = new DalXml();*/ //stage 3

        static readonly IDal t_dal = Factory.Get; //stage 4


        static void Main(string[] args)
        {
            //call to initialize the lists
            //Initialization.Do(t_dalEngineer, t_dalTask, t_dalDependency);//stage 1
            

            try
            {
                //entity option
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Task");
                Console.WriteLine("2. Dependency");
                Console.WriteLine("3. Engineer");
                Console.WriteLine("4. initialization all data");
                Console.WriteLine("0. Exit");

                int choice = tryParseIntFunction();

                //Continue displaying the menu as long as exit is not clicked
                while (choice != 0)
                {
                    if (choice == 4)
                    {
                        Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                        if (ans == "Y") //stage 3
                        {
                            t_dal.Reset();
                            Initialization.Do(); //stage 2
                            Console.WriteLine("The data has been initialized");
                        }

                    }
                    if (choice != 4)
                    {
                        PerformOperation(choice);
                    }
                   
                    Console.WriteLine("\nMain Menu:");
                    Console.WriteLine("1. Task");
                    Console.WriteLine("2. Dependency");
                    Console.WriteLine("3. Engineer");
                    Console.WriteLine("4. initialization all data");
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
            Console.WriteLine("6. Reset");
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
                Console.WriteLine("6. Reset");
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
                case 6:
                    ResetEntity(choice);
                    break;
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
                            Console.WriteLine("Enter values in the following format:");
                            Console.WriteLine("isMileStone, EngineerId, Alias, ComplexityLevel, StartDate, ScheduledDate, DeadlineDate, CompleteDate, RequiredEffortTime, Description, Deliverables, Remarks");
                            DO.Task task = new DO.Task()
                            {
                                IsMileStone = bool.Parse(Console.ReadLine() ?? ""),
                                EngineerId = tryParseIntFunction(),
                                Alias = Console.ReadLine(),
                                Complexity = (EngineerExperience)(tryParseIntFunction()),
                                StartDate = tryParseDateTimeFunction(),
                                ScheduledDate = tryParseDateTimeFunction(),
                                DeadlineDate = tryParseDateTimeFunction(),
                                CompleteDate = tryParseDateTimeFunction(),
                                RequiredEffortTime = TimeSpan.Parse(Console.ReadLine() ?? ""),
                                Description = Console.ReadLine(),
                                Deliverables = Console.ReadLine(),
                                Remarks = Console.ReadLine()
                            };
                            id = t_dal?.Task.Create(task);
                        }
                        break;
                    case 2:
                        {
                            //receiving the dependency values
                            Console.WriteLine("Enter values in the following format:");
                            Console.WriteLine("DependentTask, DependentOnTask");
                            DO.Dependency dep = new DO.Dependency()
                            {
                                DependentTask = tryParseIntFunction(),
                                DependentOnTask = tryParseIntFunction()
                            };
                            id = t_dal?.Dependency.Create(dep);
                        }
                        break;
                    case 3:
                        {
                            //receiving the engineer values
                            Console.WriteLine("Enter values in the following format:");
                            Console.WriteLine("Id, Level, Name, Email, Cost");
                            Engineer eng = new Engineer()
                            {
                                Id = tryParseIntFunction(),
                                Level = (EngineerExperience)(tryParseIntFunction()),
                                Name = Console.ReadLine(),
                                Email = Console.ReadLine(),
                                Cost = tryParseIntFunction()
                            };
                            id = t_dal?.Engineer.Create(eng);
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
                            DO.Task? ts = t_dal?.Task.Read(id);
                            Console.WriteLine(ts);
                        }
                        break;
                    case 2:
                        {
                            //Printing the requested dependency
                            Console.WriteLine("Enter id");
                            int id = tryParseIntFunction();
                            Dependency? dep = t_dal?.Dependency.Read(id);
                            Console.WriteLine(dep);
                        }
                        break;
                    case 3:
                        {
                            //Printing the requested engineer
                            Console.WriteLine("Enter id");
                            int id = tryParseIntFunction();
                            Engineer? eng = t_dal?.Engineer.Read(id);
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
                        List<DO.Task> allTask = t_dal!.Task.ReadAll()?.Where(d => d != null).Select(d => d!).ToList() ?? new List<DO.Task>();
                        Console.WriteLine("All Tasks");
                        foreach (var item in allTask)
                            Console.WriteLine(item);
                    }
                    break;
                case 2:
                    {
                        //Print all depenencies
                        List<Dependency> allDep = t_dal!.Dependency.ReadAll()?.Where(d => d != null).Select(d => d!).ToList() ?? new List<Dependency>();
                        Console.WriteLine("All Depenencies");
                        foreach (var item in allDep)
                            Console.WriteLine(item);
                    }
                    break;
                case 3:
                    {
                        //Print all engineers
                        List<Engineer> alleEng = t_dal!.Engineer.ReadAll()?.Where(d => d != null).Select(d => d!).ToList() ?? new List<Engineer>();
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
                            Console.WriteLine("Enter values in the following format to update the preferred task:");
                            Console.WriteLine("ID, isMileStone, EngineerId, Alias, ComplexityLevel, StartDate, ScheduledDate, DeadlineDate, CompleteDate, RequiredEffortTime, Description, Deliverables, Remarks");
                            int id = tryParseIntFunction();
                            DO.Task task = new DO.Task
                            {
                                Id = id,
                                IsMileStone = bool.Parse(Console.ReadLine() ?? ""),
                                EngineerId = tryParseIntFunction(),
                                Alias = getLastStringTask(id, "Alias"),
                                Complexity = (EngineerExperience)(tryParseIntFunction()),
                                StartDate = tryParseDateTimeFunction(),
                                ScheduledDate = tryParseDateTimeFunction(),
                                DeadlineDate = tryParseDateTimeFunction(),
                                CompleteDate = tryParseDateTimeFunction(),
                                RequiredEffortTime = TryParseTimeSpanFunction(),
                                Description = getLastStringTask(id, "Description"),
                                Deliverables = getLastStringTask(id, "Deliverables"),
                                Remarks = getLastStringTask(id, "Remarks")
                            };
                            t_dal?.Task.Update(task);
                        }
                        break;
                    case 2:
                        {
                            //receiving the update dependency values
                            Console.WriteLine("Enter values in the following format to update the preferred dependency:");
                            Console.WriteLine("Id, DependentTask, DependentOnTask");
                            DO.Dependency dep = new DO.Dependency
                            {
                                Id = tryParseIntFunction(),
                                DependentTask = tryParseIntFunction(),
                                DependentOnTask = tryParseIntFunction()
                            };
                            t_dal?.Dependency.Update(dep);
                        }
                        break;
                    case 3:
                        {
                            //receiving the update engineer values
                            Console.WriteLine("Enter values in the following format to update the preferred engineer:");
                            Console.WriteLine("Id, Level, Name, Email, Cost");
                            int id = tryParseIntFunction();
                            Engineer eng = new Engineer
                            {
                                Id = id,
                                Level = (EngineerExperience)(tryParseIntFunction()),
                                Name = getLastStringEngineer(id, "Name"),
                                Email = getLastStringEngineer(id, "Email"),
                                Cost = tryParseIntFunction()
                            };
                            t_dal?.Engineer.Update(eng);
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
                            t_dal?.Task.Delete(id);
                        }
                        break;
                    case 2:
                        {
                            //deleting the requested dependency
                            Console.WriteLine("Enter dependency id");
                            int id = tryParseIntFunction();
                            t_dal?.Dependency.Delete(id);
                        }
                        break;
                    case 3:
                        {
                            //deleting the requested engineer
                            Console.WriteLine("Enter engineer id");
                            int id = tryParseIntFunction();
                            t_dal?.Engineer.Delete(id);
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

        /// <summary>
        /// ResetEntity function
        /// </summary>
        /// <param name="choice"></param>
        static void ResetEntity(int choice)
        {
            try
            {
                switch (choice)
                {
                    case 1:
                        //Deleting all tasks
                        t_dal?.Task.Reset();
                        break;
                    case 2:
                        //Deleting all dependencies
                        t_dal?.Dependency.Reset();
                        break;
                    case 3:
                        //Deleting all engineers
                        t_dal?.Engineer.Reset();
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
        /// function to get the last option that was before the change at task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeS"></param>
        /// <returns></returns>
        static string? getLastStringTask(int id, string typeS)
        {
            //function to get the last option that was before the change at task
            string? choice;
            choice = Console.ReadLine();

            //in case that this engineer is exist and no input was returned
            if (choice == "" && t_dal.Task.Read(id) != null)
            {
                //check which attribute of the task
                if (typeS == "Description")
                    choice = t_dal.Task.Read(id)!.Description;
                else if (typeS == "Deliverables")
                    choice = t_dal.Task.Read(id)!.Deliverables;
                else if (typeS == "Alias")
                    choice = t_dal.Task.Read(id)!.Alias;
                else
                    choice = t_dal.Task.Read(id)!.Remarks;
            }

            return choice;
        }

        /// <summary>
        /// function to get the last option that was before the change at engineer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeS"></param>
        /// <returns></returns>
        static string? getLastStringEngineer(int id, string typeS)
        {
            string? choice;
            choice = Console.ReadLine();

            //in case that this engineer is exist and no input was returned
            if (choice =="" && t_dal.Engineer.Read(id) != null)
            {
                //check which attribute of the engineer
                if (typeS == "Name")
                    choice = t_dal.Engineer.Read(id)!.Name;
                else
                    choice = t_dal.Engineer.Read(id)!.Email;
            }

            return choice;
        }

       
    }
}


