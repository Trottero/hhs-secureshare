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

		public async Task<string> GetAllUsersAsync()
		{
			return await _service.GetAllRequestAsync<User>();
		}

		public async Task<string> GetUserByEntityAsync(User user)
		{
			return await _service.GetOneRequestAsync<User>(user.UserId);
		}

		public async Task<string> GetUserByIdAsync(Guid userId)
		{
			return await _service.GetOneRequestAsync<User>(userId);
		}

		public async Task<string> AddUserAsync(User user)
		{
			return await _service.PostRequestAsync<User>(user);
		}

		public async Task<string> DeleteUserByEntityAsync(User user)
		{
			return await _service.DeleteRequestAsync<User>(user.UserId);
		}

		public async Task<string> DeleteUserByIdAsync(Guid userId)
		{
			return await _service.DeleteRequestAsync<User>(userId);
		}
	}
}