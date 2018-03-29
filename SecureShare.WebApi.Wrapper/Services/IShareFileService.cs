using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services
{
	public interface IShareFileService
	{
		Task<IEnumerable<Users_UserFiles>> ShareFileAsync(Users_UserFiles sharefile);
		Task<IEnumerable<UserFile>> GetSharedFilesFromUser(Guid id);
	}
}