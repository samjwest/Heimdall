﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Heimdall.Gateway.API.Models
{
    public class ModuleInfo
    {
        public string Name { get; set; }
        public Assembly Assembly { get; set; }
        public string Path { get; set; }
        public string SortName { get { return Name.Split('.').Last(); } }
        
    }
}
