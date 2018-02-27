using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heimdall.Gateway.Core.Extensions
{
    
    public static class ListExtensions
    {
        public static string ToString( this IList<String> list, string delimiter )
        {
            return string.Join( delimiter, list.ToArray() );
        }

        public static IList<T> EnsureNotNull<T>(this IList<T> list)
        {
            return list ?? new List<T>();
        }

    }
}
