using System.Reflection;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using ConversationApp.Framework;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using ConversationApp.Framework.Interfaces;
using ConversationApp.Framework.Services;
using ConversationApp.Data;

namespace ConversationApp
{
    public class Startup
    {
        //private ILog logger;

        public Startup(IWebHostEnvironment env)
        {
            var environmentLevel = env.EnvironmentName.ToLower();

            //XmlDocument log4netConfig = new XmlDocument();
            //log4netConfig.Load(File.OpenRead($"log4net.{environmentLevel}.config"));
            //var repo = log4net.LogManager.CreateRepository(
            //    Assembly.GetEntryAssembly(),
            //    typeof(log4net.Repository.Hierarchy.Hierarchy));
            //log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            //logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile($"appsettings.{environmentLevel}.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //AppSettings settings = new AppSettings(this.Configuration);
            services.AddDbContext<CaDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("CaDb")));


            services.Configure<AppSettings>(Configuration.GetSection("Values"));

            services.AddHttpClient();
            services.AddScoped<CaDbContext>();

            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IUserService, UserService>();

            //services.AddScoped<ISettings, Settings>();
            //services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

            services.AddControllersWithViews();
            services.AddSession(options => {
                // Set a short timeout for easy testing
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();

             services.AddSingleton<SessionManager>();

            services.AddAuthentication(options =>
        {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = Configuration.GetSection("Authentication:Google:ClientId").Value;
    options.ClientSecret = Configuration.GetSection("Authentication:Google:ClientSecret").Value;
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");

});

    }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
           

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}");
            });
        }
    }
}
