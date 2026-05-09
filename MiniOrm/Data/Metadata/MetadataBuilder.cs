using MiniOrm.Attributes;
using System.Reflection;

namespace MiniOrm.Data.Metadata;

/// <summary>
/// This class is responsible for building metadata information for a given context type. 
/// It analyzes the properties of the context type to identify entities (represented by DbSet<T>) 
/// and their associated properties. The resulting metadata includes details 
/// such as entity names, property names, data types, primary key status, and nullability. 
/// This metadata can be used for various purposes, such as mapping, serialization, or schema inspection at runtime.
/// </summary>
public class MetadataBuilder
{
    /// <summary>
    /// Builds metadata information for a given context type. 
    /// It identifies entities and their properties based on the context's properties 
    /// that are of type DbSet<T>.
    /// </summary>
    /// <param name="contextType"></param>
    /// <returns></returns>
    public ModelMetadata Build(Type contextType)
    {
        var model = new ModelMetadata();
        var properties = contextType.GetProperties();

        foreach ( var property in properties)
        {
            if (!property.PropertyType.IsGenericType)
                continue;

            var genericType = property.PropertyType.GetGenericTypeDefinition();

            if (genericType != typeof(DbSet<>))
                continue;

            var entityType = property.PropertyType.GetGenericArguments()[0];
            model.Entities?.Add(BuildEntityMetadata(entityType));
        }

        return model;
    }

    /// <summary>
    /// Builds metadata for a specific entity type. It extracts the table name, property names, data types,
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    private EntityMetadata BuildEntityMetadata(Type entityType)
    {
        var entity = new EntityMetadata
        {
            Name = GetTableName(entityType),
        };

        var properties = entityType.GetProperties();

        foreach ( var property in properties)
        {
            var databaseType = new TypeMapper().Map(property.PropertyType);
            var Property = new PropertyMetadata
            {
                Name = GetColumnName(property),
                ClrType = property.PropertyType.GetType(),
                DatabaseType = databaseType.DatabaseType,
                IsPrimaryKey = CheckPrimaryKey(property),
                Nullable = databaseType.Nullable
            };
            entity.Properties?.Add(Property);
        }

        return entity;
    }

    /// <summary>
    /// This method retrieves the table name for a given entity type. It first checks if the entity type has a TableAttribute
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    private string GetTableName(Type entityType)
    {
        var attr = entityType.GetCustomAttribute<TableAttribute>();

        if (attr != null)
            if (attr.Name != null)
                return attr.Name;

        return entityType.Name + "s";
    }

    /// <summary>
    /// This method retrieves the column name for a given property. It first checks if the property has a ColumnAttribute
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    private string GetColumnName(PropertyInfo propertyInfo)
    {
        var nameAttr = propertyInfo.GetCustomAttribute<ColumnAttribute>();

        if (nameAttr != null)
            if(nameAttr.Name != null)
                return nameAttr.Name;

        return propertyInfo.Name;
    }

    /// <summary>
    /// This method checks if a given property is a primary key. It first checks if the property has a PrimaryKeyAttribute,
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    private bool CheckPrimaryKey(PropertyInfo propertyInfo)
    {
        var pkAttr = propertyInfo.GetCustomAttribute<PrimaryKeyAttribute>();

        if (pkAttr != null)
            return true;

        if (propertyInfo.Name == "Id")
            return true;

        return false;
    }
}
