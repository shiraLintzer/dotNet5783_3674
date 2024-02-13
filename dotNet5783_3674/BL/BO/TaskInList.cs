using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// TaskInList logical entity
/// </summary>
public class TaskInList
{
    //public int Id { get; init; }
    //public required string Description { get; set; }
    //public required string Alias { get; set; }
    //public BO.Status Status { get; set; }


    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public BO.Status? Status { get; set; }

    public override string ToString() => Tools.ToStringProperty(this);
}
