using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLICommander.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace CLICommander
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   
            // configure our DbContext class for uses within rest of app. Through dependency injection (config for connection string), we add dbcontext to the service container
            services.AddDbContext<CLICommanderContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("CLICommanderConnection")));

            // adding the controller for commands, also adding the configurations on the JSON serialization settings for controller so that all property names in the serialized JSON follow camel case naming conventions
            services.AddControllers().AddNewtonsoftJson(s => {
            s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();});

            /* for mock repo dependency injection
            services.AddScoped<ICLICommanderRepo, MockCLICommanderRepo>();
            */

            // registering service container with dependency injection: whenever ICLICommanderRepo is asked, give SqlCLICommanderRepo, to change the implementation, just swap the second parameter
            services.AddScoped<ICLICommanderRepo, SqlCLICommanderRepo>();

            // add automapper through dependency injection to the rest of our application for data transfer objects (DTO)
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // enable Swagger UI
            services.AddSwaggerGen(c => {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Ryu's CLI Commander API",
                    Description = "A simple ASP.NET Core Web API for storing commands",
                    Contact = new OpenApiContact
                    {
                        Name = "Bipbos",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/Xunzer")
                    }
                });
            });

            // configure Swagger to use Newtonsoft.Json for JSON serialization and deserialization
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CLI Commander API V1");
                c.RoutePrefix = string.Empty;               
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
