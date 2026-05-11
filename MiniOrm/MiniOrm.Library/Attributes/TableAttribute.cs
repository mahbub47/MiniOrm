namespace MiniOrm.MiniOrm.Library.Attributes;

/// <summary>
/// Specifies the database table name that a class is mapped to when using an object-relational mapper.
/// </summary>
/// <remarks>Apply this attribute to a class to override the default table name used by the data access framework.
/// If the attribute is not specified or the name is null, the framework typically uses the class name as the table
/// name. This attribute is commonly used in ORMs such as Entity Framework to control table mapping.</remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TableAttribute : Attribute
{
    public string? Name { get; set; }
    public TableAttribute(string? Name = null)
    {
        this.Name = Name;
    }
}
