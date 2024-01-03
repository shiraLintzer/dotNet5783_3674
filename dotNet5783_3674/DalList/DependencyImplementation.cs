namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class DependencyImplementation : IDependency
{

    /// <summary>
    /// creat dependency function
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        //create a running ID number
        int newId = DataSource.Config.NextDependencyId;
        //Copying the dependency data
        Dependency d = new Dependency(newId,item.DependentTask,item.DependentOnTask);
        DataSource.Dependencies.Add(d);
        return newId;
    }


    /// <summary>
    /// delete dependency function
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        //In case that no dependency is found with such an ID, throw a note
        if (!(DataSource.Dependencies.Any(dep => dep.Id == id)))
            throw new DalDoesNotExistException($"Dependency with ID={id} is not exists");

        foreach (var x in DataSource.Dependencies)
        {
            if (id == x.Id)
            {
                //delete the requested dependency
                DataSource.Dependencies.Remove(x);
                break;
            }

        }
    }



    /// <summary>
    /// read Dependency function
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        //return the requested dependency
        return DataSource.Dependencies.FirstOrDefault(dep => dep.Id == id);
    }

    //read Dependency by filter function
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    //readAll dependency function
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
    }


    /// <summary>
    /// update dependency function
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {
        //In the case that no dependency is found with such an ID, throw a note
        if (!(DataSource.Dependencies.Any(dep => dep.Id == item.Id)))
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} is not exists");

        foreach (var x in DataSource.Dependencies)
        {
            if (item.Id == x.Id)
            {
                //remove the old dependency
                DataSource.Dependencies.Remove(x);
                //add the new dependency
                DataSource.Dependencies.Add(item);
                break;
            }
        }
    }


    /// <summary>
    /// reset all data in dependency function
    /// </summary>
    public void Reset()
    {
        //delete all dependencies

        //Creating an array so that we don't reach an exception in the list
        Dependency[] temp = new Dependency[DataSource.Dependencies.Count];
        temp = DataSource.Dependencies.ToArray();

        //deletion of each member
        for (int i = 0; i < temp.Length; i++)
        {
            Delete(temp[i].Id);
        }
    }
}
