using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Task logical entity
/// </summary>
public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; } = null;
    public string? Alias { get; set; }
    public DateTime? createAtDate { get; set; } = null;
    public BO.Status? Status { get; set; } = null;
    public List<TaskInList>? Dependencies { get; set; } = null;
    //TimeSpan? RequiredEffortTime { get; init; } 
    public MilestoneInTask? Milestone { get; set; } = null;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? ForecastDate { get; set; } = null;
    public DateTime? DeadlineDate { get; set; } = null;
    public DateTime? CompleteDate { get; set; } = null;
    public string? Deliverables { get; set; } = null;
    public string? Remarks { get; set; } = null;
    public EngineerInTask? Engineer { get; set; } = null;
    public EngineerExperience? Complexity { get; set; } = null;
    public TimeSpan? RequiredEffortTime { get; set; } = null;



    public override string ToString() => Tools.ToStringProperty(this);

}

