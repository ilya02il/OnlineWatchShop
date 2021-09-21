using AutoMapper;
using OnlineWatchShop.DAL.Contracts.Entities;
using OnlineWatchShop.Web.Models;
using System.Collections.Generic;

namespace OnlineWatchShop.Web.Profiles
{
	public class MainProfile : Profile
	{
		public MainProfile()
		{
			CreateMap<ProductEntity, ProductDto>();
			CreateMap<ProductDto, ProductEntity>();
			CreateMap<List<ProductEntity>, List<ProductDto>>();
			CreateMap<List<ProductDto>, List<ProductEntity>>();
		}
	}
}
