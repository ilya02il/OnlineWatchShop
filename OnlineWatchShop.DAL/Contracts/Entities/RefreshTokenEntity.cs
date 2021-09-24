using System;

namespace OnlineWatchShop.DAL.Contracts.Entities
{
	public class RefreshTokenEntity : IEntity
	{
		public int Id { get; set; }

		public string Token { get; set; }
		public DateTime Expires { get; set; }
		public DateTime Created { get; set; }
		public string CreatedByIp { get; set; }
		public DateTime? Revoked { get; set; }
		public string RevokedByIp { get; set; }
		public string ReplaceByToken { get; set; }
		public string ReasonRevoked { get; set; }
		public bool IsExpired
		{
			get
			{
				return DateTime.UtcNow >= Expires;
			}
			private set { }
		}
		
		public bool IsRevoked
		{
			get
			{
				return Revoked != null;
			}
			private set { }
		}
		public bool IsActive
		{
			get
			{
				return !IsRevoked && !IsExpired;
			}
			private set { }
		}

		public int UserId { get; set; }
		public UserEntity User { get; set; }
	}
}
