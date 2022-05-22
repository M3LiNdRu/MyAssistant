using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyAssistant.Apis.Expenses.Api.Swagger;
using MyAssistant.Apis.Expenses.Api.Resources.Categories;
using MyAssistant.Apis.Expenses.Api.Resources.Expenses;

namespace MyAssistant.Apis.Expenses.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnv;

        private IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddMvc()
                .Services
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
                })
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
