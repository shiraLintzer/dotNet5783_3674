namespace DO;

/// <summary>
/// Engineer entity
/// </summary>
/// <param name="Id"></param>
/// <param name="Level"></param>
/// <param name="Name"></param>
/// <param name="Email"></param>
/// <param name="Cost" for hour></param>
public record Engineer
(
    int Id,
    EngineerExperience Level,
    string? Name = null,
    string? Email = null,
    double? Cost = null
)

{
    //Default constructor
    public Engineer() : this(0, 0) { }

    //ToString Engineer function
    public override string ToString()
    {
        return $"id: {Id}, Level: {Level}, Name:{Name}, Email: {Email}, Cost:{Cost}";
    }
}

