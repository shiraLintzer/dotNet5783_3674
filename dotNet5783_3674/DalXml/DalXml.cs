using DalApi;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dal;

sealed internal class DalXml : IDal
{
    static string s_data_config_xml = "data_config_xml";

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }


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

        XMLTools.SetDate(s_data_config_xml, "startProject", null);
        XMLTools.SetDate(s_data_config_xml, "endProject", null);
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
        XMLTools.SetDate(s_data_config_xml, "startProject", value);
    }

    public void updateStartProject(DateTime? value)
    {
        Config.startProject = value;
        XMLTools.SetDate(s_data_config_xml, "endProject", value);
    }
}
