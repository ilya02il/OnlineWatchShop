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
			CreateMap<ProductEntity, ProductModel>();
			CreateMap<ProductModel, ProductEntity>();
			CreateMap<List<ProductEntity>, List<ProductModel>>();
			CreateMap<List<ProductModel>, List<ProductEntity>>();
		}
	}
}
