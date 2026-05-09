using MiniOrm.Attributes;
using System.Reflection;

namespace MiniOrm.Data.Metadata;

public class MetadataBuilder
{
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

    private string GetTableName(Type entityType)
    {
        var attr = entityType.GetCustomAttribute<TableAttribute>();

        if (attr != null)
            if (attr.Name != null)
                return attr.Name;

        return entityType.Name + "s";
    }

    private string GetColumnName(PropertyInfo propertyInfo)
    {
        var nameAttr = propertyInfo.GetCustomAttribute<ColumnAttribute>();

        if (nameAttr != null)
            if(nameAttr.Name != null)
                return nameAttr.Name;

        return propertyInfo.Name;
    }

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
