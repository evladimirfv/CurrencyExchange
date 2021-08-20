using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExchange.API.Data;
using CurrencyExchange.API.Data.Store;
using CurrencyExchange.API.Helpers;
using CurrencyExchange.API.Services;
using CurrencyExchange.API.Services.Helpers;
using CurrencyExchange.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CurrencyExchange", Version = "v1" });
            });
         services.AddCors(options =>
            {
                options.AddPolicy("api-currencyexchange-policy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            services.AddScoped<ICurrencyPurchaseService, CurrencyPurchaseService>();
            services.AddScoped<IHttpCallService, HttpCallService>();
            services.AddScoped<IPurchaseStore, PurchaseStore>();
            services.AddScoped<IUserStore, UserStore>();
            services.Configure<ApiSettings>(Configuration);

            // Database settings
            var connectionString = Configuration.GetConnectionString("ExchangeContext");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CurrencyExchange v1"));
            }

            app.UseRouting();

            app.UseCors("api-currencyexchange-policy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
