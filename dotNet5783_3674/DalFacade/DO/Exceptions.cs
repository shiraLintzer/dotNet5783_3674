
namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

[Serializable]
public class DalDoesAlreadyExistException : Exception
{
    public DalDoesAlreadyExistException(string? message) : base(message) { }
}

[Serializable]
public class DalCanNotDeletException : Exception
{
    public DalCanNotDeletException(string? message) : base(message) { }
}

[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}

[Serializable]
public class DalXmlFormatException : Exception
{
    public DalXmlFormatException(string? message) : base(message) { }
}
