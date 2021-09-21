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

		public IEnumerable<ProductDto> GetAll()
		{
			//var productsEntities = _dbRepository.GetAll<ProductEntity>();
			//var productsDto = _mapper.Map<List<ProductDto>>(productsEntities);

			var productsDto = new List<ProductDto>()
			{
				new()
				{
					Id = 1,
					Name = "dsfdsfsd"
				},
				new()
				{
					Id = 2,
					Name = "kdsjflk"
				}
			};

			return productsDto;
		}

		public ProductDto GetSingle(int id)
		{
			var productEntity = _dbRepository.GetAll<ProductEntity>()
				.FirstOrDefault(e => e.Id == id);
			var productDto = _mapper.Map<ProductDto>(productEntity);

			return productDto;
		}

		public async Task<int> Add(ProductDto newProduct)
		{
			var productEntity = _mapper.Map<ProductEntity>(newProduct);

			var newId = _dbRepository.Add(productEntity);
			await _dbRepository.SaveChangesAsync();

			return newId;
		}

		public async Task<ProductDto> Update(ProductDto productDto)
		{
			var productEntity = _mapper.Map<ProductEntity>(productDto);

			await _dbRepository.Update(productEntity);
			await _dbRepository.SaveChangesAsync();

			return productDto;
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
