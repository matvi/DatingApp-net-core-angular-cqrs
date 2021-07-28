using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using API.Common.Filters;
using API.Configuration;
using API.Data;
using API.Dtos;
using API.Helpers;
using API.Services;
using Common.Data.Repository;
using Common.Interfaces;
using Common.PipelinesBehaviours;
using Common.Queries;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration config)
        {    
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options => {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IUploadPhotoService, UploadPhotoService>();
            services.AddMemoryCache();
            //TODO: use ApiExceptionFilter instead of ExceptionMiddleware it will give you more control over the exceptions
            // step1: uncomment the next lines to add the ApiExceptionFilter
            // step2; remove app.useMiddleware(ExceptionMiddleware) from app configuration
            // step3: change ValidationException by CustomValidationException in ValidationBehaviour pipeline
            // step4: prepare Angular app to work with new type of response when errors
            // services.AddControllers(config =>
            // {
            //     config.Filters.Add(new ApiExceptionFilter());
            // });
        }
        
        public static void AddCQRSApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(typeof(Startup));
            // if handlers are in other project we need to add the assembly.
            // There are two options and both options will get the same object data.
            //option 1
            var assembly = AppDomain.CurrentDomain.Load("Common");
            services.AddMediatR(assembly);
            //option 2
            //services.AddMediatR(typeof(GetAllMembersQuery).GetTypeInfo().Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehaviour<,>));
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
        }

        public static void AddMassTransitApplicationService(this IServiceCollection services, IConfiguration config)
        {
            var rabbitMqSettings = config.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
            services.AddMassTransit(massConf =>
            {
                massConf.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host);
                });
            });
            services.AddMassTransitHostedService();
        }
    }
}