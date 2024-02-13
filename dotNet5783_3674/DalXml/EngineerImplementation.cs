using DalApi;
using DO;

namespace Dal;

public class EngineerImplementation : IEngineer
{
    const string s_engineer = @"engineers";
    const string s_task = @"tasks";

    /// <summary>
    /// creaete new engineer 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesAlreadyExistException"></exception>
    public int Create(Engineer item)
    {
        List<Engineer> eng = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineer);
        if (eng.FirstOrDefault(en => en.Id == item.Id) != null)
        {
            throw new DalDoesAlreadyExistException($"Engineer with ID={item.Id} already exists");
        }
        eng.Add(item);
        XMLTools.SaveListToXMLSerializer(eng, s_engineer);
        return item.Id;
    }


    /// <summary>
    /// function for delete an item of Engineer object
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    /// <exception cref="DalCanNotDeletException"></exception>
    public void Delete(int id)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineer);
        List<DO.Task> listTaks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        //if the id of engineer not exists -no need for delete
        if (!(listEngineers.Any(eng => eng.Id == id)))
            throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        //if the id of engineer exists in the task-cannot be deleted
        if (listTaks.Any(tsk => tsk.EngineerId == id))
            throw new DalCanNotDeletException($"Engineer with ID={id} can not be deleted because, he still has task");
        foreach (var x in listEngineers)
        {
            if (id == x.Id)
            {
                listEngineers.Remove(x);
                break;
            }
        }
        XMLTools.SaveListToXMLSerializer(listTaks, s_task);
        XMLTools.SaveListToXMLSerializer(listEngineers, s_engineer);
    }


    /// <summary>
    /// function for get an item of Engineer by checking the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineer);
        return listEngineers.FirstOrDefault(eng => eng.Id == id);
    }


    /// <summary>
    /// function to read a single object by a condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineer);
        return listEngineers.FirstOrDefault(filter) /*?? throw new DalDoesNotExistException("No engineer found matching the specified condition.")*/;
    }


    /// <summary>
    /// function for reading all of the objects in the list or by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineer);
        if (filter == null)
            return listEngineers.Select(item => item);
        else if (listEngineers == null)
            throw new DalDoesNotExistException("this list is not exist");
        return listEngineers.Where(filter);
    }


    /// <summary>
    /// function for reset all the list of Engineer
    /// </summary>
    public void Reset()
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineer);
        listEngineers.Clear();
        XMLTools.SaveListToXMLSerializer(listEngineers, s_engineer);
    }


    /// <summary>
    /// function for reset all the list of Engineer
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Engineer item)
    {
        List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineer);
        //if id doesnt exist-no need for updating
        if (!(listEngineers.Any(eng => eng.Id == item.Id)))
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
        //foreach (var x in eng)
        //{
        //    if (item.Id == x.Id)
        //    {
        //        //first- remove the existing item
        //        eng.Remove(x);
        //        //second- adding the new item
        //        eng.Add(item);
        //    }
        //}
        listEngineers.RemoveAll(x => x.Id == item.Id);
        listEngineers.Add(item);
        XMLTools.SaveListToXMLSerializer(listEngineers, s_engineer);
    }
}
