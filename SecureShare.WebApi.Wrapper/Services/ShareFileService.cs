using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;

namespace SecureShare.WebApi.Wrapper.Services
{
	public class ShareFileService : IShareFileService
	{
		private readonly IHttpService _httpService;

		public ShareFileService(IHttpService httpService)
		{
			_httpService = httpService;
		}

		public Task<IEnumerable<Users_UserFiles>> ShareFileAsync(Users_UserFiles sharefile)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<UserFile>> GetSharedFilesFromUser(Guid id)
		{
			var result = await _httpService.GetOneRequestAsync<UserFile>(id.ToString(), "user/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}
	}
}