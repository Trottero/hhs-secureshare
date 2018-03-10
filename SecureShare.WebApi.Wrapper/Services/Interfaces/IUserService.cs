using System;
using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces

{
	public interface IUserService
	{
		Task<string> GetAllUsersAsync();
		Task<string> GetUserByEntityAsync(User user);
		Task<string> GetUserByIdAsync(Guid userId);
		Task<string> AddUserAsync(User user);
		Task<string> DeleteUserByEntityAsync(User user);
		Task<string> DeleteUserByIdAsync(Guid userId);
	}
}