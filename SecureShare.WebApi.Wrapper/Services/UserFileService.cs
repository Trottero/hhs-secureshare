using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;

namespace SecureShare.WebApi.Wrapper.Services
{
	//The purpose of this class is to provide easy access to all API functions related to UserFiles.
	public class UserFileService : IUserFileService
	{
		private readonly IHttpService _httpService;

		public UserFileService(IHttpService httpService)
		{
			_httpService = httpService;
		}

//		public async Task<IEnumerable<UserFile>> GetAllUserFilesAsync()
//		{
//			var result = await _httpService.GetAllRequestAsync<UserFile>();
//			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(result);
//		}

		public async Task<UserFile> GetUserFileAsync(Guid id)
		{
			var result = (await _httpService.GetOneRequestAsync<UserFile>(id.ToString())).ReadAsStringAsync();
			var user = JsonConvert.DeserializeObject<UserFile>(await result);
			var result2 = await (await _httpService.GetOneRequestAsync<FileStream>(user.BlobId.ToString())).ReadAsStreamAsync();
			var writestream = new FileStream(Path.GetTempPath() + user.FileName, FileMode.Create);
			result2.CopyTo(writestream);
			byte[] bytes;
			using (var memoryStream = new MemoryStream())
			{
				result2.CopyTo(memoryStream);
				bytes = memoryStream.ToArray();
			}

			await writestream.WriteAsync(bytes, 0, bytes.Length);
			writestream.Close();
			return user;
		}

		public Task<UserFile> GetUserFileAsync(string id)
		{
			//check if it is a valid guid.
			if (!Guid.TryParse(id, out _))
				throw new ArgumentException(id + " is an invalid argument.");

			var guid = new Guid(id);
			return GetUserFileAsync(guid);
		}

		public Task<UserFile> GetUserFileAsync(UserFile file)
		{
			return GetUserFileAsync(file.OwnerId);
		}

		public async Task<UserFile> AddUserFileAsync(UserFile file)
		{
			var result = await _httpService.PostRequestAsync<UserFile>(file);
			return JsonConvert.DeserializeObject<UserFile>(await result.ReadAsStringAsync());
		}

		public Task<UserFile> DeleteUserFileAsync(UserFile user)
		{
			return DeleteUserFileAsync(user.UserFileId);
		}

		public async Task<UserFile> DeleteUserFileAsync(Guid id)
		{
			var result = await _httpService.DeleteRequestAsync<UserFile>(id.ToString());
			return JsonConvert.DeserializeObject<UserFile>(await result.ReadAsStringAsync());
		}

		public Task<UserFile> DeleteUserFileAsync(string id)
		{
			if (!Guid.TryParse(id, out _))
				throw new ArgumentException(id + " is an invalid argument.");

			var guid = new Guid(id);
			return DeleteUserFileAsync(guid);
		}
	}
}