using CurrencyConverterCBR.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;


namespace CurrencyConverterCBR
{
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSwaggerGen();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Default", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File($@"{Directory.GetCurrentDirectory()}\Logs\{DateTime.Now:yyyy-MM-dd}.log", outputTemplate: "[{Timestamp:HH:mm:ss.fff}] |{Level:u3}| {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            services.AddSingleton(Log.Logger);

            services.AddSingleton<IConverterService, ConverterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency Converter"); });
            }
            else {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            //app.UseEndpoints(endpoints => {
            //    endpoints.MapControllers();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
