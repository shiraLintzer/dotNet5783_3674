using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// MilestoneInList logical entity
/// </summary>

public class MilestoneInList
{
    public int Id { get; init; }
    public required string Description { get; set; }
    public required string Alias { get; set; }
    Status Status { get; init; }
    public required double CompletionPercentage { get; set; }
}
