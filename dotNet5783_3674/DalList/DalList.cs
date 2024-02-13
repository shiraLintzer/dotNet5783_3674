namespace Dal;
using DalApi;


sealed internal class DalList : IDal
{
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public ITask Task => new TaskImplementation();

    public static IDal Instance { get; } = new DalList();
    private DalList() { }


    //reset all data
    public void Reset()
    {
        DataSource.Dependencies.Clear();
        DataSource.Engineers.Clear();
        DataSource.Tasks.Clear();
    }
    //function to update project end date
    public void updateEndProject(DateTime? value)
    {
        DataSource.Config.endProject = value;
    }
    //function to update project start date
    public void updateStartProject(DateTime? value)
    {
        DataSource.Config.startProject = value;
    }
    //function to return project end date
    public DateTime? ReturnEndProject()
    {
        return DataSource.Config.endProject;
    }
    
    //function to return project start date
    public DateTime? ReturnStartProject()
    {
        return DataSource.Config.startProject;
    }

}


