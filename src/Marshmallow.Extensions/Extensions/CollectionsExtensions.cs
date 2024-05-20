namespace Marshmallow.Extensions.Extensions;

public static class CollectionsExtensions
{
    /// <summary>
    /// Perform a null check and check the <paramref name="collection"/> for the existence of elements in it 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns><c>true</c> if <paramref name="collection"/> is null or empty, otherwise <c>false</c></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) =>
        collection.IsNull() || 
        collection.IsEmpty();

    /// <summary>
    /// Perform check the <paramref name="collection"/> on empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(this IEnumerable<T> collection) =>
        ObjectExtensions.IsNull(collection.FirstOrDefault());

    /// <summary>
    /// Perform a check for the existence of items in the <paramref name="collection"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns><c>true</c> if <paramref name="collection"/> is not empty, otherwise <c>false</c></returns>
    public static bool IsNotEmpty<T>(this IEnumerable<T> collection) =>
        collection.Any();

    /// <summary>
    /// Perform a null check and check the <paramref name="collection"/> for the existence of elements in it 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">Коллекция с значениями</param>
    /// <returns><c>true</c> if <paramref name="collection"/> is not null and not empty, otherwise <c>false</c></returns>
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection) =>
        collection.IsNotNull() && 
        collection.IsNotEmpty();

    /// <summary>
    /// Perform a null check
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns><c>true</c> if the <paramref name="collection"/> value is null, otherwise <c>false</c></returns>
    public static bool IsNull<T>(this IEnumerable<T>? collection) =>
        ObjectExtensions.IsNull(collection);

    /// <summary>
    /// Perform a null check
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns><c>true</c> if the <paramref name="collection"/> value is not null, otherwise <c>false</c></returns>
    public static bool IsNotNull<T>(this IEnumerable<T>? collection) =>
        ObjectExtensions.IsNotNull(collection);
}