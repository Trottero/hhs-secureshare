using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;

namespace SecureShare.WebApi.Wrapper.Services
{
	public class ShareFileService : IShareFileService
	{
		private readonly IHttpService _httpService;
		private readonly IUserFileService _userFileService;

		public ShareFileService(IHttpService httpService, IUserFileService userFileService)
		{
			_httpService = httpService;
			_userFileService = userFileService;
		}

		public async Task<IEnumerable<UserFile>> GetSharedWithUserAsync(Guid id)
		{
			var result = await _httpService.GetOneRequestAsync<UserFile>(id.ToString(), "sharedwith/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}

		public async Task<IEnumerable<UserFile>> GetSharedWithUserAsync(string id)
		{
			var result = await _httpService.GetOneRequestAsync<UserFile>(id, "sharedwith/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}

		public async Task<IEnumerable<UserFile>> GetSharedWithUserAsync(User user)
		{
			var userId = user.UserId.ToString();
			var result = await _httpService.GetOneRequestAsync<UserFile>(userId, "sharedwith/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}

		public async Task<Users_UserFiles> AddSharedFile(IFormFile file, Guid owner,User sharingUser)
		{
			var userfile = await _userFileService.AddUserFileAsync(file, owner);
			var usersUserFiles = new Users_UserFiles
			{
				UserFileId = userfile.UserFileId,
				UserId = sharingUser.UserId
			};

			var result = await _httpService.PostRequestAsync<Users_UserFiles>(usersUserFiles);
			return JsonConvert.DeserializeObject<Users_UserFiles>(await result.ReadAsStringAsync());
		}

	    public async Task<Users_UserFiles> ShareFileAsync(Guid userFileId, Guid userToBeSharedWith)
	    {
            var userFile = new Users_UserFiles
            {
                UserFileId = userFileId,
                UserId = userToBeSharedWith,
                
            };
	        var result = await _httpService.PostRequestAsync<Users_UserFiles>(userFile);
            return JsonConvert.DeserializeObject<Users_UserFiles>(await result.ReadAsStringAsync());
        }

	    public async Task<IEnumerable<UserFile>> GetSharedWithOthersAsync(Guid id)
		{
			var result = await _httpService.GetOneRequestAsync<UserFile>(id.ToString(), "user/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}

		public async Task<IEnumerable<UserFile>> GetSharedWithOthersAsync(string id)
		{
			var result = await _httpService.GetOneRequestAsync<UserFile>(id, "user/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}

		public async Task<IEnumerable<UserFile>> GetSharedWithOthersAsync(User user)
		{
			var userId = user.UserId.ToString();
			var result = await _httpService.GetOneRequestAsync<UserFile>(userId, "user/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}
	}
}