using Hashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineWatchShop.DAL.Contracts;
using OnlineWatchShop.DAL.Contracts.Entities;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Helpers;
using OnlineWatchShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWatchShop.Web.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly IDbRepository _dbRepository;
		private readonly JwtConfiguration _jwtConfiguration;
		private readonly IJwtUtils _jwtUtils;

		public AccountService(IDbRepository dbRepository, IOptions<JwtConfiguration> jwtConfigurationOptions,
			IJwtUtils jwtUtils)
		{
			_dbRepository = dbRepository;
			_jwtUtils = jwtUtils;
			_jwtConfiguration = jwtConfigurationOptions.Value;
		}

		public async Task<AuthenticateResponseModel> Authenticate(LoginModel model, string ipAddress)
		{
			var userEntity = await _dbRepository.GetAllInclude<UserEntity>(user => user.Role)
				.FirstOrDefaultAsync(user =>
					user.Login == model.Login);

			if (userEntity == null)
				return null;

			if (userEntity.HashedPassword != Hasher.HashPassword(model.Password, userEntity.Salt))
				return null;

			var accessToken = _jwtUtils.GenerateJwtToken(userEntity);
			var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);

			refreshToken.UserId = userEntity.Id;
			await _dbRepository.Add(refreshToken);

			await RemoveOldRefreshTokens(userEntity.Id);

			await _dbRepository.SaveChangesAsync();

			return new AuthenticateResponseModel()
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken.Token,
				UserId = userEntity.Id,
				Username = userEntity.Login,
				AccessTokenExpiry = DateTime.Now.AddMinutes(10)
			};
		}

		public async Task<bool> Register(RegisterModel model)
		{
			var userEntity = await _dbRepository.GetAll<UserEntity>()
				.FirstOrDefaultAsync(user => user.Login == model.Login);

			if (userEntity != null) return false;

			var salt = Hasher.GenerateSalt(20);

			var newUserEntity = new UserEntity()
			{
				Login = model.Login,
				Salt = salt,
				HashedPassword = Hasher.HashPassword(model.Password, salt),
				RoleId = 2
			};

			await _dbRepository.Add(newUserEntity);
			await _dbRepository.SaveChangesAsync();

			return true;
		}

		public async Task<AuthenticateResponseModel> RefreshToken(string token, string ipAddress)
		{
			var refreshToken = _dbRepository.GetAll<RefreshTokenEntity>()
				.ToList().Single(t => t.Token == token);
			var userEntity = GetUserByRefreshToken(token);


			if (refreshToken.IsRevoked)
			{
				RevokeDescendantRefreshTokens(refreshToken, userEntity, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
				_dbRepository.SaveChanges();
			}

			if (!refreshToken.IsActive)
				throw new Exception("Invalid token");

			var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
			newRefreshToken.UserId = userEntity.Id;

			await _dbRepository.Add(newRefreshToken);

			await RemoveOldRefreshTokens(userEntity.Id);

			await _dbRepository.SaveChangesAsync();

			var jwtToken = _jwtUtils.GenerateJwtToken(userEntity);

			return new AuthenticateResponseModel()
			{
				Username = userEntity.Login,
				AccessToken = jwtToken,
				RefreshToken = newRefreshToken.Token,
				AccessTokenExpiry = DateTime.Now.AddMinutes(10)
			};
		}

		public void RevokeToken(string token, string ipAddress)
		{
			var refreshToken = _dbRepository.GetAll<RefreshTokenEntity>()
				.ToList()
				.Single(t => t.Token == token);

			if (!refreshToken.IsActive)
				throw new Exception("Invalid token");

			// revoke token and save
			RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");

			_dbRepository.SaveChanges();
		}

		public UserEntity GetById(int id)
		{
			var user = _dbRepository.GetAllInclude<UserEntity>(u => u.Role)
			.Where(u => u.Id == id)
			.FirstOrDefault();

			if (user == null) throw new KeyNotFoundException("User not found");
			return user;
		}

		private void RevokeDescendantRefreshTokens(RefreshTokenEntity refreshToken, UserEntity user, string ipAddress, string reason)
		{
			// recursively traverse the refresh token chain and ensure all descendants are revoked
			if (string.IsNullOrEmpty(refreshToken.ReplaceByToken)) return;

			var childToken = _dbRepository.GetAll<RefreshTokenEntity>()
				.Where(rt => rt.UserId == user.Id)
				.SingleOrDefault(x => x.Token == refreshToken.ReplaceByToken);

			if (childToken == null) 
				return;

			if (childToken.IsActive)
				RevokeRefreshToken(childToken, ipAddress, reason);
			else
				RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
		}

		private void RevokeRefreshToken(RefreshTokenEntity token, string ipAddress, string reason = null, string replacedByToken = null)
		{
			token.Revoked = DateTime.UtcNow;
			token.RevokedByIp = ipAddress;
			token.ReasonRevoked = reason;
			token.ReplaceByToken = replacedByToken;

			_dbRepository.Update(token);
		}

		private async Task RemoveOldRefreshTokens(int userId)
		{
			// remove old inactive refresh tokens from user based on TTL in app settings
			await _dbRepository.Remove<RefreshTokenEntity>(entity =>
				entity.UserId == userId &&
				!entity.IsActive &&
				entity.Created.AddMinutes(_jwtConfiguration.Lifetime) <= DateTime.UtcNow);
		}

		private RefreshTokenEntity RotateRefreshToken(RefreshTokenEntity refreshToken, string ipAddress)
		{
			var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
			RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
			return newRefreshToken;
		}
		private UserEntity GetUserByRefreshToken(string token)
		{
			var userEntity = _dbRepository.GetAllInclude<UserEntity>(u => u.Role)
			.SingleOrDefault(user => user.RefreshTokens
			.Any(t => t.Token == token));
				
			return userEntity;
		}
	}
}