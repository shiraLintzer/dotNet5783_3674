using BlApi;
using System.Runtime.InteropServices;

namespace BlImplementation;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public IEngineer Engineer => new EngineerImplementation();
    public IMilestone Milestone => new MilestoneImplementation();
    public ITask Task => new TaskImplementation(this);

    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public bool IsCreate { get; set; } = false;

    public void InitializeDB() => DalTest.Initialization.Do();


    public void ResetDB() => DalTest.Initialization.Reset();

    private static DateTime s_Clock = DateTime.Now;

    ///// <summary>
    ///// reset all data;
    ///// </summary>
    //public void Reset()
    //{
    //    DalApi.Factory.Get.Reset();
    //}

    /// <summary>
    /// Finding the earliest date to start a task
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BO.Validation"></exception>
    private DateTime? EarliestStartDate(DO.Task task)
    {
        
        var dependencies = _dal.Dependency.ReadAll(dep => dep.DependentTask == task.Id);
        DateTime? earliestDate = null;
        if (dependencies.Any())
        {
            //Finding all that depend on this task
            earliestDate = _dal.Task.Read((int)(dependencies!.First()!.DependentOnTask)!)!.DeadlineDate;
            foreach (var item in dependencies)
            {
                //Getting the earliest start date of the task
                DateTime endDate = _dal.Task.Read((int)(item!.DependentOnTask)!)
                    ?.DeadlineDate ?? throw new BO.ValidationException($"You dont have a deadline date to task with id:{item.DependentTask}");
                earliestDate = endDate > earliestDate ? endDate : earliestDate;
            }
        }
        return earliestDate;
    }

    /// <summary>
    /// Update date of a task
    /// </summary>
    /// <param name="task"></param>
    /// <param name="startDate"></param>
    private void updateTask(DO.Task task, DateTime? startDate)
    {
        DateTime? d = startDate!.Value.Add(task.RequiredEffortTime!.Value );
        _dal.Task.Update(task! with { StartDate = startDate, DeadlineDate = d });
    }


    /// <summary>
    /// Updating the following assignment times
    /// </summary>
    /// <param name="prevTasks"></param>
    /// <returns></returns>
    private Queue<DO.Task> UpdateNextTask(IEnumerable<DO.Task> prevTasks)
    {
        Queue<DO.Task> taskQueue = new Queue<DO.Task>(prevTasks);

        //As long as the tasks are not over
        while (taskQueue.Count > 0)
        {
            //Take the first task from the queue
            var currentTask = taskQueue.Dequeue();
            //If it is a first task, you do not need to update a date for it
            Console.WriteLine(currentTask);
            if (currentTask.StartDate == null)
            {
                DateTime? earliestDate = EarliestStartDate(currentTask);
                updateTask(currentTask, earliestDate);
            }

            //Adding the tasks that depend on the current task to the queue
            var dependedInPrevTask = _dal.Dependency
                .ReadAll(dep => dep.DependentOnTask == currentTask?.Id)
                .Select(dep =>
                {
                    var dependentTask = _dal.Task.Read((int)dep!.DependentTask!);
                    if (dependentTask != null)
                        taskQueue.Enqueue(dependentTask); 
                    return dependentTask;
                })
                .ToList();
        }

        return taskQueue;
    }


    /// <summary>
    /// Create a schedule
    /// </summary>
    public void CreateProject(DateTime d)
    {
        if(d < Clock)
        {
            throw new BO.ValidationException("you can not put a date from the past");
        }
        DateTime startDate = d;
        IsCreate = true;

        var tasks = _dal.Task.ReadAll();
        //Updating the first tasks
        var firstTasks = tasks.Where(task => !_dal.Dependency.ReadAll(dep => dep.DependentTask == task?.Id).Any()).ToList();
        firstTasks.ForEach(task => updateTask(task!, startDate));
        //Sending the tasks to the queue
        UpdateNextTask(firstTasks!).ToList();

        //Update start date
        StartDate = startDate;

        var allTasks = _dal.Task.ReadAll();
        DateTime? endDate = allTasks.First()?.DeadlineDate;
        foreach ( var task in allTasks )
        {
            if(task?.DeadlineDate > endDate)
                endDate = task.DeadlineDate;
        }

        //Update endDate according to the latest deadline date
        EndDate = endDate;


        _dal.updateStartProject(startDate);
        _dal.updateEndProject(endDate);
      //  DalApi.Factory.Get.updateStartProject
    }


    /// <summary>
    /// clock
    /// </summary>
    public DateTime Clock
    {
        get { return s_Clock; }
        private set { s_Clock = value; }
    }

    /// <summary>
    /// add year
    /// </summary>
    /// <param name="years"></param>
    public void AdvanceTimeByYear(int year)
    {
        s_Clock = s_Clock.AddYears(year);
    }

    /// <summary>
    /// add year
    /// </summary>
    /// <param name="months"></param>
    public void AdvanceTimeByMonth(int month)
    {
        s_Clock = s_Clock.AddMonths(month);
    }

    /// <summary>
    /// add day
    /// </summary>
    /// <param name="days"></param>
    public void AdvanceTimeByDay(int day)
    {
        s_Clock = s_Clock.AddDays(day);
    }


    /// <summary>
    /// add hour
    /// </summary>
    /// <param name="hours"></param>
    public void AdvanceTimeByHour(int hour)
    {
        s_Clock = s_Clock.AddHours(hour);
    }

    /// <summary>
    /// Initialize time to now
    /// </summary>
    public void InitializeTime()
    {
        s_Clock = DateTime.Now;
    }

}

