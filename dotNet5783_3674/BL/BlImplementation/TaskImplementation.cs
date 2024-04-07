using BlApi;
using BO;
using DalApi;
using DO;
using System.Linq;
using System.Threading.Tasks;

namespace BlImplementation;


internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;

    /// <summary>
    /// create task function
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Validation"></exception>
    /// <exception cref="BO.BlDoesAlreadyExistException"></exception>
    public int Create(BO.Task item)
    {
        if (_dal.ReturnStartProject() != null)
            throw new ValidationException("you cant add a after creating a schedule");
        //validation 
        if (item.Id < 0)
            throw new ValidationException("wrong Id");
        if (item.Alias == null || item.Alias == "")
            throw new ValidationException("Alias is empty");
        if (item.RequiredEffortTime == null)
            throw new ValidationException("RequiredEffortTime is empty");

        //create the do task
        item.createAtDate = _bl.Clock;
        item.Status = 0;
        DO.Task doTask = new DO.Task
        (item.Id, false, item.Engineer?.Id, item.Alias, (DO.EngineerExperience?)item.Complexity, item.createAtDate, item.StartDate, item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.RequiredEffortTime, item.Description, item.Deliverables, item.Remarks);
        try
        {
            int idTask = _dal.Task.Create(doTask);

            //the dependencies that related to this task
            var listDep = item.Dependencies;
            if (listDep != null)
                foreach (var taskDep in listDep)
                {
                    var newDep = new DO.Dependency { DependentTask = idTask, DependentOnTask = taskDep.Id };
                    _dal.Dependency.Create(newDep);
                }

            return idTask;
        }
        catch (DO.DalDoesAlreadyExistException ex)
        {
            throw new BO.BlDoesAlreadyExistException($"Task with ID={item.Id} already exists", ex);
        }


    }

    /// <summary>
    /// delete task function
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Validation"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {

        //in case a schedule was created you cant delete the task
        if (_dal.Task.ReadAll().Any(x => x!.IsMileStone == true))
            throw new ValidationException("you cant delete after creating a schedule");

        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exists", ex);
        }
    }


    /// <summary>
    /// read task item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="DalDoesNotExistException"></exception>
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        List<BO.TaskInList> dependencies = _dal.Dependency
             .ReadAll(dep => dep.DependentTask == id)
             //on each of the previous tasks
             .Select(dep =>
             {
                 //Creates an element of type task in list according to the values found in the task
                 var currentTask = _dal.Task.Read(task => task.Id == dep?.DependentTask);
                 int newid = dep?.DependentOnTask ?? throw new DalDoesNotExistException("???");
                 var task = _dal.Task.Read(newid);
                 //Calculates the status
                 Status status = Calculate(newid);
                 //Returns a new member of type task in list
                 return new BO.TaskInList
                 {
                     Id = newid,
                     Alias = task?.Alias ?? "",
                     Description = task?.Description ?? "",
                     Status = status
                 };
             }).ToList();


        int? milestoneDep = _dal.Dependency.Read(dep => dep.DependentTask == id)?.DependentOnTask;
        string? alias = _dal.Task.Read(tsk => tsk.Id == milestoneDep)?.Alias;



        Status st = Calculate(id);


        return new BO.Task()
        {
            Id = id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            createAtDate = doTask.CreateDate,
            Status = st,
            Dependencies = dependencies,
            Milestone = st == 0 ? null : new MilestoneInTask { Id = id, Alias = alias ?? "" },
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            ForecastDate = null,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = doTask.EngineerId != null
                ? new EngineerInTask { Id = (int)doTask.EngineerId, Name = _dal.Engineer.Read((int)doTask.EngineerId)?.Name ?? "" }
                : null,
            Complexity = (BO.EngineerExperience?)doTask.Complexity,
            RequiredEffortTime = doTask.RequiredEffortTime,

        };
    }

    /// <summary>
    /// readAll tasks function
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Validation"></exception>
    public IEnumerable<BO.Task?> ReadAll()
    {
        var list = _dal.Task.ReadAll();
        if (list != null)
            return list.Where(tsk => tsk?.IsMileStone == false).Select(tsk => Read(tsk!.Id));
        throw new ValidationException("list is null");

    }



    /// <summary>
    /// update task function
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Validation"></exception>
    public void Update(BO.Task item)
    {
        DO.Task taskUpdate;
        //Before creating a schedule
        if (_dal.ReturnStartProject() == null)
        {
            ///Check if there is a circle dependencies
            if (item!.Dependencies?.Count() > 0 && !ImpossibleDependency(item!))
                throw new BO.ValidationException("You cant insert a circle dependencies");

            taskUpdate = new DO.Task
            (item.Id, false, item.Engineer?.Id, item.Alias, (DO.EngineerExperience?)item.Complexity, item.createAtDate, item.StartDate, item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.RequiredEffortTime, item.Description, item.Deliverables, item.Remarks);
            if (item.Dependencies != null)
            {
                var depToDelete = _dal.Dependency.ReadAll(dep => dep?.DependentTask == item.Id);
                depToDelete.ToList().ForEach(dep =>
                {
                    _dal.Dependency.Delete(dep!.Id);
                });
                //the dependencies that related to this task
                var listDep = item.Dependencies;
                if (listDep != null)
                    foreach (var taskDep in listDep)
                    {
                        var newDep = new DO.Dependency { DependentTask = item.Id, DependentOnTask = taskDep.Id };
                        _dal.Dependency.Create(newDep);
                    }
            }
        }
        //after creating a schedule
        else
        {
            //When you want to change the start date of a task
            if (item.StartDate != null)
            {
                //If the start date is less than the end date of the tasks that depend on them
                if (!_dal.Dependency.ReadAll(dep => dep.DependentTask == item.Id).All(dep => _dal.Task.Read((int)dep!.DependentOnTask!)?.DeadlineDate >= item.StartDate))
                    throw new BO.ValidationException("The start date entered does not match the rest of the schedule");
            }
            //If the start date is less than the start date of the entire project
            if (!_dal.Dependency.ReadAll().Any(dep => dep?.DependentTask == item.Id) && item.StartDate < _dal.ReturnStartProject())
            {
                throw new BO.ValidationException("The start date is less than the start date of the entire project");
            }
            //If the start date is greater than the end date
            if (item.StartDate > item.DeadlineDate)
                throw new BO.ValidationException("The start date is greater than the end date");
            //When you want to update an engineer who already has another engineer for the task 
            if (_dal.Task.Read(item.Id)?.EngineerId != null && _dal.Task.Read(item.Id)?.EngineerId != item.Engineer?.Id)
                throw new BO.ValidationException("There is already an engineer for the task");
            //calculate the deadline date
            var deadLineDate = item.StartDate + item.RequiredEffortTime;
            //calculate the requiredEffortTime
            var requiredEffortTime = item.DeadlineDate - item.StartDate;
            taskUpdate = new DO.Task
           (item.Id, false, item.Engineer?.Id, item.Alias, (DO.EngineerExperience?)item.Complexity, item.createAtDate, item.StartDate, item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.RequiredEffortTime, item.Description, item.Deliverables, item.Remarks);
        }

        try
        {
            _dal.Task.Update(taskUpdate);
        }
        catch (DO.DalDoesAlreadyExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={item.Id} already exists", ex);
        }
    }

    /// <summary>
    /// function that returns all tasks that have no startDate
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.Task?> GetTasksWithNoStartDate()
    {
        return GetTasksByFilter(task => task?.StartDate == null);
    }

    /// <summary>
    /// function that returns all tasks that have no engineer
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.TaskInEngineer?> GetAvailableTask()
    {

        return GetTasksByFilter(task => task?.Engineer == null).Select(tsk => new TaskInEngineer { Id = tsk!.Id, Alias = tsk.Alias }).ToList(); ;
    }


    /// <summary>
    /// function that return tasks for EngineerWindowOptions
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public IEnumerable<TaskInEngineer> GetAvailableTasksForEngineer(int Id)
    {
        BO.EngineerExperience? engEx = s_bl.Engineer.Read(Id)?.Level!;
        return GetTasksByFilter(tsk => tsk!.Engineer == null && tsk.Complexity! <= engEx).Select(tsk => new TaskInEngineer { Id = tsk!.Id, Alias = tsk.Alias }).ToList();
    }

    /// <summary>
    /// general filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Task?> GetTasksByFilter(Func<BO.Task?, bool> filter)
    {
        return ReadAll().Where(filter);
    }


    /// <summary>
    /// calculate the task status
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Status Calculate(int id)
    {
        DO.Task? item = _dal.Task.Read(id);

        if (item?.CompleteDate != null)
            return (Status)3;
        else if (item?.StartDate != null)
            return (Status)2;
        else if (item?.ScheduledDate != null)
            return (Status)1;
        else
            return (Status)0;
    }


    /// <summary>
    /// function to check for circular dependency
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private bool ImpossibleDependency(BO.Task task)
    {
        Queue<BO.TaskInList> taskQueue = new Queue<BO.TaskInList>(task.Dependencies!);
        while (taskQueue.Count() != 0)
        {
            var currentTask = taskQueue.Dequeue();
            if (currentTask.Id == task.Id)
                return false;
            Read(currentTask.Id)!.Dependencies?.ForEach(tsk => taskQueue.Enqueue(tsk));
        }
        return true;

    }


    /// <summary>
    /// function to return all tasks in TaskForList version
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.TaskForList?> GetAllTasksForList()
    {
        return ReadAll().Select(tsk => new TaskForList { Id = tsk!.Id, Description = tsk.Description, Alias = tsk.Alias, Status = tsk.Status, Dependencies = tsk.Dependencies, ComplexityLevel = tsk.Complexity }).ToList();
    }

    /// <summary>
    /// function to return all tasks with required status in TaskInList version
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public IEnumerable<BO.TaskForList?> GetTasksAppropriateStatus(BO.Status status)
    {
        return GetTasksByFilter(tsk => tsk?.Status == status).Select(tsk => new TaskForList { Id = tsk!.Id, Description = tsk.Description, Alias = tsk.Alias, Status = tsk.Status, Dependencies = tsk.Dependencies, ComplexityLevel = tsk.Complexity });
    }


    /// <summary>
    /// function for gant show
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public List<BO.TaskScheduleDays?> GetAllScheduleTasks()
    {
        // בדיקה אם קיים תאריך התחלת פרויקט
        var startProject = _dal.ReturnStartProject();
        if (startProject == null)
            throw new ValidationException("There is no schedule yet");

        // רשימה לאחסון תוצאות
        List<BO.TaskScheduleDays?> result = new List<BO.TaskScheduleDays?>();

        // לולאה על כל המשימות ויצירת פריטי TaskScheduleDays
        foreach (var task in ReadAll())
        {
            // חישוב ימים מתאריך התחלת הפרויקט עד תאריך התחלת המשימה
            int daysFromProjectStart = (int)((task!.StartDate!.Value - _dal.ReturnStartProject()!.Value).TotalDays);
            int daysFromProjectEnd = (int)((_dal.ReturnEndProject()!.Value - task!.DeadlineDate!.Value).TotalDays);
            var req = (int)task.RequiredEffortTime!.Value.TotalDays;
            string d = "";
            foreach (var dep in _dal.Dependency.ReadAll())
            {
                if( dep?.DependentTask == task.Id )
                {
                    d += + dep.DependentOnTask + ", ";
                }
            }
               
            // יצירת פריט TaskScheduleDays
            BO.TaskScheduleDays taskSchedule = new BO.TaskScheduleDays
            {
                TaskId = task.Id,
                DependencyId = d,
                TaskName = task.Alias,
                DaysFromProjectStart = daysFromProjectStart,
                TaskDays = req,
                DaysToProjectEnd = daysFromProjectEnd
            };


            result.Add(taskSchedule);
        }

        return result;
    }

    /// <summary>
    /// function for add task dependency
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TaskInList?> GetAllDependenciesOptions()
    {

        IEnumerable<BO.TaskInList> depList = _dal.Task.ReadAll().Select(tsk =>
        {
            //creating a new task in list for the ienumerable item 
            return new BO.TaskInList
            {
                Id = tsk!.Id,
                Alias = tsk?.Alias ?? "",
                Description = tsk?.Description ?? "",
                Status = (Status?)Calculate(tsk!.Id)
            };
        }).ToList();
        return depList;
    }

}
