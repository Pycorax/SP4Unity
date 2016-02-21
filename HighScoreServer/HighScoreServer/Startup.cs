using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace HighScoreServer
{
    public class Startup
    {
        private IConfiguration config;

        // This function sets up the configuration environment
        public Startup(IApplicationEnvironment env)
        {
            config = new ConfigurationBuilder()
                .SetBasePath(env.ApplicationBasePath)
                .AddEnvironmentVariables()
                .AddJsonFile("config.json")
                .AddJsonFile("config.dev.json", true)
                .AddUserSecrets()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Initialize MVC
            services.AddMvc();

            // Initialize ScoreDataContext for use with the Controllers via Dependency Injection
            services.AddScoped<HighScoreServer.Models.ScoreDataContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // Obtain User Secret
            // var password = config.Get<string>("password");

            // Use different settings depending on debug or production builds
            if (config.Get<bool>("debug"))
            {
                // Register the Microsoft default Error Handler
                app.UseDeveloperExceptionPage();

                // Register the ASP.NET Runtime Info Page
                app.UseRuntimeInfoPage();
            }
            else
            {
                // Use a custom user-friendly error page if not
                app.UseExceptionHandler("/Home/Error");
            }

            // Register MVC Middleware AND specify the routing format
            app.UseMvc(routes => routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}"));

            // Register the File Server Middleware
            app.UseFileServer();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
