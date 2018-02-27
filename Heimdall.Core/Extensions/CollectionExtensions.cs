using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Heimdall.Gateway.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty( this ICollection obj )
        {
            return (obj == null || obj.Count == 0);
        }
    }
}
