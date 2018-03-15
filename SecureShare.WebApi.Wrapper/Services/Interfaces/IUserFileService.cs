using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces
{
	public interface IUserFileService
	{
		Task<UserFile> AddUserFileAsync(UserFile file);
		Task<UserFile> DeleteUserFileAsync(Guid id);
		Task<UserFile> DeleteUserFileAsync(string id);
		Task<UserFile> DeleteUserFileAsync(UserFile user);
//		Task<IEnumerable<UserFile>> GetAllUserFilesAsync();
		Task<UserFile> GetUserFileAsync(Guid id);
		Task<UserFile> GetUserFileAsync(string id);
		Task<UserFile> GetUserFileAsync(UserFile file);
	}
}