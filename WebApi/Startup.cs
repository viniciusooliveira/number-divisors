using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Business;
using Domain.Contracts.Business;
using Domain.Contracts.Infrastructure;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace WebApi
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
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Divisors WebApi", 
                    Version = "v1"
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            
            #region DI

            services.AddSingleton<IRedisRepository, RedisBaseRepository>(x => 
                new RedisBaseRepository(Configuration.GetConnectionString("Redis")));
            services.AddSingleton<INumberBusiness, NumberBusiness>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Divisors WebApi v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(option => option.AllowAnyOrigin());
            
            app.UseRouting();

            app.UseResponseCaching();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}