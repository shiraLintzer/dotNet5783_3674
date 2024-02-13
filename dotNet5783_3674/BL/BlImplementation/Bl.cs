using BlApi;


namespace BlImplementation;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public IEngineer Engineer => new EngineerImplementation();
    public IMilestone Milestone => new MilestoneImplementation();
    public ITask Task => new TaskImplementation();

    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public bool IsCreate { get; set; } = false;

    /// <summary>
    /// reset all data;
    /// </summary>
    public void Reset()
    {
        DalApi.Factory.Get.Reset();
    }

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
                    ?.DeadlineDate ?? throw new BO.Validation($"You dont have a deadline date to task with id:{item.DependentTask}");
                earliestDate = endDate > earliestDate ? endDate : earliestDate;
            }
        }
        return earliestDate;
    }


    /// <summary>
    /// Getting the project start date
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BO.Validation"></exception>
    private DateTime GetStartDate()
    {
        Console.WriteLine("insert the start date:");
        var startDate = DateTime.TryParse(Console.ReadLine(), out DateTime result) ? result : throw new BO.Validation("must insert value!");
        return startDate;
    }


    /// <summary>
    /// Update date of a task
    /// </summary>
    /// <param name="task"></param>
    /// <param name="startDate"></param>
    private void updateTask(DO.Task task, DateTime? startDate)
    {
        _dal.Task.Update(task! with { StartDate = startDate, DeadlineDate = startDate + task.RequiredEffortTime });
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
    public void CreateProject()
    {

        DateTime startDate = GetStartDate();
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
        
    }
}

