using System;
using System.Linq;

namespace SauroGenerador.Plugin;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input)
    {
        switch (input)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

    public static string Left(string param, int length)
    {
        string result = param.Substring(0, length);
        return result;
    }

    public static string Right(string param, int length)
    {
        int value = param.Length - length;
        string result = param.Substring(value, length);
        return result;
    }

    public static string Mid(string param, int startIndex, int length)
    {
        string result = param.Substring(startIndex, length);
        return result;
    }

    public static string Mid(string param, int startIndex)
    {
        string result = param.Substring(startIndex);
        return result;
    }

}