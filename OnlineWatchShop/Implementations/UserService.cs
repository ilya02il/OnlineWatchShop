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
	public class UserService : IUserService
	{
		private readonly IDbRepository _dbRepository;
		private readonly IMapper _mapper;
		public UserService(IDbRepository dbRepository, IMapper mapper)
		{
			_dbRepository = dbRepository;
			_mapper = mapper;
		}

		public IEnumerable<UserModel> GetAll()
		{
			var userEntities = _dbRepository.GetAllInclude<UserEntity>(u => u.PersonalData);
			var userModels = _mapper.Map<List<UserModel>>(userEntities);

			return userModels;
		}

		public UserModel GetSingle(int id)
		{
			var userEntity = _dbRepository.GetAllInclude<UserEntity>(u => u.PersonalData)
				.Where(e => e.Id == id)
				.FirstOrDefault();

			var userModel = _mapper.Map<UserModel>(userEntity);

			return userModel;
		}

		public async Task<int> Add(UserModel newUser)
		{
			var userEntity = _mapper.Map<UserEntity>(newUser);

			var newId = await _dbRepository.Add(userEntity);
			await _dbRepository.SaveChangesAsync();

			return newId;
		}

		public async Task<UserModel> Update(UserModel productModel)
		{
			var userEntity = _mapper.Map<UserEntity>(productModel);

			await _dbRepository.Update(userEntity);
			await _dbRepository.SaveChangesAsync();

			return productModel;
		}

		public async Task<int> Delete(int id)
		{
			var userEntity = _dbRepository.GetAll<UserEntity>()
				.FirstOrDefault(e => e.Id == id);

			await _dbRepository.Remove(userEntity);
			await _dbRepository.SaveChangesAsync();

			return id;
		}
	}
}
