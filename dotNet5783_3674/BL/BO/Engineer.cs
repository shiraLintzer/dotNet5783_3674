using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Engineer logical entity
/// </summary>
public class Engineer
{
    public int Id { get; init; }
    public EngineerExperience Level { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public double? Cost { get; set; }
    public TaskInEngineer? Task { get; set; }


    public override string ToString() => Tools.ToStringProperty(this);
}
