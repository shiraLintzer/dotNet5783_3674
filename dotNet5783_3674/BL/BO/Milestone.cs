using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Milestone logical entity
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public string? Alias { get; set; } 
    public string? Description { get; set; } 
    public DateTime? CreatedAtDate { get; set; } 
    public BO.Status Status { get; set; }
    public DateTime? StartDate { get; set; } 
    public DateTime? ForecastDate { get; set; } 
    public DateTime? DeadlineDate { get; set; } 
    public DateTime? CompleteDate { get; set; } 
    public double? CompletionPercentage { get; set; }
    public string? Remarks { get; set; } 
    public List<TaskInList>? Dependencies { get; set; } = null;



    public override string ToString() => Tools.ToStringProperty(this);

}
