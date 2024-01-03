using DalApi;
using System.Xml.Linq;

namespace Dal;

public class DalXml : IDal
{
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public void Reset()
    {
        //open file foreach entity, remove all, close the file
        XElement rootDependency = XMLTools.LoadListFromXMLElement("dependencies");
        rootDependency.Elements().Remove();
        XMLTools.SaveListToXMLElement(rootDependency, "dependencies");
        XElement rootTask = XMLTools.LoadListFromXMLElement("tasks");
        rootTask.Elements().Remove();
        XMLTools.SaveListToXMLElement(rootTask, "tasks");
        XElement rootEngineer = XMLTools.LoadListFromXMLElement("engineers");
        rootEngineer.Elements().Remove();
        XMLTools.SaveListToXMLElement(rootEngineer, "engineers");
    }

    public DateTime? ReturnEndProject()
    {
        return Config.endProject;
    }

    public DateTime? ReturnStartProject()
    {
        return Config.startProject;
    }

    public void updateEndProject(DateTime? value)
    {
        Config.endProject = value;
    }

    public void updateStartProject(DateTime? value)
    {
        Config.startProject = value;
    }
}
