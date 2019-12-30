// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
	public static class Config
	{
		public static IEnumerable<IdentityResource> Ids =>
			new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				new IdentityResources.Email(),
				new IdentityResources.Phone(),
				new IdentityResources.Address(),
				new IdentityResource()
				{
					Name = "gaming",
						DisplayName = "Game Information",
						Description = "Displays your gaming information.",
						UserClaims = {
							"favourite_game"
						}
				}
			};
		public static IEnumerable<ApiResource> Apis =>
			new ApiResource[]
			{
				new ApiResource("random.api", "The Random API")
			};

		public static IEnumerable<Client> Clients =>
			new Client[]
			{
				new Client
				{
					ClientId = "random.console",
						AllowedGrantTypes = {
							GrantType.ClientCredentials,
							GrantType.ResourceOwnerPassword
						},
						ClientSecrets = {
							new Secret("secret".Sha256())
						},
						AllowedScopes = {
							"random.api",
							"gaming",
							IdentityServerConstants.StandardScopes.OpenId,
							IdentityServerConstants.StandardScopes.Email,
							IdentityServerConstants.StandardScopes.Profile,
						},
						AlwaysIncludeUserClaimsInIdToken = true,
				},
				// interactive ASP.NET Core MVC client
				new Client
				{
					ClientId = "random.mvc",
						ClientSecrets = { new Secret("another secret".Sha256()) },

						AllowedGrantTypes = GrantTypes.Code,
						RequireConsent = false,
						RequirePkce = true,

						// where to redirect to after login
						RedirectUris = { "http://localhost:5002/signin-oidc" },

						// where to redirect to after logout
						PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

						AllowedScopes = new List<string>
						{
							IdentityServerConstants.StandardScopes.OpenId,
							IdentityServerConstants.StandardScopes.Profile,
							IdentityServerConstants.StandardScopes.Email,
							"random.api"
						},
						AllowOfflineAccess = true
				}
			};

	}
}