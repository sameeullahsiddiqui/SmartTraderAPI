using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartTrader.Core.Helpers;
using SmartTrader.Core.Inerfaces;
using SmartTrader.Infrastructure.EFStructures;
using SmartTrader.Infrastructure.Repositories;

namespace SmartTrader.API
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
            services.AddDbContext<SmartTraderContext>(options =>options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("SmartTrader")));

            // Add cors
            services.AddCors();

            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartTrader_API", Version = "v1" });
                //c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "SmartTrader", "SmartTrader_API" }
                            }
                        }
                    }
                });
            });

            services.AddAutoMapper(typeof(Startup));

            // Configurations
            //services.Configure<AppSettings>(Configuration);

            // DB Creation and Seeding
            //services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();


            services.AddControllers();

            services.AddScoped<ISmartTraderContext, SmartTraderContext>();
            services.AddScoped<IIndustryViewRepository, IndustryViewRepository>();
            services.AddScoped<ISectorAnalysisRepository, SectorAnalysisRepository>();
            services.AddScoped<ISectorStockViewRepository, SectorStockViewRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IStockPriceRepository, StockPriceRepository>();
            services.AddScoped<ISymbolRepository, SymbolRepository>();
            services.AddScoped<IWatchListRepository, WatchListRepository>();
            services.AddScoped<IEarningReportRepository, EarningReportRepository>();
            services.AddScoped<ISuperstarPortfolioRepository, SuperstarPortfolioRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IEarningReportRepository, EarningReportRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Utilities.ConfigureLogger(loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger UI - StockPortal";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"SmartTrader API V1");
                c.OAuthClientId("swaggerui");
                c.OAuthClientSecret("no_password"); //Leaving it blank doesn't work
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
