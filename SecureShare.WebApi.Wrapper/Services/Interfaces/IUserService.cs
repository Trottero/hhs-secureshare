using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces

{
	public interface IUserService
	{
		string AddUser(User user);

		string DeleteUser(User user);

		string GetAllUsers();

		string GetOneUser(User user);
	}
}