using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;


namespace SecureShare.WebApi.Wrapper.Services
{
	class UserService : IUserService
	{
		private readonly IHttpService _service;

		public UserService(IHttpService service)
		{
			_service = service;
		}

		public string AddUser(User user)
		{
			return _service.PostRequest("users", user).Result;
		}

		public string DeleteUser(User user)
		{
			return _service.DeleteRequest("users", user.UserId.ToString()).Result;
		}

		public string GetAllUsers()
		{
			return _service.GetAllRequest("users").Result;
		}

		public string GetOneUser(User user)
		{
			throw new System.NotImplementedException();
		}

//		public User GetOneUser(User user)
//		{
//			return _service.GetOneRequest("users", user.UserId.ToString());
//		}
	}
}