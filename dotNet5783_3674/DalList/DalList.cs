namespace Dal;
using DalApi;


sealed public class DalList : IDal
{
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public ITask Task => new TaskImplementation();

}


