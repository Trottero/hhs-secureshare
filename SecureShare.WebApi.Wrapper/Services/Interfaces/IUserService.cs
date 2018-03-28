using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces

{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserAsync(User user);
		Task<User> GetUserAsync(string userId);
		Task<User> GetUserAsync(Guid userId);
		Task<User> AddUserAsync(User user);
		Task<User> DeleteUserAsync(User user);
		Task<User> DeleteUserAsync(string userId);
		Task<User> DeleteUserAsync(Guid userId);
	}
}