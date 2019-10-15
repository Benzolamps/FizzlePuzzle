using System;
using System.Collections;
using System.Collections.Generic;

namespace FizzlePuzzle.Extension
{
    internal static class CollectionExtension
    {
        internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T obj in source)
            {
                action(obj);
            }
        }

        internal static string Join<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        internal static IEnumerable<T> CreateEnumerable<T>(this IEnumerator<T> source)
        {
            return new Enumerable<T>(source);
        }

        public static T[] Subarray<T>(this T[] source, int start, int end)
        {
            T[] objArray = new T[end - start + 1];
            for (int index = 0; index <= end - start; ++index)
                objArray[index] = source[index + start];
            return objArray;
        }

        private class Enumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> enumerator;

            internal Enumerable(IEnumerator<T> enumerator)
            {
                this.enumerator = enumerator;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerator;
            }
        }
    }
}
