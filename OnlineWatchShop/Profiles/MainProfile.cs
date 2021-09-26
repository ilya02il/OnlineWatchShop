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
			//Map ProductEntity and ProductModel
			CreateMap<ProductEntity, ProductModel>();
			CreateMap<ProductModel, ProductEntity>();
			CreateMap<List<ProductEntity>, List<ProductModel>>();
			CreateMap<List<ProductModel>, List<ProductEntity>>();
			//Map CartEntity and CartModel
			CreateMap<CartEntity, CartModel>();
			CreateMap<CartModel, CartEntity>();
			CreateMap<List<CartEntity>, List<CartModel>>();
			CreateMap<List<CartModel>, List<CartEntity>>();
			//Map OrderEntity and OrderModel
			CreateMap<OrderEntity, OrderModel>();
			CreateMap<OrderModel, OrderEntity>();
			CreateMap<List<OrderEntity>, List<OrderModel>>();
			CreateMap<List<OrderModel>, List<OrderEntity>>();
			//Map ImageEntity and ImageModel
			CreateMap<ImageEntity, ImageModel>();
			CreateMap<ImageModel, ImageEntity>();
			CreateMap<List<ImageEntity>, List<ImageModel>>();
			CreateMap<List<ImageModel>, List<ImageEntity>>();
			//Map UserEntity and UserModel
			CreateMap<UserEntity, UserModel>();
			CreateMap<UserModel, UserEntity>();
			CreateMap<List<UserEntity>, List<UserModel>>();
			CreateMap<List<UserModel>, List<UserEntity>>();
		}
	}
}
