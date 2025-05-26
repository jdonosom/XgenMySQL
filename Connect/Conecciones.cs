#nullable disable
namespace XgenMySQL.Connect;

public static class Conecciones
{
    public static string Proyecto;
    public static string ServerHost;

    public static string NombreUsuario;
    public static string Contraseña;
    public static string Database;
    public static string Port;
    public static string Proveedor;
    public static string TipoConeccion { get; internal set; }
    public static string Seleccionadas;
    public static string Disponibles;

    // Models
    public static string NamespaceMd;
    public static bool modiMdPublic;
    public static bool modiMdPrivate;

    // Modificadores
    public static bool modiMdPartial;
    public static bool modiMdStatic;
    public static bool modiMdSealed;
    public static bool modiMdAbstract;

    // Business Logic
    public static string NamespaceBl;
    public static bool modiBlPublic;
    public static bool modiBlPrivate;

    // Modificadores
    public static bool modiBlPartial;
    public static bool modiBlStatic;
    public static bool modiBlSealed;
    public static bool modiBlAbstract;
    public static string rutaProyecto;

    public static Database Bd = null;


}
