
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiniOrm.Helper;

public class TypeConverter : JsonConverter<Type>
{
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeName = reader.GetString();
        if(string.IsNullOrEmpty(typeName)) return null;

        var type = Type.GetType(typeName);
        if(type != null) return type;

        return AppDomain.CurrentDomain.GetAssemblies()
        .Select(a => a.GetType(typeName))
        .FirstOrDefault(t => t != null);
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.AssemblyQualifiedName);
    }
}
