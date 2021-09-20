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

namespace OnlineWatchShop.Web.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly IDbRepository _dbRepository;
		//private readonly AuthSettings _authSettings;

		public AccountService(IDbRepository dbRepository)
		{
			_dbRepository = dbRepository;
			//_authSettings = authSettings;
		}

		public async Task<string> Login(LoginModel model)
		{
			//var userEntity = await _dbRepository.GetAll<UserEntity>()
			//	.FirstOrDefaultAsync(user =>
			//		user.Login == model.Login &&
			//		user.HashedPassword == Hasher.HashPassword(model.Password, user.Salt));

			//var claims = new List<Claim>()
			//{
			//	new(ClaimsIdentity.DefaultNameClaimType, userEntity.Login),
			//	new(ClaimsIdentity.DefaultRoleClaimType, userEntity.Role.Name)
			//};

			var claims = new List<Claim>()
			{
				new(ClaimsIdentity.DefaultNameClaimType, model.Login),
				//new(ClaimsIdentity.DefaultRoleClaimType, model.Role.Name)
			};

			var claimsIdentity = new ClaimsIdentity(claims, "Token",
				ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);

			var jwtToken = new JwtSecurityToken(
				//issuer: _authSettings.Issuer,
				//audience: _authSettings.Audience,
				notBefore: DateTime.Now,
				claims: claimsIdentity.Claims,
				expires: DateTime.Now.Add(TimeSpan.FromMinutes(1)),
				signingCredentials: new SigningCredentials(
					new SymmetricSecurityKey(Encoding.ASCII.GetBytes("efefefefefefefefefefefsafdasdfadsfasdfadsfasdfasdfdas")),
					SecurityAlgorithms.HmacSha256)
			);

			return null;
			//return new JwtSecurityTokenHandler().WriteToken(jwtToken);
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
			throw new System.NotImplementedException();
		}
	}
}
