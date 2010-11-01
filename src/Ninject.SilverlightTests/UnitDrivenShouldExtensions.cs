﻿//-------------------------------------------------------------------------------
// <copyright file="UnitDrivenShouldExtensions.cs" company="bbv Software Services AG">
//   Copyright (c) 2010 Software Services AG
//   Remo Gloor (remo.gloor@gmail.com)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <auto-generated>just to ignore stylecop</auto-generated>
//-------------------------------------------------------------------------------

#if !SILVERLIGHT_MSTEST
namespace UnitDriven.Should
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using UnitDriven;

    public static class UnitDrivenShouldExtensions
    {
        public static void ShouldContain(this string self, string str)
        {
            Assert.IsTrue(self.Contains(str));
        }

        public static void ShouldContain(this string self, string str, StringComparison comparison)
        {
            Assert.IsTrue(self.Contains(str));
        }

        public static void ShouldContain<T>(this IEnumerable<T> series, T item)
        {
            Assert.IsTrue(series.Contains(item));
        }

        public static void ShouldContain<T>(this IEnumerable<T> series, T item, IComparer<T> comparer)
        {
            Assert.IsTrue(series.Contains(item, new EqualityComparerUsingComparer<T>(comparer)));
        }

        public static void ShouldNotContain(this string self, string str)
        {
            Assert.IsFalse(self.Contains(str));
        }

        public static void ShouldNotContain(this string self, string str, StringComparison comparison)
        {
            Assert.IsFalse(self.Contains(str));
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> series, T item)
        {
            Assert.IsFalse(series.Contains(item));
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> series, T item, IComparer<T> comparer)
        {
            Assert.IsFalse(series.Contains(item, new EqualityComparerUsingComparer<T>(comparer)));
        }

        public static void ShouldBeEmpty(this IEnumerable series)
        {
            Assert.IsFalse(series.GetEnumerator().MoveNext());
        }

        public static void ShouldNotBeEmpty(this IEnumerable series)
        {
            Assert.IsTrue(series.GetEnumerator().MoveNext());
        }

        public static void ShouldBe<T>(this T self, T other)
        {
            Assert.AreEqual(other, self);
        }

        public static void ShouldBe<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) == 0);
        }

        public static void ShouldNotBe<T>(this T self, T other)
        {
            Assert.AreNotEqual(other, self);
        }

        public static void ShouldNotBe<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsFalse(comparer.Compare(self, other) == 0);
        }

        public static void ShouldBeNull(this object self)
        {
            Assert.IsNull(self);
        }

        public static void ShouldNotBeNull(this object self)
        {
            Assert.IsNotNull(self);
        }

        public static void ShouldBeSameAs(this object self, object other)
        {
            Assert.AreSame(other, self);
        }

        public static void ShouldNotBeSameAs(this object self, object other)
        {
            Assert.AreNotSame(other, self);
        }

        public static void ShouldBeTrue(this bool self)
        {
            Assert.IsTrue(self);
        }

        public static void ShouldBeTrue(this bool self, string message)
        {
            Assert.IsTrue(self, message);
        }

        public static void ShouldBeFalse(this bool self)
        {
            Assert.IsFalse(self);
        }

        public static void ShouldBeFalse(this bool self, string message)
        {
            Assert.IsFalse(self, message);
        }

        /*
        public static void ShouldBeInRange<T>(this T self, T low, T high)
        {
            Assert.InRange(self, low, high);
        }

        public static void ShouldNotBeInRange<T>(this T self, T low, T high)
        {
            Assert.NotInRange(self, low, high);
        }
        */

        public static void ShouldBeGreaterThan<T>(this T self, T other)
            where T : IComparable<T>
        {
            Assert.IsTrue(self.CompareTo(other) > 0);
        }

        public static void ShouldBeGreaterThan<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) > 0);
        }

        public static void ShouldBeGreaterThanOrEqualTo<T>(this T self, T other)
            where T : IComparable<T>
        {
            Assert.IsTrue(self.CompareTo(other) >= 0);
        }

        public static void ShouldBeGreaterThanOrEqualTo<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) >= 0);
        }

        public static void ShouldBeLessThan<T>(this T self, T other)
            where T : IComparable<T>
        {
            Assert.IsTrue(self.CompareTo(other) < 0);
        }

        public static void ShouldBeLessThan<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) < 0);
        }

        public static void ShouldBeLessThanOrEqualTo<T>(this T self, T other)
            where T : IComparable<T>
        {
            Assert.IsTrue(self.CompareTo(other) <= 0);
        }

        public static void ShouldBeLessThanOrEqualTo<T>(this T self, T other, IComparer<T> comparer)
        {
            Assert.IsTrue(comparer.Compare(self, other) <= 0);
        }

        public static void ShouldBeInstanceOf<T>(this object self)
        {
            Assert.IsInstanceOfType(typeof(T), self);
        }

        public static void ShouldBeInstanceOf(this object self, Type type)
        {
            Assert.IsInstanceOfType(type, self);
        }

        public static void ShouldNotBeInstanceOf<T>(this object self)
        {
            Assert.IsNotInstanceOfType(typeof(T), self);
        }

        public static void ShouldNotBeInstanceOf(this object self, Type type)
        {
            Assert.IsNotInstanceOfType(type, self);
        }

        /*
        public static void ShouldBeThrownBy<T>(this T self, Assert.ThrowsDelegate method)
            where T : Exception
        {
            Assert.Throws<T>(method);
        }
        */

        private class EqualityComparerUsingComparer<T> : IEqualityComparer<T>
        {
            private IComparer<T> comparer;

            public EqualityComparerUsingComparer(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public bool Equals(T x, T y)
            {
                return this.comparer.Compare(x, y) == 0;
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
#endif
