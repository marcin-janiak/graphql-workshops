using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BoardsWorkshops.API.DataAccess;
using BoardsWorkshops.API.Graph.Users;
using Microsoft.IdentityModel.Tokens;

namespace BoardsWorkshops.API.Identity
{
	public interface IIdentityService
	{
		Task<string> Authenticate(string username, string password);
	}

	public class IdentityService : IIdentityService
	{
		private readonly IUserRepository _userRepository;

		public IdentityService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<string> Authenticate(string username, string password)
		{
			var user = await _userRepository.GetByUsernameAsync(username);

			if (user == null || user.Password != password)
			{
				throw new AuthenticationException();
			}

			return GenerateAccessToken(user);
		}

		private string GenerateAccessToken(User user)
		{
			var key = new SymmetricSecurityKey(
			                                   Encoding.UTF8.GetBytes("secretsecretsecret")
			                                  );

			var claims = new List<Claim>
			             {
					             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					             new Claim(ClaimTypes.Name, user.Username),
			             };

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
			                                 "issuer",
			                                 "audience",
			                                 claims,
			                                 expires: DateTime.Now.AddDays(90),
			                                 signingCredentials: creds
			                                );

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}