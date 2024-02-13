namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class ValidationException : Exception
{
    public ValidationException(string? message) : base(message) { }
} 


[Serializable]
public class BlDoesAlreadyExistException : Exception
{
    public BlDoesAlreadyExistException(string? message) : base(message) { }
    public BlDoesAlreadyExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlCanNotDeletException : Exception
{
    public BlCanNotDeletException(string? message) : base(message) { }
    public BlCanNotDeletException(string message, Exception innerException)
                : base(message, innerException) { }
}



