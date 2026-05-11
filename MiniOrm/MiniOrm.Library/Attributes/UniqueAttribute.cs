namespace MiniOrm.MiniOrm.Library.Attributes;

/// <summary>
/// Specifies that a property must have a unique value within its context.
/// </summary>
/// <remarks>Apply this attribute to a property to indicate that its value should be unique, typically for
/// validation or data modeling purposes. The enforcement of uniqueness depends on the consuming framework or validation
/// logic and is not handled by this attribute itself.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueAttribute : Attribute { }
