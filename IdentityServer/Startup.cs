﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
	public class Startup
	{
		public IWebHostEnvironment Environment { get; }

		public Startup(IWebHostEnvironment environment)
		{
			Environment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// uncomment, if you want to add an MVC-based UI
			services.AddControllersWithViews();

			var builder = services.AddIdentityServer()
				.AddInMemoryIdentityResources(Config.Ids)
				.AddInMemoryApiResources(Config.Apis)
				.AddInMemoryClients(Config.Clients)
				.AddTestUsers(TestUsers.Users);

			services.AddAuthentication()
				.AddGoogle("Google", options =>
				{
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
					options.ClientId = "85382615635-32iccpnnt663ecd2r2s338c4nohlfgul.apps.googleusercontent.com";
					options.ClientSecret = "RGa3lWvgFy_chGB6L6JM794L";
				});
			// not recommended for production - you need to store your key material somewhere secure
			builder.AddDeveloperSigningCredential();
		}

		public void Configure(IApplicationBuilder app)
		{
			if (Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// uncomment if you want to add MVC
			app.UseStaticFiles();
			app.UseRouting();

			app.UseIdentityServer();

			// uncomment, if you want to add MVC
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}