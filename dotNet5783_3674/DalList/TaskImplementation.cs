namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    //create task function
    public int Create(Task item)
    {
        //create a running ID number
        int newId = DataSource.Config.NextTaskId;
        //Copying the task data
        Task t = new Task(newId, item.IsMileStone,item.EngineerId,item.Alias, item.Complexity/*,item.CreateDate*/, item.StartDate,item.ScheduledDate/*,item.ForecastDate*/,item.DeadlineDate,item.CompleteDate,item.RequiredEffortTime,item.Description,item.Deliverables,item.Remarks);
        DataSource.Tasks.Add(t);
        return newId;
    }

    //delete task function
    public void Delete(int id)
    {
        //In case that no task is found with such an ID, throw a note
        if (!(DataSource.Tasks.Exists(ts => ts.Id == id)))
            throw new Exception($"Task with ID={id} is not exists");

        //in case that there is a function that depend on this task
        if(DataSource.Dependencies.Exists(ts => ts.DependentOnTask == id))
            throw new Exception($" There is a task that depends on this task with ID={id}, so you can't delete it");

        foreach (var x in DataSource.Tasks)
        {
            if (id == x.Id)
            {
                //delete the requested task
                DataSource.Tasks.Remove(x);
                break;
            }
                
        }
    }

    //read task function
    public Task? Read(int id)
    {
       
        //return the requested dependency
        return DataSource.Tasks.Find(ts => ts.Id == id);
    }

    //readAll task function
    public List<Task> ReadAll()
    {
        //return all task list
        return new List<Task>(DataSource.Tasks);
    }

    //update task function
    public void Update(Task item)
    {
        //In the case that no task is found with such an ID, throw a note
        if (!(DataSource.Tasks.Exists(ts => ts.Id == item.Id)))
            throw new Exception($"Task with ID={item.Id} is not exists");

        foreach (var x in DataSource.Tasks)
        {
            if (item.Id == x.Id)
            {
                //remove the old task
                DataSource.Tasks.Remove(x);
                //add the new task
                DataSource.Tasks.Add(item);
                break;
            }
        }
    }

    //reset task function
    public void Reset()
    {
        //delete all tasks

        //Creating an array so that we don't reach an exception in the list
        Task[] temp = new Task[DataSource.Tasks.Count];
        temp = DataSource.Tasks.ToArray();

        //deletion of each member
        for (int i = 0; i < temp.Length; i++)
        {
            Delete(temp[i].Id);
        }
    }
}
