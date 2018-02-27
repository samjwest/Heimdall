using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Heimdall.Gateway.Core
{
    public interface IGatewayModule
    {
        void Initialize(IServiceCollection serviceCollection);
    }
}
