using API.Filters;
using API.Middlewares;
using DataSource;
using Identity.IOCConfig;
using Identity.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Application;

namespace API
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
            services.AddDbContext<ReadAndWriteDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("KnowledgeManagementDBConnection")));

            // Adding Services ;
            services.AddApplications();
            services.ConfigureIdentity(Configuration);

            services.AddApplicationServices();
            //  services.AddTransient<IdentityIOCConfiguration>();


            // services.AddScoped<PersianConvertorFilterAttribute>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();

                        //  builder.AllowCredentials();
                    });
            });

            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                
                // config.Filters.Add(new HttpResponseExceptionFilter());
            });

            //.AddXmlDataContractSerializerFormatters();

            //services.AddSwaggerGen(c =>
            //{

            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KnowledgeManagementAPI", Version = "v1" });
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KnowledgeManagementAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReadAndWriteDbContext context)
        {
           
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            context.Database.EnsureCreated();
            // app.ConfigureExceptionHandler();
            app.UseMiddleware<ExceptionMiddleware>();

            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KnowledgeManagementAPI v1"));


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
