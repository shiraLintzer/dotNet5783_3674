namespace DO;

/// <summary>
/// Dependency entity
/// </summary>
/// <param name="Id"></param>
/// <param name="DependentTask" What task does it depend on></param>
/// <param name="DependentOnTask" What a task depends on it></param>
public record Dependency
(
    int Id,
    int? DependentTask = null,
    int? DependentOnTask = null
)

{
    //Default constructor
    public Dependency() : this(0) { }

    //ToString Dependency function
    public override string ToString()
    {
        return $"id: {Id}, DependentTask: {DependentTask}, DependentOnTask:{DependentOnTask}";

    }
}
