using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiniOrm.MiniOrm.Library.Helper;

/// <summary>
/// This class is a custom JSON converter for the System.Type type. 
/// It allows for the serialization and deserialization of Type objects to and from 
/// their assembly-qualified names in JSON format. When serializing, it writes the assembly-qualified name of the Type. 
/// When deserializing, it attempts to resolve the Type from the provided string, 
/// first by using Type.GetType and then by searching through all loaded assemblies if necessary.
/// </summary>
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
