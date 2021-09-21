using Microsoft.IdentityModel.Tokens;
using OnlineWatchShop.DAL.Contracts;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OnlineWatchShop.Web.Helpers;

namespace OnlineWatchShop.Web.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly IDbRepository _dbRepository;
		private readonly JwtConfiguration _jwtConfiguration;

		public AccountService(IDbRepository dbRepository, IOptions<JwtConfiguration> jwtConfigurationOptions)
		{
			_dbRepository = dbRepository;
			_jwtConfiguration = jwtConfigurationOptions.Value;
		}

		public async Task<object> Login(LoginModel model)
		{
			//var userEntity = await _dbRepository.GetAll<UserEntity>()
			//	.FirstOrDefaultAsync(user =>
			//		user.Login == model.Login &&
			//		user.HashedPassword == Hasher.HashPassword(model.Password, user.Salt));

			//if (userEntity == null)
			//	return null;

			return new
			{
				token = GenerateJwtToken(model),
				//username = claimsIdentity.Name
			};
		}

		public async Task<bool> Register(RegisterModel model)
		{
			//var userEntity = await _dbRepository.GetAll<UserEntity>()
			//	.FirstOrDefaultAsync(user => user.Login == model.Login);

			//if (userEntity != null) return false;

			//var salt = Hasher.GenerateSalt(20);

			//var newUserEntity = new UserEntity()
			//{
			//	Login = model.Login,
			//	Salt = salt,
			//	HashedPassword = Hasher.HashPassword(model.Password, salt),
			//	RoleId = 2
			//};

			//_dbRepository.Add(newUserEntity);
			//await _dbRepository.SaveChangesAsync();

			return true;
		}

		public Task Authenticate(string login)
		{
			throw new NotImplementedException();
		}

		private string GenerateJwtToken(LoginModel model)
		{
			// generate token that is valid for 7 days
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key);

			var claims = new List<Claim>()
			{
				new(ClaimsIdentity.DefaultNameClaimType, model.Login),
				//new(ClaimsIdentity.DefaultRoleClaimType, model.Role.Name)
			};

			var claimsIdentity = new ClaimsIdentity(claims, "Token",
				ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claimsIdentity,
				Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.Lifetime),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
