namespace MiniOrm.MiniOrm.Library.Attributes;

/// <summary>
/// Specifies the mapping between a class property and a database column for use with object-relational mapping
/// frameworks.
/// </summary>
/// <remarks>Apply this attribute to a property to customize the column name or database type used when persisting
/// the property to a database. This attribute is typically used in data access scenarios to override default mapping
/// conventions.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnAttribute : Attribute
{
    public string? Name { get; set; }
    public string? DataBaseType { get; set; }

    public ColumnAttribute(string? Name = null, string? DataBaseType = null)
    {
        this.Name = Name;
        this.DataBaseType = DataBaseType;
    }
}
