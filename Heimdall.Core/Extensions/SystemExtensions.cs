using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Heimdall.Gateway.Core.Extensions
{
    public static class SystemExtensions
    {
        public static bool In( this string s, params string[] args )
        {
            return args.Any( x => x.Equals( s ) );
        }

        public static bool In<T>(this T t, params T[] values)
        {
            return values.Contains( t );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveNonNumeric( this string s )
        {
            if( !string.IsNullOrEmpty( s ) )
            {
                char[] result = new char[s.Length];
                int resultIndex = 0;
                foreach( char c in s )
                {
                    if( char.IsNumber( c ) )
                        result[resultIndex++] = c;
                }
                if( 0 == resultIndex )
                    s = string.Empty;
                else if( result.Length != resultIndex )
                    s = new string( result, 0, resultIndex );
            }
            return s;
        }


        /// <summary>
        /// Converts a string to a guid, or empty guid if empty or invalid
        /// </summary>
        public static Guid ToGuid(this string input)
        {
            Guid result;
            Guid.TryParse(input, out result);
            return result;
        }


    }

}
