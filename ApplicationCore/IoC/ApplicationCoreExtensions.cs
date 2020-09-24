using ApplicationCore.Interfaces.Services;
using ApplicationCore.Services;
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
            services.AddScoped<IDoctypeService, DoctypeServices>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
