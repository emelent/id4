using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace RandomClient
{
	class Program
	{
		static async Task<TokenResponse> RequestToken(DiscoveryDocumentResponse disco)
		{
			var client = new HttpClient();

			if (disco.IsError)
			{
				throw new Exception(disco.Error);
			}

			// request token
			// var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			// {
			// 	Address = disco.TokenEndpoint,

			// 		ClientId = "random.console",
			// 		ClientSecret = "secret",
			// 		Scope = "random.api"
			// });

			// request token
			var tokenResponse = await client.RequestPasswordTokenAsync(
				new PasswordTokenRequest()
				{
					Address = disco.TokenEndpoint,
						ClientId = "random.console",
						ClientSecret = "secret",
						Scope = "random.api openid custom.profile",
						UserName = "bob",
						Password = "bob"
				});
			if (tokenResponse.IsError)
			{
				throw new Exception(tokenResponse.Error);
			}

			Console.WriteLine(tokenResponse.Json);
			return tokenResponse;
		}

		static async Task MakeApiCalls()
		{
			// call api
			var client = new HttpClient();
			var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
			var tokenResponse = await RequestToken(disco);
			client.SetBearerToken(tokenResponse.AccessToken);

			var response = await client.GetAsync("http://localhost:5001/identity");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				// Console.WriteLine(JArray.Parse(content));
			}

			response = await client.GetAsync(disco.UserInfoEndpoint);
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				var parsedContent = JObject.Parse(content);
				Console.WriteLine(parsedContent);
				// var name = parsedContent.GetValue("name");
				// var favouriteGame = parsedContent.GetValue("favourite_game");
				// Console.WriteLine($"Welcome {name}, would you like to play some {favouriteGame}?");
			}
		}
		static async Task Main(string[] args)
		{
			await MakeApiCalls();
		}

	}
}