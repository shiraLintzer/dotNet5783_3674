namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public ITask Task { get; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsCreate { get; set; } 

    //reset all data;
    void Reset();

    //Create a schedule
    public void CreateProject();


}
