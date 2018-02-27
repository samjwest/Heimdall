using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Heimdall.Gateway.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (list == null)
                return;

            foreach (T t in list)
            {
                action(t);
            }
        }

        // for generic interface IEnumerable<T>
        public static string ToString<T>(this IEnumerable<T> source, string separator)
        {
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            if (string.IsNullOrEmpty(separator))
                throw new ArgumentException("Parameter separator can not be null or empty.");

            string[] array = source.Where(n => n != null).Select(n => n.ToString()).ToArray();

            return string.Join(separator, array);
        }

        // for interface IEnumerable
        public static string ToString(this IEnumerable source, string separator)
        {
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            if (string.IsNullOrEmpty(separator))
                throw new ArgumentException("Parameter separator can not be null or empty.");

            string[] array = source.Cast<object>().Where(n => n != null).Select(n => n.ToString()).ToArray();

            return string.Join(separator, array);
        }

        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentException("Parameter list can not be null.");

            BindingList<T> result = new BindingList<T>();
            foreach (T item in list)
            {
                result.Add(item);
            }
            return result;
        }

        //public static bool IsNullOrEmpty<T>(this IEnumerable<T> obj)
        //{
        //    return (obj == null || !obj.Any());
        //}
    }
}
