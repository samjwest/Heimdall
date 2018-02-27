using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Heimdall.Gateway.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmailAddress(this string value)
        {
            return Regex.IsMatch(value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
        }

        public static bool IsNumeric(this string s)
        {
            int result;
            return int.TryParse(s, out result);
        }

        public static bool IsGuid(this string s)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(s, out result);
            return result != Guid.Empty;
        }

        public static string Truncate(this string value, int maxLength)
        {
            return value != null && value.Length > maxLength - 1 ? value.Substring(0, Math.Min(value.Length, maxLength)) : value;
        }

        public static bool IsYesOrNo(this string s)
        {
            return (s.ToUpper() == "Y" || s.ToUpper() == "YES") ? true : false;
        }
    }
}
