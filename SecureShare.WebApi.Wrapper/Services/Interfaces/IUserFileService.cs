using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces
{
	public interface IUserFileService
	{
		Task<UserFile> AddUserFileAsync(IFormFile file, Guid owner);
		Task<UserFile> DeleteUserFileAsync(Guid id);
		Task<UserFile> DeleteUserFileAsync(string id);
		Task<UserFile> DeleteUserFileAsync(UserFile userFile);
//		Task<IEnumerable<UserFile>> GetAllUserFilesAsync();
		Task<UserFile> GetUserFileAsync(Guid id);
		Task<UserFile> GetUserFileAsync(string id);
		Task<UserFile> GetUserFileAsync(UserFile file);
        Task<FileDownloadInfo> GetUserFileDownloadPath(UserFile file);
	}
}