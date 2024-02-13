using DalApi;
using DO;
using System.Data.Common;
using System.Xml.Linq;

namespace Dal;

internal class DependencyImplementation : IDependency
{

    const string s_dependencies = "dependencies";

    /// <summary>
    /// create dependency for XElement that received
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    /// <exception cref="DalXmlFormatException"></exception>
    static Dependency? createDependencyFromXElement(XElement d)
    {
        return new Dependency
        {
            Id = d.ToIntNullable("Id") ?? throw new DalXmlFormatException("id"),
            DependentTask = (int?)d.ToIntNullable("DependenTask"),
            DependentOnTask = (int?)d.ToIntNullable("DependensOnTask")
        };
    }

    /// <summary>
    /// creaete new dependency with running id
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies);
        int elemId = Config.NextDependencyId;
        XElement dependElemnt = new XElement("Dependency",
          new XElement("Id", elemId),
          new XElement("DependenTask", item.DependentTask),
          new XElement("DependensOnTask", item.DependentOnTask)
          );
        rootDependency.Add(dependElemnt);
        XMLTools.SaveListToXMLElement(rootDependency, s_dependencies);
        return elemId;

    }

    /// <summary>
    /// function for delete an item of Dependency object
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies);
        XElement? dep = (from dp in rootDependency.Elements()
                         where (int?)dp.Element("Id") == id
                         select dp).FirstOrDefault() ?? throw new DalDoesNotExistException("missing Id");
        dep.Remove();
        XMLTools.SaveListToXMLElement(rootDependency, s_dependencies);
    }

    /// <summary>
    /// function for get an item of dependency by checking the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies);
        return (from dp in rootDependency?.Elements()
                where dp.ToIntNullable("Id") == id
                select (Dependency?)createDependencyFromXElement(dp)).FirstOrDefault();
    }

    /// <summary>
    /// function for get an item of dependency by checking filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies);
        return (from dp in rootDependency?.Elements()
                let dependency = createDependencyFromXElement(dp)
                where dependency != null && filter(dependency)
                select (Dependency?)dependency).FirstOrDefault() /*?? throw new DalDoesNotExistException("matching dependency not found")*/;
    }

    /// <summary>
    /// function for reading all of the objects in the list or by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies);
        if (filter != null)
        {
            return from dp in rootDependency.Elements()
                   let doDep = createDependencyFromXElement(dp)
                   where filter(doDep)
                   select (Dependency?)doDep;
        }
        else
        {
            return from dp in rootDependency.Elements()
                   select createDependencyFromXElement(dp);
        }
    }

    /// <summary>
    /// function for reset all the list of Dependency
    /// </summary>
    public void Reset()
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies);

        // Clear all elements
        rootDependency.Elements().Remove();

        // Save the modified XML
        XMLTools.SaveListToXMLElement(rootDependency, s_dependencies);
    }

    /// <summary>
    /// function for update details of dependency
    /// </summary>
    /// <param name="item"></param>
    public void Update(Dependency item)
    {
        //calling the other crud functions
        Delete(item.Id);
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies);
        int elemId = item.Id;
        XElement dependElemnt = new XElement("Dependency",
          new XElement("Id", elemId),
          new XElement("DependenTask", item.DependentTask),
          new XElement("DependensOnTask", item.DependentOnTask)
          );
        rootDependency.Add(dependElemnt);
        XMLTools.SaveListToXMLElement(rootDependency, s_dependencies);
    }
}