namespace DalTest;
using DalApi;
using DO;
using System.Data.Common;
using System.Security.Cryptography;
using System;
using Dal;

public static class Initialization
{
    //private static IEngineer? t_dalEngineer = new EngineerImplementation();//stage 1
    //private static ITask? t_dalTask = new TaskImplementation();//stage 1
    //private static IDependency? t_dalDependency = new DependencyImplementation();//stage 1

    //private static IDal? t_dal = new DalList(); //stage 2
    static readonly IDal t_dal = new DalXml(); //stage 3


    private static readonly Random s_rand = new();

    //initialize of Engineers function
    private static void creatEngineer()
    {
        string[] EngineerNames =
        {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

        foreach (var _name in EngineerNames)
        {
            int? help;
            int _id;
            EngineerExperience _level;
            int _cost;

            //Checking that it will not repeat the same IDs
            do
                _id = s_rand.Next(200000000, 400000000);
            while (t_dal?.Engineer.Read(_id) != null);

            _level = (EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length);
            _cost = s_rand.Next(200, 1000);

            string _email = _id+ s_rand.Next(10, 20) + "@gmail.com";

            Engineer newEng = new(_id, _level, _name, _email, _cost);

            help = t_dal?.Engineer.Create(newEng);
        }

    }

    //initialize of tasks function
    private static void creatTask()
    {
        int help;
        string[] TaskNames =
        {
        "D1", "E24", "Y7","A43", "D8", "S10","F34","J21","M8","B4","C9","X3","Z11","P6","L32","W22","V6","K4","O3","U9"
        };

        foreach (var _name in TaskNames)
        {
            //Random number of project start date between now and another year
            DateTime start = DateTime.Today;
            int rangeInDays = 365; 
            int randomNumberOfDays = s_rand.Next(rangeInDays);
            DateTime randomDate = start.AddDays(randomNumberOfDays);

            DateTime _startDate = randomDate;
            //DateTime _scheduledDate = randomDate.AddDays(100);
            //DateTime _forecastDate = randomDate.AddDays(90);
            //DateTime _deadlineDate = randomDate.AddDays(110);
            //DateTime? _completeDate = (i % 2) == 0 ? randomDate.AddDays(90) : null;

            int randomDays = s_rand.Next(30, 70); // בטווח של חודש ויותר (30 ימים עד 60 ימים)
            TimeSpan _requiredEffortTime = TimeSpan.FromDays(randomDays);
            string? _description = _name+" Description";
            string? _alias = _name + " Alias";
            string? _deliverables = _name + " Deleverables";
            string? _remarks = _name + " Remarks";
            


            Task newTS = new(0, false, null, _alias, null, _startDate, null, null, null, _requiredEffortTime, _description, _deliverables, _remarks);
            help = t_dal!.Task.Create(newTS);

        }
    }

    //initialize of Engineers dependencies function
    private static void creatDependency()
    {
        //variable that will receive the returned id
        int help;

        //according to the request that both X and Y will depend on A,B,C
        int _depTask, _depOnTask;
        Dependency newDep1 = new(0, 1000, 1001);
        help = t_dal!.Dependency.Create(newDep1);
        Dependency newDep2 = new(0, 1000, 1002);
        help = t_dal!.Dependency.Create(newDep2);
        Dependency newDep3 = new(0, 1000, 1003);
        help = t_dal!.Dependency.Create(newDep3);


        Dependency newDep4 = new(0, 1004, 1001);
        help = t_dal!.Dependency.Create(newDep4);
        Dependency newDep5 = new(0, 1004, 1002);
        help = t_dal!.Dependency.Create(newDep5);
        Dependency newDep6 = new(0, 1004, 1003);
        help = t_dal!.Dependency.Create(newDep6);


        for (int i = 0; i<36; i++)
        {
            //recive a random task
            _depTask = RandomTask().Id;

            //Preventing circular dependency
            do
                _depOnTask = RandomTask().Id;
            while (_depOnTask == _depTask);

            //Creating the dependency
            Dependency newDep = new(0, _depTask, _depOnTask);
            help = t_dal!.Dependency.Create(newDep);
        }
    }

    //random number of a task function
    private static Task RandomTask()
    {
        //Receiving the list of tasks as an array
        Task[] temp = new Task[20];
        temp = (t_dal?.Task.ReadAll()?.Where(task => task != null).ToArray() ?? Array.Empty<Task>())!;
        //Return a random task
        return temp[s_rand.Next(0,19)];
    }

    //call to initialize the entities function
    public static void init()
    {
        creatEngineer();
        creatTask();
        creatDependency();
    }

    //initialize the lists function
    public static void Do(IDal? dal)
    {
        init();
        //dalEngineer = t_dalEngineer;
        //dalTask = t_dalTask;
        //dalDependency = t_dalDependency;

        dal = t_dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2

    }
}
