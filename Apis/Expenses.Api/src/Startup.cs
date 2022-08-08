using Api.Authentication;
using Library.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyAssistant.Apis.Expenses.Api.Resources.Categories;
using MyAssistant.Apis.Expenses.Api.Resources.Expenses;
using MyAssistant.Apis.Expenses.Api.Swagger;
using System.Collections.Generic;

namespace MyAssistant.Apis.Expenses.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        private IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _environment = env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddMvc()
                .Services
                .AddOptions()
                .AddGoogleAuthentication(
                    authority: Configuration.GetValue<string>("Authentication:Google:Authority"), 
                    audience: Configuration.GetValue<string>("Authentication:Google:ClientId")
                )
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("0.1.0", new OpenApiInfo
                    {
                        Version = "0.1.0",
                        Title = "Expenses API",
                        Description = "Expenses API (ASP.NET 6)",
                    });
                    c.CustomSchemaIds(type => type.FullName);
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                    c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });


                })
                .ConfigureMongoDb(Configuration)
                .RegisterCategoriesFeatures()
                .RegisterExpensesFeatures();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();

            //TODO: Uncomment this if you need wwwroot folder
            // app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("0.1.0/swagger.yaml", "Expenses Api v1"); 
            });

            //TODO: Use Https Redirection
            // app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
        }
    }
}
