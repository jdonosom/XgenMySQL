using Xgen.Plugin;

namespace SauroGenerador.Plugin;

public static class TypeSQL
{
    public static string ToSQLType(this SQLType type)
    {
        return type switch
        {
            SQLType.MySQL => "MySQL Server",
            _ => "Desconocido"
        };
    }
}
public enum SQLType
{
    MySQL,
}
