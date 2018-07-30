using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.CQRS.Queries;
using IQSoft.PersonList.CQRS.Queries.GetPersonList;
using IQSoft.PersonList.Server.Dal;
using IQSoft.PersonList.Server.Modules;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;

namespace IQSoft.PersonList.Server
{
    public sealed class Startup
    {
        public IConfigurationRoot Configuration { get; }
        
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            
            Configuration = builder.Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PersonListContext>(c => c.UseSqlServer(Configuration["Data:connectionString"]));
            
            services.AddMediatR(typeof(GetPersonListQueryHandler), typeof(PersonListModule));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var connectionString = Configuration["Data:connectionString"];
            app.UseOwin(buildFunc => buildFunc.UseNancy(options =>
            {
                options.Bootstrapper = new CustomNancyBootstrapper(connectionString);
            }));
        }
    }
}