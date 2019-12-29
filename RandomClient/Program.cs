using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace RandomClient
{
	class Program
	{
		static async Task<TokenResponse> RequestToken()
		{
			var client = new HttpClient();
			var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
			if (disco.IsError)
			{
				throw new Exception(disco.Error);
			}

			// request token
			var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			{
				Address = disco.TokenEndpoint,

					ClientId = "random.console",
					ClientSecret = "secret",
					Scope = "random.api"
			});

			if (tokenResponse.IsError)
			{
				throw new Exception(tokenResponse.Error);
			}

			// Console.WriteLine(tokenResponse.Json);
			return tokenResponse;
		}
		static async Task Main(string[] args)
		{
			// call api
			var client = new HttpClient();
			var tokenResponse = await RequestToken();
			client.SetBearerToken(tokenResponse.AccessToken);

			var response = await client.GetAsync("http://localhost:5001/identity");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(JArray.Parse(content));
			}

			response = await client.GetAsync("http://localhost:5001/random");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(JObject.Parse(content));
			}
		}
	}
}