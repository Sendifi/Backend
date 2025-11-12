using Humanizer;

namespace WebApplication2.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Convierte nombres tipo "FavoriteSources" o "favoriteSources"
    /// a "favorite-sources" usando Humanizer.
    /// </summary>
    public static string ToKebabCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        // Kebaberize maneja PascalCase, camelCase, espacios y underscores
        var s = input.Kebaberize();          // e.g., "FavoriteSources" -> "favorite-sources"
        return s.Trim('-');
    }

    /// <summary>
    /// Convierte a snake_case usando Humanizer.
    /// </summary>
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var s = input.Underscore();          // e.g., "FavoriteSources" -> "favorite_sources"
        return s.Trim('_');
    }
    

}