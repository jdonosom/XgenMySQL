#nullable disable
using SauroGenerador.Plugin;

namespace Tools.Plugin;

internal class GnrUtil
{


    /// <summary>
    /// Convierte una cadena a UpperCamelCase
    /// </summary>
    /// <param name="value">Cadena de caracteres a convertir a camelcase</param>
    /// <returns>Retorna una cadena en formato CamelCase</returns>
    internal static string ToUpperCamelCase(
            string value
        )
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }
        return char.ToUpper(value[0]) + value.Substring(1);
    }


    /// <summary>
    /// Agrega una barra invertida al final de la ruta si no existe
    /// </summary>
    /// <param name="path">Ruta a procesar</param>
    /// <returns>Retorna una ruta con barra invertida al final de cada ruta</returns>
    internal static string AddSlash(
            string path
        )
    {
        if (!path.EndsWith("\\"))
        {
            path += "\\";
        }
        return path;
    }


    internal static string GetSingular(
            string plural
        )
    {
        string singular = string.Empty;

        if (plural.EndsWith("es"))
        {
            singular = plural.Substring(0, plural.Length - 2);
        }
        else if (plural.EndsWith("s"))
        {
            singular = plural.Substring(0, plural.Length - 1);
        }

        return (singular == "" ? plural : singular);
    }


    /// <summary>
    /// Guarda un archivo en la ruta especificada
    /// </summary>
    /// <param name="filePath">Ruta donde se almacenara</param>
    /// <param name="fileName">Nombre del archivo</param>
    /// <param name="content">contenido del archivo</param>
    internal static void SaveFile(
            string filePath,
            string fileName,
            string content
        )
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        File.WriteAllText($"{filePath}{fileName}", content);
    }

}
