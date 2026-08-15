using System.Diagnostics.CodeAnalysis;

namespace RecipeBook.Domain.Guards;

/// <summary>
/// Guard clause helpers that throw when a condition does not pass.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Throws the exception produced by <paramref name="exceptionFactory"/> when <paramref name="condition"/> is false.
    /// </summary>
    public static void Against<TException>(bool condition, Func<TException> exceptionFactory)
        where TException : Exception
    {
        if (!condition)
        {
            throw exceptionFactory();
        }
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> with <paramref name="message"/> when <paramref name="condition"/> is false.
    /// </summary>
    public static void Against(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> when <paramref name="value"/> is null; otherwise returns it.
    /// </summary>
    public static T AgainstNotNull<T>([NotNull] T? value, string parameterName) where T : class
    {
        return value ?? throw new ArgumentNullException(parameterName);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when <paramref name="value"/> is null or empty; otherwise returns it.
    /// </summary>
    public static string AgainstNotNullOrEmpty([NotNull] string? value, string parameterName)
    {
        return string.IsNullOrEmpty(value) ? throw new ArgumentException("Value must not be null or empty.", parameterName) : value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when <paramref name="value"/> is null, empty, or whitespace; otherwise returns it.
    /// </summary>
    public static string AgainstNotNullOrWhiteSpace([NotNull] string? value, string parameterName)
    {
        return string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Value must not be null or whitespace.", parameterName) : value;
    }
}
