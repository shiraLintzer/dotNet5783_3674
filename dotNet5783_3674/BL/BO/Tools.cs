
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BO;
internal static class Tools
{
    /// <summary>
    /// Prints for each attribute its values including attributes that are IEnumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string ToStringProperty<T>(T t)
    {
        if (t == null)
        {
            return string.Empty;
        }

        Type type = t.GetType();
        PropertyInfo[] properties = type.GetProperties();

        string result = $"{type.Name} properties:\n";

        foreach (var property in properties)
        {
            object? value = property.GetValue(t);

            if (value is IEnumerable enumerableValue && !(value is string))
            {
                result += $"{property.Name}: [{string.Join(", ", enumerableValue.Cast<object>())}]\n";
            }
            else
            {
                result += $"{property.Name}: {GetValueAsString(value)}\n";
            }
        }

        return result;
    }

    /// <summary>
    /// return a value to the object value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string GetValueAsString(object? value)
    {
        return value?.ToString() ?? "null";
    }


}