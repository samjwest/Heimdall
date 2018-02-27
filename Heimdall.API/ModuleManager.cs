using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Runtime.Loader;
using Heimdall.Gateway.API.Models;
using Microsoft.Extensions.DependencyInjection;
using Heimdall.Gateway.Core;

namespace Heimdall.Gateway.API
{
    public static class ModuleManager
    {

        public static IList<Assembly> FindModules(string path)
        {
            IList<Assembly> modules = new List<Assembly>();
            if (string.IsNullOrEmpty(path))
                return modules;

            var dirInfo = new DirectoryInfo(path);
            foreach( var file in dirInfo.GetFileSystemInfos("*.dll"))
            {
                Assembly assembly;
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    modules.Add(assembly);

                }
                catch(FileLoadException ex)
                {
                    if (ex.Message == "Assembly with same name is already loaded")
                    {
                        continue;
                    }
                    throw;
                }

                
            }

            return modules;
        }

        public static IServiceCollection LoadModules(this IServiceCollection services, IList<Assembly> assemblies)
        {
            var mvcBuilder = services.AddMvc();
            var moduleInterface = typeof(IGatewayModule);

            foreach (var assembly in assemblies)
            {
                // Register controllers
                mvcBuilder.AddApplicationPart(assembly);

                // Initialize module dependencies
                var moduleType = assembly.GetTypes().Where(x => typeof(IGatewayModule).IsAssignableFrom(x)).FirstOrDefault();
                if (moduleType != null && moduleType != typeof(IGatewayModule))
                {
                    var module = (IGatewayModule)Activator.CreateInstance(moduleType);
                    module.Initialize(services);
                }
            }

            return services;
        }
    }
}
