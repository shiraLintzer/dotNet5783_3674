using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// MilestoneInTask logical entity
/// </summary>
/// 
public class MilestoneInTask
{
    public int Id { get; init; }
    public required string Alias { get; set; }
}
