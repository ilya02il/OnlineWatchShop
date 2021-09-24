using OnlineWatchShop.DAL.Contracts.Entities;

namespace OnlineWatchShop.Web.Contracts
{
	public interface IJwtUtils
	{
		string GenerateJwtToken(UserEntity userEntity);
		int? ValidateJwtToken(string token);
		RefreshTokenEntity GenerateRefreshToken(string ipAddress);
	}
}
