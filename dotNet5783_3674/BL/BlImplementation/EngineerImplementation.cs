using BlApi;
using BO;


namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// create new Engineer function
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="BO.validation"></exception>
    /// <exception cref="BO.BlDoesAlreadyExistException"></exception>
    public int Create(BO.Engineer item)
    {
        if (!IsValidIsraeliID(item.Id))
            throw new BO.ValidationException("the ID is not correct");
        if ((item.Cost != null && item.Cost < 0) || (item.Email != null && !item.Email.Contains('@')))
            throw new BO.ValidationException("one of the inputs that you put is not valid");
        DO.Engineer doEngineer = new DO.Engineer
        (item.Id, (DO.EngineerExperience)item.Level, item.Name, item.Email, item.Cost);
        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);
            return idEngineer;
        }
        //in case that engineer whith this id ia already exist
        catch (DO.DalDoesAlreadyExistException ex)
        {
            throw new BO.BlDoesAlreadyExistException($"Engineer with ID={item.Id} already exists", ex);
        }
    }

    /// <summary>
    /// delete Engineer function
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {

        try
        {
            _dal.Engineer.Delete(id);
        }
        //In case there is no engineer with such an ID 
        catch (DO.DalDoesNotExistException exception)
        {
            throw new BO.BlDoesNotExistException($"Engineer ID = {id} does not exist", exception);
        }
    }

    /// <summary>
    /// Read item by ID of Engineer function
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        //Search for the engineer's task
        var taskInEng = _dal.Task.Read(task => task.EngineerId == id);

        return new BO.Engineer()
        {
            Id = doEngineer.Id,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Cost = doEngineer.Cost,
            Task = taskInEng != null
                ? new TaskInEngineer { Id = taskInEng.Id, Alias = taskInEng.Alias }
                : null,

        };

    }

    /// <summary>
    /// readAll items of Engineer function
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.Engineer?> ReadAll()
    {
        var doEngList = _dal.Engineer.ReadAll();
        List<BO.Engineer?> boEngList = new List<BO.Engineer?>();
        foreach (var engineer in doEngList)
        {
            boEngList.Add(Read(engineer!.Id));
        }
        return boEngList;

    }

    /// <summary>
    /// update Engineer function
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(Engineer item)
    {
        //validation
        DO.Engineer? prevItem = _dal.Engineer.Read(item.Id);
        if (prevItem != null)
        {
            if (((int)(prevItem.Level) > (int)(item.Level)) || (item.Cost != null && item.Cost < 0) || (item.Email != null && !item.Email.Contains('@')))
                throw new BO.ValidationException("one of the inputs that you put is not valid");
        }

        //Checking that all tasks before this task have been Done
        var dependencies = _dal.Dependency.ReadAll(dep => dep.DependentTask == item.Task?.Id);
        bool isAllDone = dependencies.All(dep => Factory.Get().Task.Read((int)(dep?.DependentOnTask!))?.Status == Status.Done);
        //In case a schedule hasn't started yet and all tasks before have been done
        if (BlApi.Factory.Get().IsCreate && isAllDone)
        {

            //If the task is empty, just delete the engineer from the previous task
            var deleteTaskInEng = _dal.Task.Read(task => task.EngineerId == item.Id);
            if (deleteTaskInEng != null)
            {
                _dal.Task.Update(deleteTaskInEng with { EngineerId = null });
            }

            //If the task is not empty, update the assignment with the engineer's ID
            if (item.Task != null)
            {
                var newTask = _dal.Task.Read(task => task.Id == item.Task?.Id);
                //If there is a task and it is not assigned to another engineer,
                //and the engineer's level also matches the requirements
                if (newTask != null && newTask.EngineerId == null && (int?)newTask.Complexity <= (int?)item.Level)
                    _dal.Task.Update(newTask with { EngineerId = item.Id });
                else
                    Console.WriteLine("The engineer was not assigned the new task because there is a problem with one of the details, the rest of the data will be updated");
            }


        }
        else
        {
            Console.WriteLine("Schedule has not been started or there are tasks that are still a work in progress, task cannot be updated. The rest of the data will be updated");
        }

        DO.Engineer doEngineer = new DO.Engineer
        (item.Id, (DO.EngineerExperience)item.Level, item.Name, item.Email, item.Cost);
        try
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalDoesAlreadyExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={item.Id} already exists", ex);
        }
    }

    /// <summary>
    /// function that returns all engineers whose level matches the required level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer?> GetEngineersAppropriateLevel(BO.EngineerExperience level)
    {

        return GetEngineersByFilter(engineer => engineer?.Level >= level);
    }


    /// <summary>
    /// function that returns all engineers whose cost matches the required cost
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer?> GetEngineersAppropriateCost(int cost)
    {

        return GetEngineersByFilter(engineer => engineer?.Cost <= cost);
    }

    /// <summary>
    /// general filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer?> GetEngineersByFilter(Func<Engineer?, bool> filter)
    {
        return ReadAll().Where(filter);

    }



    static bool IsValidIsraeliID(int id)
    {
        // בדיקת אורך מספר תעודת הזהות
        if (id.ToString().Length != 9)
            return false;

        // יצירת מערך ספרות מהמספר תעודת הזהות
        int[] digits = new int[9];
        for (int i = 8; i >= 0; i--)
        {
            digits[i] = id % 10;
            id /= 10;
        }

        // חישוב סכום ומכפלה עבור בדיקת אמיתות התעודה
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            int digit = digits[i];
            if (i % 2 == 0) // אם מספר במיקום זוגי
            {
                sum += digit;
            }
            else // אם מספר במיקום אי זוגי
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
                sum += digit;
            }
        }

        // בדיקה שסכום הספרות חלוק ב-10
        return sum % 10 == 0;
    }

}
