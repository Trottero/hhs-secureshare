using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services
{
	public interface IShareFileService
	{
		Task<IEnumerable<UserFile>> GetSharedWithOthersAsync(Guid id);
		Task<IEnumerable<UserFile>> GetSharedWithOthersAsync(string id);
		Task<IEnumerable<UserFile>> GetSharedWithOthersAsync(User user);
		Task<IEnumerable<UserFile>> GetSharedWithUserAsync(Guid id);
		Task<IEnumerable<UserFile>> GetSharedWithUserAsync(string id);
		Task<IEnumerable<UserFile>> GetSharedWithUserAsync(User user);
		Task<Users_UserFiles> AddSharedFile(IFormFile file, Guid owner, User sharingUser);

	    Task<Users_UserFiles> ShareFileAsync(Guid userFileId, Guid userToBeSharedWith);
	}
}