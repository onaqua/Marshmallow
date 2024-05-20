using System.Diagnostics.CodeAnalysis;

namespace Marshmallow.Extensions.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Perform a null check
    /// </summary>
    /// <param name="value"></param>
    /// <returns><c>true</c> if the <paramref name="value"/> is not null, otherwise <c>false</c></returns>
    public static bool IsNotNull([NotNullWhen(true)] this object? value) =>
        value is not null;

    /// <summary>
    /// Perform a null check
    /// </summary>
    /// <param name="value"></param>
    /// <returns><c>true</c> if the <paramref name="value"/> is null, otherwise <c>false</c></returns>
    public static bool IsNull([NotNullWhen(false)] this object? value) =>
        value is null;
}