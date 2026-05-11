namespace MiniOrm.MiniOrm.Library.Attributes;

/// <summary>
/// Indicates that a property is the primary key of the entity.
/// </summary>
/// <remarks>Apply this attribute to a property to designate it as the primary key when mapping an object to a
/// data store. Only one property in a class should be marked with this attribute. This attribute is typically used in
/// object-relational mapping (ORM) scenarios to identify the unique identifier for an entity.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PrimaryKeyAttribute : Attribute { }
