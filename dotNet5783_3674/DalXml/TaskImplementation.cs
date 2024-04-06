using DalApi;
using DO;
using System.Linq;
using System.Threading.Tasks;

namespace Dal;

public class TaskImplementation : ITask
{
 
    const string s_task = @"tasks";
    const string s_dependency = @"dependencies";

    /// <summary>
    /// creaete new task with running id
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(DO.Task item)
    {
        List<DO.Task> listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        //create a running ID number
        int newId = Config.NextTaskId;
        DO.Task t = new DO.Task(newId, item.IsMileStone, item.EngineerId, item.Alias, item.Complexity, item.CreateDate, item.StartDate, item.ScheduledDate/*,item.ForecastDate*/, item.DeadlineDate, item.CompleteDate, item.RequiredEffortTime, item.Description, item.Deliverables, item.Remarks);
        listTasks.Add(t);
        XMLTools.SaveListToXMLSerializer(listTasks, s_task);
        return newId;

    }

    /// <summary>
    /// function for delete an item of task object
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    /// <exception cref="DalCanNotDeletException"></exception>
    public void Delete(int id)
    {
        List<Dependency> listDependency = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependency);
        List<DO.Task> listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);

        //In case that no task is found with such an ID, throw a note
        if (!(listTasks.Any(ts => ts.Id == id)))
            throw new DalDoesNotExistException($"Task with ID={id} is not exists");

        //in case that there is a function that depend on this task
        if (listDependency.Any(ts => ts.DependentOnTask == id))
            throw new DalCanNotDeletException($" There is a task that depends on this task with ID={id}, so you can't delete it");

        foreach (var x in listTasks)
        {
            if (id == x.Id)
            {
                //delete the requested task
                listTasks.Remove(x);
                break;
            }
        }

        XMLTools.SaveListToXMLSerializer(listDependency, s_dependency);
        XMLTools.SaveListToXMLSerializer(listTasks, s_task);
    }


    /// <summary>
    /// function for get an item of Engineer by checking the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DO.Task? Read(int id)
    {
        List<DO.Task> listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        return listTasks.FirstOrDefault(tsk => tsk.Id == id);
    }


    /// <summary>
    /// function to read a single object by a condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        return listTasks.FirstOrDefault(filter) /*?? throw new DalDoesNotExistException("No task found matching the specified condition.")*/;
    }


    /// <summary>
    /// function for reading all of the objects in the list or by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        if (filter == null)
            return listTasks.Select(item => item);
        else if (listTasks == null)
            throw new DalDoesNotExistException("this list is not exist");
        return listTasks.Where(filter);
    }


    /// <summary>
    /// function for reset all the list of Task
    /// </summary>
    public void Reset()
    {
        List<DO.Task> listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        listTasks.Clear();
        XMLTools.SaveListToXMLSerializer(listTasks, s_task); ;
    }


    /// <summary>
    /// function for update details of Task
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(DO.Task item)
    {
        List<DO.Task> listTasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        //if id doesnt exist-no need for updating
        if (!(listTasks.Any(eng => eng.Id == item.Id)))
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        //foreach (var x in listTasks)
        //{
        //    if (item.Id == x.Id)
        //    {
        //        //first- remove the existing item
        //        listTasks.Remove(x);
        //        //second- adding the new item
        //        listTasks.Add(item);
        //    }
        //}
        listTasks.RemoveAll(x => x.Id == item.Id);
        listTasks.Add(item);
        XMLTools.SaveListToXMLSerializer(listTasks, s_task); ;
    }
}
