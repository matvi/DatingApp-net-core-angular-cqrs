using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    }
}