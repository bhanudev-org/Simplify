using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Simplify.SeedWork
{
    public static class Guard
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotEmpty(Guid argumentValue, [CallerArgumentExpression("argumentValue")]
            string? argumentName = default)
        {
            if(argumentValue == Guid.Empty) throw new ArgumentException("Value cannot be empty.", argumentName);
        }

        [DebuggerStepThrough]
        public static void NotNull(object? argumentValue, [CallerArgumentExpression("argumentValue")]
            string? argumentName = default)
        {
            if(argumentValue == null) throw new ArgumentNullException(argumentName);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotDefault<T>(T argumentValue, [CallerArgumentExpression("argumentValue")]
            string? argumentName = default)
        {
            if(Equals(argumentValue, default(T)!)) throw new ArgumentException("Value cannot be an the default value.", argumentName);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNullOrEmpty(string? argumentValue, [CallerArgumentExpression("argumentValue")]
            string? argumentName = default)
        {
            NotNull(argumentValue, argumentName);

            if(string.IsNullOrWhiteSpace(argumentValue)) throw new ArgumentException("String parameter cannot be null or empty and cannot contain only blanks.", argumentName);
        }
    }
}