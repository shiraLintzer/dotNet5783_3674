using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class TaskForList
{
    public int Id { get; init; }
    public string? Description { get; set; } = null;
    public string? Alias { get; set; } = null;
    public List<TaskInList>? Dependencies { get; init; } = null;
    public Status? Status { get; set; } = null;
    public EngineerExperience? ComplexityLevel { get; set; } = null;


    public override string ToString() => Tools.ToStringProperty(this);

}
