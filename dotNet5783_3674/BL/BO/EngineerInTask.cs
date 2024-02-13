using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// EngineerInTask logical entity
/// </summary>
public class EngineerInTask
{
    public int Id { get; init; }
    public string? Name { get; set; }


    public override string ToString() => Tools.ToStringProperty(this);
}

