using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFavThings.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;
using System.Diagnostics;

namespace MyFavThings.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.ConfigureIdentityProvider(Configuration.GetConnectionString("MyFavThingsDb"));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials());
            });

            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        };
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "MyFavThings API",
                    Description = "A Simple API to create our favorite things with ASP.NET Core"                    
                });

                var xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "MyFavThings.Web.Api.xml");
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllParametersInCamelCase();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(filterHandler =>
                {
                    filterHandler.Run(async context =>
                    {

                        var handler = context.Features.Get<IExceptionHandlerFeature>();
                        context.Response.ContentType = "application/json";

                        if (handler != null)
                        {
                            var error = new
                            {
                                Message = handler.Error.Message,
                                Status = (int)HttpStatusCode.InternalServerError
                            };

                            await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(error));
                        }
                    });
                });
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyFavThings V1");
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
