using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service.Iservice;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Text;
namespace Service
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDataBaseService(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExcelService, ExcelService>();
              //services.AddScoped<IHOCRService, HOCRService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFormRepository, FormRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddRepositories();
            return services;
        }
    }
}
