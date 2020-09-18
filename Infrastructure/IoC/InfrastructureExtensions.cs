using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.IoC
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddRepository (this IServiceCollection services)
        {
            return services;
        }
    }
}
