namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    //create engineer function
    public int Create(Engineer item)
    {
        foreach (var x in DataSource.Engineers)
        {
            //In case that such an id already exists
            if (item.Id == x.Id)
                throw new Exception($"Engineer with ID={item.Id} already exists");
        }
        //adding the engineer
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    //delete engineer function
    public void Delete(int id)
    {
        //In case that no engineer is found with such an ID, throw a note
        if (!(DataSource.Engineers.Exists(eng => eng.Id == id)))
            throw new Exception($"Engineer with ID={id} is not exists");

        //in case that this engineer stiil has tasks to do
        if(DataSource.Tasks.Exists(eng => eng.EngineerId == id))
            throw new Exception($"Engineer with ID={id} can't be deleted, there are tasks that need to be done by him");

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

    //read engineer function
    public Engineer? Read(int id)
    {
       
        //return the requested engineer
        return DataSource.Engineers.Find(eng => eng.Id == id);
    }

    //readAll engineer function
    public List<Engineer> ReadAll()
    {
        //return all engineer list
        return new List<Engineer>(DataSource.Engineers);
    }

    //update engineer function
    public void Update(Engineer item)
    {
        //In case that no engineer is found with such an ID, throw a note
        if (!(DataSource.Engineers.Exists(eng => eng.Id == item.Id)))
            throw new Exception($"Engineer with ID={item.Id} is not exists");

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

    //reset engineer function
    public void Reset()
    {
        //delete all engineers

        //Creating an array so that we don't reach an exception in the list
        Engineer[] temp = new Engineer[DataSource.Engineers.Count];
        temp = DataSource.Engineers.ToArray();

        //deletion of each member
        for (int i = 0;i < temp.Length;i++)
        {
            Delete(temp[i].Id);
        }
    }
}
