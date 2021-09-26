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
	public class OrderService : IOrderService
	{
		private readonly IDbRepository _dbRepository;
		private readonly IMapper _mapper;
		public OrderService(IDbRepository dbRepository, IMapper mapper)
		{
			_dbRepository = dbRepository;
			_mapper = mapper;
		}

		public IEnumerable<OrderModel> GetAll()
		{
			var orderEntities = _dbRepository.GetAllInclude<OrderEntity>(c => c.OrderProducts).ToList();
			var orderModels = new List<OrderModel>();

			foreach (var orderEntity in orderEntities)
			{
				var orderProductEntities = GetProducts(orderEntity.OrderProducts);
				var orderProductModels = new List<ProductModel>();

				foreach (var orderProductEntity in orderProductEntities)
				{
					var orderProductModel = _mapper.Map<ProductModel>(orderProductEntity);

					orderProductModels.Add(orderProductModel);
				}

				var orderModel = _mapper.Map<OrderModel>(orderEntity);
				orderModel.Products = orderProductModels;

				orderModels.Add(orderModel);
			}

			return orderModels;
		}

		public OrderModel GetSingle(int id)
		{
			var orderEntity = _dbRepository.GetAllInclude<OrderEntity>(c => c.OrderProducts)
				.Where(e => e.Id == id).FirstOrDefault();
			var orderModel = _mapper.Map<OrderModel>(orderEntity);

			var orderProducts = GetProducts(orderEntity.OrderProducts);

			var orderProductModels = new List<ProductModel>();

			foreach (var orderProductEntity in orderProducts)
			{
				var orderProductModel = _mapper.Map<ProductModel>(orderProductEntity);

				orderProductModels.Add(orderProductModel);
			}

			orderModel.Products = orderProductModels;

			return orderModel;
		}

		public async Task<int> Add(OrderModel newOrder)
		{
			var orderEntity = _mapper.Map<OrderEntity>(newOrder);
			await _dbRepository.Add(orderEntity);

			await _dbRepository.SaveChangesAsync();

			var newOrderProducts = GetOrderProducts(orderEntity.Id, newOrder.Products);
			await _dbRepository.AddRange(newOrderProducts);

			await _dbRepository.SaveChangesAsync();

			return orderEntity.Id;
		}

		public async Task<OrderModel> Update(OrderModel orderModel)
		{
			var orderEntity = _mapper.Map<OrderEntity>(orderModel);

			await _dbRepository.Remove<OrderProductEntity>(cp => cp.OrderId == orderEntity.Id);

			var newOrderProducts = GetOrderProducts(orderEntity.Id, orderModel.Products);
			await _dbRepository.AddRange(newOrderProducts);

			//await _dbRepository.Update(orderEntity);
			await _dbRepository.SaveChangesAsync();

			return orderModel;
		}

		public async Task<int> Delete(int id)
		{
			var orderEntity = _dbRepository.GetAll<OrderEntity>()
				.FirstOrDefault(e => e.Id == id);

			await _dbRepository.Remove(orderEntity);
			await _dbRepository.SaveChangesAsync();

			return id;
		}

		private IEnumerable<OrderProductEntity> GetOrderProducts(int orderId, ICollection<ProductModel> productModels)
		{
			var orderProducts = new List<OrderProductEntity>();

			foreach (var productModel in productModels)
			{
				var orderProduct = new OrderProductEntity()
				{
					OrderId = orderId,
					ProductId = productModel.Id,
					Amount = productModel.Amount
				};

				orderProducts.Add(orderProduct);
			}

			return orderProducts;
		}

		private IEnumerable<ProductEntity> GetProducts(ICollection<OrderProductEntity> orderProductEntities)
		{
			var productEntities = new List<ProductEntity>();

			foreach (var orderProductEntity in orderProductEntities)
			{
				var productEntity = _dbRepository.Get<ProductEntity>(p => p.Id == orderProductEntity.ProductId).FirstOrDefault();

				if (productEntity.Id != 0)
					productEntities.Add(productEntity);
			}

			return productEntities;
		}
	}
}
