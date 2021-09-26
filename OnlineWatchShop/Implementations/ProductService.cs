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
	public class ProductService : IProductService
	{
		private readonly IDbRepository _dbRepository;
		private readonly IMapper _mapper;

		public ProductService(IDbRepository dbRepository, IMapper mapper)
		{
			_dbRepository = dbRepository;
			_mapper = mapper;
		}

		public IEnumerable<ProductModel> GetAll()
		{
			var productEntities = _dbRepository.GetAllInclude<ProductEntity>(p => p.Images);
			var productModels = _mapper.Map<List<ProductModel>>(productEntities);

			return productModels;
		}

		public ProductModel GetSingle(int id)
		{
			var productEntity = _dbRepository.GetAllInclude<ProductEntity>(p => p.Images)
				.FirstOrDefault(e => e.Id == id);
			var productModel = _mapper.Map<ProductModel>(productEntity);

			return productModel;
		}

		public async Task<int> Add(ProductModel newProduct)
		{
			var productEntity = _mapper.Map<ProductEntity>(newProduct);

			var newId = await _dbRepository.Add(productEntity);
			await _dbRepository.SaveChangesAsync();

			return newId;
		}

		public async Task<ProductModel> Update(ProductModel productModel)
		{
			var productEntity = _mapper.Map<ProductEntity>(productModel);

			await _dbRepository.Update(productEntity);
			await _dbRepository.SaveChangesAsync();

			return productModel;
		}

		public async Task<int> Delete(int id)
		{
			var productEntity = _dbRepository.GetAll<ProductEntity>()
				.FirstOrDefault(e => e.Id == id);

			await _dbRepository.Remove(productEntity);
			await _dbRepository.SaveChangesAsync();

			return id;
		}
	}
}
