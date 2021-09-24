using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineWatchShop.DAL.Contracts.Entities;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineWatchShop.Web.Implementations
{
	public class JwtUtils : IJwtUtils
	{
		private readonly JwtConfiguration _jwtConfiguration;

		public JwtUtils(IOptions<JwtConfiguration> jwtConfigurationOptions)
		{
			_jwtConfiguration = jwtConfigurationOptions.Value;
		}

		public string GenerateJwtToken(UserEntity userEntity)
		{

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key);

			var claims = new List<Claim>()
			{
				new("Id", userEntity.Id.ToString()),
				new("Role", userEntity.Role.Name)
			};

			var claimsIdentity = new ClaimsIdentity(claims);

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

		public int? ValidateJwtToken(string token)
		{
			if (token == null)
				return null;

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key);
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);
				return userId;
			}
			catch
			{
				// return null if validation fails
				return null;
			}
		}

		public RefreshTokenEntity GenerateRefreshToken(string ipAddress)
		{
			using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

			var randomBytes = new byte[64];
			rngCryptoServiceProvider.GetBytes(randomBytes);

			var refreshToken = new RefreshTokenEntity()
			{
				Token = Convert.ToBase64String(randomBytes),
				Expires = DateTime.UtcNow.AddDays(7),
				Created = DateTime.UtcNow,
				CreatedByIp = ipAddress
			};

			return refreshToken;
		}
	}
}
