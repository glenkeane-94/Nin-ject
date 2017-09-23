// -------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsForIEnumerable.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010, Enkari, Ltd.
//   Copyright (c) 2010-2017, Ninject Project Contributors
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Infrastructure.Language
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable"/>.
    /// </summary>
    internal static class ExtensionsForIEnumerable
    {
        private static readonly MethodInfo Cast = typeof(Enumerable).GetMethod(nameof(Cast));
        private static readonly MethodInfo ToArray = typeof(Enumerable).GetMethod(nameof(ToArray));
        private static readonly MethodInfo ToList = typeof(Enumerable).GetMethod(nameof(ToList));

        /// <summary>
        /// Casts the elements of an <see cref="IEnumerable"/> to the specified type using reflection.
        /// </summary>
        /// <param name="series">The <see cref="IEnumerable"/> that contains the elements to be cast.</param>
        /// <param name="elementType">The type to cast the elements of source to.</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> that contains each element of the
        /// source sequence cast to the specified type.
        /// </returns>
        public static IEnumerable CastSlow(this IEnumerable series, Type elementType)
        {
            var method = Cast.MakeGenericMethod(elementType);
            return method.Invoke(null, new[] { series }) as IEnumerable;
        }

        /// <summary>
        /// Creates an array from an <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="series">An <see cref="IEnumerable"/> to create an array from.</param>
        /// <param name="elementType">The type of the elements.</param>
        /// <returns>An array that contains the elements from the input sequence.</returns>
        public static Array ToArraySlow(this IEnumerable series, Type elementType)
        {
            var method = ToArray.MakeGenericMethod(elementType);
            return method.Invoke(null, new[] { series }) as Array;
        }

        /// <summary>
        /// Creates an <see cref="IList"/> from an <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="series">An <see cref="IEnumerable"/> to create an <see cref="IList"/> from.</param>
        /// <param name="elementType">The type of the elements.</param>
        /// <returns>An <see cref="IList"/> that contains the elements from the input sequence.</returns>
        public static IList ToListSlow(this IEnumerable series, Type elementType)
        {
            var method = ToList.MakeGenericMethod(elementType);
            return method.Invoke(null, new[] { series }) as IList;
        }
    }
}