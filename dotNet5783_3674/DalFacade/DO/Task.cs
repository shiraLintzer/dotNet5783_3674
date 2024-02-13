namespace DO;


/// <summary>
/// Task entity
/// </summary>
/// <param name="Id"></param>
/// <param name="Description"></param>
/// <param name="Alias"></param>
/// <param name="IsMileStone"></param>
/// <param name="EngineerId"></param>
/// <param name="Complexity"></param>
/// <param name="CreateDate"></param>
/// <param name="StartDate"></param>
/// <param name="ScheduledDate"></param>
/// <param name="DeadlineDate"></param>
/// <param name="CompleteDate"></param>
/// <param name="RequiredEffortTime"></param>
/// <param name="Deliverables"></param>
/// <param name="Remarks"></param>
public record Task
(
    int Id,
    bool IsMileStone,
    int? EngineerId = null,
    string? Alias = null,
    EngineerExperience? Complexity = null,
    //DateTime? CreateDate = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    //DateTime? ForecastDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    TimeSpan? RequiredEffortTime = null,
    string? Description = null,
    string? Deliverables = null,
    string? Remarks = null

)

{
    //Default constructor
    public Task() : this(0, false) { }

    //Task creation date
    public DateTime CreateDate => DateTime.Now;

    //ToString Task function
    public override string ToString()
    {
        return $"id: {Id}, IsMileStone: {IsMileStone}, EngineerId:{EngineerId}, Complexity: {Complexity}, Alias: {Alias} ,CreateDate: {CreateDate}, StartDate:{StartDate}, ScheduledDate: {ScheduledDate}, DeadlineDate:{DeadlineDate}, CompleteDate: {CompleteDate}, RequiredEffortTime:{RequiredEffortTime},Description: {Description}, Deliverables:{Deliverables}, Remarks: {Remarks}";

    }
}
