using System;
using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces

{
	public interface IUserService
	{
		Task<string> GetAllUsers();
		Task<string> GetUserByEntity(User user);
		Task<string> GetUserById(Guid userId);
		Task<string> AddUser(User user);
		Task<string> DeleteUserByEntity(User user);
		Task<string> DeleteUserById(Guid userId);
	}
}