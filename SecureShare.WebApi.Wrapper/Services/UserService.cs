using System;
using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;


namespace SecureShare.WebApi.Wrapper.Services
{
	public class UserService : IUserService
	{
		private readonly IHttpService _service;

		public UserService(IHttpService service)
		{
			_service = service;
		}

		public async Task<string> GetAllUsers()
		{
			return  _service.GetAllRequest<User>().Result;
		}

		public async Task<string> GetUserByEntity(User user)
		{
			return await _service.GetOneRequest<User>(user.UserId);
		}

		public async Task<string> GetUserById(Guid userId)
		{
			return await _service.GetOneRequest<User>(userId);
		}

		public async Task<string> AddUser(User user)
		{
			return await _service.PostRequest<User>(user);
		}

		public async Task<string> DeleteUserByEntity(User user)
		{
			return await _service.DeleteRequest<User>(user.UserId);
		}

		public async Task<string> DeleteUserById(Guid userId)
		{
			return await _service.DeleteRequest<User>(userId);
		}

	
	}
}