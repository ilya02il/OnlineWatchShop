using AutoMapper;
using OnlineWatchShop.DAL.Contracts;
using OnlineWatchShop.DAL.Contracts.Entities;
using OnlineWatchShop.Web.Contracts;
using OnlineWatchShop.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWatchShop.Web.Implementations
{
	public class CartService : ICartService
	{
		private readonly IDbRepository _dbRepository;
		private readonly IMapper _mapper;
		public CartService(IDbRepository dbRepository, IMapper mapper)
		{
			_dbRepository = dbRepository;
			_mapper = mapper;
		}

		public IEnumerable<CartModel> GetAll()
		{
			var cartEntities = _dbRepository.GetAllInclude<CartEntity>(c => c.CartProducts).ToList();
			var cartModels = new List<CartModel>();

			foreach (var cartEntity in cartEntities)
			{
				var cartProductEntities = GetProducts(cartEntity.CartProducts);
				var cartProductModels = new List<ProductModel>();

				foreach (var cartProductEntity in cartProductEntities)
				{
					var cartProductModel = _mapper.Map<ProductModel>(cartProductEntity);

					cartProductModels.Add(cartProductModel);
				}

				var cartModel = _mapper.Map<CartModel>(cartEntity);
				cartModel.Products = cartProductModels;

				cartModels.Add(cartModel);
			}

			return cartModels;
		}

		public CartModel GetSingle(int id)
		{
			var cartEntity = _dbRepository.GetAllInclude<CartEntity>(c => c.CartProducts)
				.Where(e => e.Id == id).FirstOrDefault();
			var cartModel = _mapper.Map<CartModel>(cartEntity);

			var cartProducts = GetProducts(cartEntity.CartProducts);

			var cartProductModels = new List<ProductModel>();

			foreach (var cartProductEntity in cartProducts)
			{
				var cartProductModel = _mapper.Map<ProductModel>(cartProductEntity);

				cartProductModels.Add(cartProductModel);
			}

			cartModel.Products = cartProductModels;

			return cartModel;
		}

		public async Task<int> Add(CartModel newCart)
		{
			var cartEntity = _mapper.Map<CartEntity>(newCart);
			await _dbRepository.Add(cartEntity);

			await _dbRepository.SaveChangesAsync();

			var newCartProducts = GetCartProducts(cartEntity.Id, newCart.Products);
			await _dbRepository.AddRange(newCartProducts);

			await _dbRepository.SaveChangesAsync();

			return cartEntity.Id;
		}

		public async Task<CartModel> Update(CartModel cartModel)
		{
			var cartEntity = _mapper.Map<CartEntity>(cartModel);

			await _dbRepository.Remove<CartProductEntity>(cp => cp.CartId == cartEntity.Id);

			var newCartProducts = GetCartProducts(cartEntity.Id, cartModel.Products);
			await _dbRepository.AddRange(newCartProducts);

			//await _dbRepository.Update(cartEntity);
			await _dbRepository.SaveChangesAsync();

			return cartModel;
		}

		public async Task<int> Delete(int id)
		{
			var cartEntity = _dbRepository.GetAll<CartEntity>()
				.FirstOrDefault(e => e.Id == id);

			await _dbRepository.Remove(cartEntity);
			await _dbRepository.SaveChangesAsync();

			return id;
		}

		private IEnumerable<CartProductEntity> GetCartProducts(int cartId, ICollection<ProductModel> productModels)
		{
			var cartProducts = new List<CartProductEntity>();

			foreach(var productModel in productModels)
			{
				var cartProduct = new CartProductEntity()
				{
					CartId = cartId,
					ProductId = productModel.Id,
					Amount = productModel.Amount
				};

				cartProducts.Add(cartProduct);
			}

			return cartProducts;
		}

		private IEnumerable<ProductEntity> GetProducts(ICollection<CartProductEntity> cartProductEntities)
		{
			var productEntities = new List<ProductEntity>();

			foreach (var cartProductEntity in cartProductEntities)
			{
				var productEntity = _dbRepository.Get<ProductEntity>(p => p.Id == cartProductEntity.ProductId).FirstOrDefault();

				if (productEntity.Id != 0)
					productEntities.Add(productEntity);
			}

			return productEntities;
		}
	}
}
