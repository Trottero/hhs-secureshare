﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services;
using SecureShare.WebApi.Wrapper.Services.Interfaces;
using SecureShare.Website.Extensions;

namespace SecureShare.Website
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
			services.AddAuthentication(sharedOptions =>
				{
					sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
					sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
				})
				.AddAzureAd(options => Configuration.Bind("AzureAd", options))
				.AddCookie();

			services.AddMvc();

			var apiUrls = Configuration.GetSection("ApiUrls");
			services.Configure<ApiUrls>(apiUrls);
			services.AddTransient<IHttpService, HttpService>();
			services.AddTransient<IUserService, UserService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseStatusCodePages(async context =>
			{
				context.HttpContext.Response.ContentType = "text/plain";
				await context.HttpContext.Response.WriteAsync(
					"Status code page, status code: " +
					context.HttpContext.Response.StatusCode);
			});
		}
	}
}