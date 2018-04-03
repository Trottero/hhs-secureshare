using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;


namespace SecureShare.WebApi.Wrapper.Services
{
	public class UserService : IUserService
	{
		private readonly IHttpService _httpService;

		public UserService(IHttpService httpService)
		{
			_httpService = httpService;
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			var result = await _httpService.GetAllRequestAsync<User>();
			return JsonConvert.DeserializeObject<IEnumerable<User>>(await result.ReadAsStringAsync());
		}

		public Task<User> GetUserAsync(User user)
		{
			return GetUserAsync(user.UserId);
		}

		public Task<User> GetUserAsync(string userId)
		{
			//check if it is a valid guid.
			if (!Guid.TryParse(userId, out _))
				throw new ArgumentException(userId + " is an invalid argument.");

			var guid = new Guid(userId);
			return GetUserAsync(guid);
		}

		public async Task<User> GetUserAsync(Guid userId)
		{
			var result = await _httpService.GetOneRequestAsync<User>(userId.ToString());
			return JsonConvert.DeserializeObject<User>(await result.ReadAsStringAsync());
		}

		public async Task<User> AddUserAsync(User user)
		{
			var result = await _httpService.PostRequestAsync<User>(user);
			return JsonConvert.DeserializeObject<User>(await result.ReadAsStringAsync());
		}

		public Task<User> DeleteUserAsync(User user)
		{
			return DeleteUserAsync(user.UserId);
		}

		public Task<User> DeleteUserAsync(string userId)
		{
			//check if it is a valid guid.
			if (!Guid.TryParse(userId, out _))
				throw new ArgumentException(userId + " is an invalid argument.");

			var guid = new Guid(userId);
			return DeleteUserAsync(guid);
		}

		public async Task<User> DeleteUserAsync(Guid userId)
		{
			var result = await _httpService.DeleteRequestAsync<User>(userId.ToString());
			return JsonConvert.DeserializeObject<User>(await result.ReadAsStringAsync());
		}
	}
}