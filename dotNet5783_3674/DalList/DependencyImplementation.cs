namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    //creat dependency function
    public int Create(Dependency item)
    {
        //create a running ID number
        int newId = DataSource.Config.NextDependencyId;
        //Copying the dependency data
        Dependency d = new Dependency(newId,item.DependentTask,item.DependentOnTask);
        DataSource.Dependencies.Add(d);
        return newId;
    }

    //delete dependency function
    public void Delete(int id)
    {
        //In case that no dependency is found with such an ID, throw a note
        if (!(DataSource.Dependencies.Exists(dep => dep.Id == id)))
            throw new Exception($"Dependency with ID={id} is not exists");

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

    //read dependency function
    public Dependency? Read(int id)
    {
     
        //Returning the requested dependency
        return DataSource.Dependencies.Find(dep => dep.Id == id);
    }

    //readAll dependency function
    public List<Dependency> ReadAll()
    {
        //return all dependencies list
        return new List<Dependency>(DataSource.Dependencies);
    }

    //update dependency function
    public void Update(Dependency item)
    {
        //In the case that no dependency is found with such an ID, throw a note
        if (!(DataSource.Dependencies.Exists(dep => dep.Id == item.Id)))
            throw new Exception($"Dependency with ID={item.Id} is not exists");

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

    //reset dependency function
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
