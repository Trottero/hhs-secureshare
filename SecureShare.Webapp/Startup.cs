using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureShare.Webapp.Data;
using SecureShare.Webapp.Services;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services;
using SecureShare.WebApi.Wrapper.Services.Interfaces;
using SecureShare.Website.Controllers;
using SecureShare.Website.Models;

namespace SecureShare.Webapp
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddMicrosoftAccount(options =>
            {
                options.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                options.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];

            });

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();

            var apiUrls = Configuration.GetSection("ApiUrls");
            services.Configure<ApiUrls>(apiUrls);

            services.AddTransient<IHttpService, HttpService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserFileService, UserFileService>();
	        services.AddTransient<IShareFileService, ShareFileService>();
            services.AddTransient<FileReader>();
            var faceApiCoding = Configuration.GetSection("FaceApiCoding");
            services.Configure<FaceApiCoding>(faceApiCoding);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            //migrates the database if there are any migrations in the migrations folder. DO NOT USE EnsureCreated() This will bypass all migrations!
            context.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
