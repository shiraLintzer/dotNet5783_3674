using BO;

namespace BlApi;

public interface ITask
{
    public int Create(BO.Task item);
    public BO.Task? Read(int id);
    public IEnumerable<BO.Task?> ReadAll();
    public void Update(BO.Task item);
    public void Delete(int id);

    //public void Reset();

    public IEnumerable<BO.Task?> GetTasksWithNoStartDate();
    public IEnumerable<BO.TaskInEngineer?> GetAvailableTask();
    public IEnumerable<BO.TaskForList?> GetAllTasksForList();
    public IEnumerable<BO.TaskForList?> GetTasksAppropriateStatus(BO.Status status);
    public List<BO.TaskScheduleDays?> GetAllScheduleTasks();
    public IEnumerable<TaskInList?> GetAllDependenciesOptions();
    public IEnumerable<TaskInEngineer> GetAvailableTasksForEngineer(int Id);



}
