using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// TaskInEngineer logical entity
/// </summary>
public class TaskInEngineer
{
    public int Id { get; init; }
    public string? Alias { get; set; }


    public override string ToString() => Tools.ToStringProperty(this);

}
