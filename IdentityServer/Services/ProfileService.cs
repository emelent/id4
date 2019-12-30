using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;

namespace IdentityServer.Services
{

	public class ProfileService : IProfileService
	{
		private readonly TestUserStore _users;

		public ProfileService(TestUserStore users = null)
		{
			_users = users ?? new TestUserStore(TestUsers.Users);
		}

		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			//>Processing
			// var user = await _userManager.GetUserAsync(context.Subject);
			var user = _users.FindBySubjectId(context.Subject.GetSubjectId());

			var email = user.Claims.Where(c => c.Type == "email").Select(c => c.Value).Single();
			var name = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).Single();
			var claims = new List<Claim>
			{
				new Claim("email", email),
				new Claim("name", name),
			};

			context.IssuedClaims.AddRange(claims);
			await Task.Run(() =>
			{

			});
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			//>Processing
			// var user = _users.FindBySubjectId(context.Subject.GetSubjectId());
			// context.IsActive = tr
			// var user = await _userManager.GetUserAsync(context.Subject);

			// context.IsActive = (user != null) && user.IsActive;
			// return await Task.FromResult(null);

			var user = _users.FindBySubjectId(context.Subject.GetSubjectId());
			context.IsActive = (user != null) && user.IsActive;
			await Task.Run(() => { });
		}

	}
}