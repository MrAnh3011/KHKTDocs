using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.IoC
{
    public static class ApplicationCoreExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddTransient

            return services;
        }
    }
}
