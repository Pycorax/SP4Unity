using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Entity;
using Microsoft.Extensions.PlatformAbstractions;

namespace HighScoreServer
{
    public class Startup
    {
        private IConfiguration config;

        // This function sets up the configuration environment
        public Startup(IApplicationEnvironment env, IHostingEnvironment hEnv)
        {
            config = new ConfigurationBuilder()
                .SetBasePath(env.ApplicationBasePath)
                .AddEnvironmentVariables()
                .AddJsonFile("config.json")
                .AddJsonFile("config.dev.json", true)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{hEnv.EnvironmentName}.json", optional: true)
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
            services.AddSingleton<HighScoreServer.Models.ScoreDataContext>();

            // Initialize Entity Framework
            if (config.Get<bool>("InMemoryDB"))
            {
                services.AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<Models.ScoreDataContext>(dbConfig => dbConfig.UseInMemoryDatabase());
            }
            else
            {
                string scoreDBConnectionStr = config["Data:HighScore:ConnectionString"];
                services.AddEntityFramework()
                    .AddSqlServer()
                    .AddDbContext<Models.ScoreDataContext>(dbConfig => dbConfig.UseSqlServer(scoreDBConnectionStr));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Use different settings depending on debug or production builds
            if (env.IsDevelopment())
            {
                // Register the Microsoft default Error Handler
                app.UseDeveloperExceptionPage();

                // Register the ASP.NET Runtime Info Page
                app.UseRuntimeInfoPage();
            }
            else
            {
                //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                //        .CreateScope())
                //{
                //    serviceScope.ServiceProvider.GetService<HighScoreServer.Models.ScoreDataContext>()
                //                      .Database.Migrate();
                //}
                // Use a custom user-friendly error page if not
                app.UseExceptionHandler("/Home/Error");
            }

            // Register MVC Middleware AND specify the routing format
            app.UseMvc(routes => routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}"));

            // Register the File Server Middleware
            app.UseFileServer();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
