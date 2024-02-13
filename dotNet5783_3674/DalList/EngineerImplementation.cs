namespace Dal;
using DalApi;
using DO;
using System.Linq;

//using System.Collections.Generic;
//using System.Linq;

internal class EngineerImplementation : IEngineer
{

    /// <summary>
    /// create engineer function
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesAlreadyExistException"></exception>
    public int Create(Engineer item)
    {
        //In case that such an id already exists
        if (DataSource.Engineers.Any(x => x.Id == item.Id))
            throw new DalDoesAlreadyExistException($"Engineer with ID={item.Id} already exists");
        //adding the engineer
        DataSource.Engineers.Add(item);
        return item.Id;

    }


    /// <summary>
    /// delete engineer function
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    /// <exception cref="DalCanNotDeletException"></exception>
    public void Delete(int id)
    {
        //In case that no engineer is found with such an ID, throw a note
        if (!(DataSource.Engineers.Any(eng => eng.Id == id)))
            throw new DalDoesNotExistException($"Engineer with ID={id} is not exists");

        //in case that this engineer stiil has tasks to do
        if(DataSource.Tasks.Any(eng => eng.EngineerId == id))
            throw new DalCanNotDeletException($"Engineer with ID={id} can't be deleted, there are tasks that need to be done by him");

        foreach (var x in DataSource.Engineers)
        {
            if (id == x.Id)
            {
                //delete the requested engineer
                DataSource.Engineers.Remove(x);
                break;
            }
        }
    }



    /// <summary>
    /// read Engineer function
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        //return the requested Engineer
        return DataSource.Engineers.FirstOrDefault(en => en.Id == id);
    }


    /// <summary>
    /// read Engineer by filter function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }


    /// <summary>
    /// readAll engineer function
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }


    /// <summary>
    /// update engineer function
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Engineer item)
    {
        //In case that no engineer is found with such an ID, throw a note
        if (!(DataSource.Engineers.Any (eng => eng.Id == item.Id)))
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} is not exists");

        foreach (var x in DataSource.Engineers)
        {
            if (item.Id == x.Id)
            {
                //remove the old engineer
                DataSource.Engineers.Remove(x);
                //add the new engineer
                DataSource.Engineers.Add(item);
                break;
            }
        }

    }


    /// <summary>
    /// reset all data in engineer function
    /// </summary>
    public void Reset()
    {
        //delete all engineers
        DataSource.Engineers.Clear();




        ////Creating an array so that we don't reach an exception in the list
        //Engineer[] temp = new Engineer[DataSource.Engineers.Count];
        //temp = DataSource.Engineers.ToArray();

        ////deletion of each member
        //for (int i = 0;i < temp.Length;i++)
        //{
        //    Delete(temp[i].Id);
        //}
    }
}
