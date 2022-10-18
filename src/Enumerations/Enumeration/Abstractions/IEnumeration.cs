using System;
namespace JustinWritesCode.Enumerations.Abstractions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

public interface IEnumeration
{
    int Id { get; }
    string Name { get; }
    FieldInfo FieldInfo { get; }
    string DisplayName { get; }
    string ShortName { get; }
    string Description { get; }
    string GroupName { get; }
    int Order { get; }
    DisplayAttribute DisplayAttribute { get; }
    IComparable ComparisonPropertyValue { get; }

    int CompareTo(object other);
    bool Equals(object obj);
    int GetHashCode();
    // IDictionary<string, object> Properties<TAttribute>() where TAttribute : Attribute;
    string ToString();
}

public interface IEnumeration<TValue> where TValue : Enum
{
    TValue Value { get; }
}
