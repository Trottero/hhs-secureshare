using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
			var metadataResult = (await _httpService.GetOneRequestAsync<UserFile>(id.ToString())).ReadAsStringAsync();
			var userFileMetadata = JsonConvert.DeserializeObject<UserFile>(await metadataResult);

			return userFileMetadata;
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

		public async Task<FileDownloadInfo> GetUserFileDownloadPathAsync(UserFile userFileMetadata)
		{
			var azureBlobStream = await (await _httpService.GetOneRequestAsync<FileStream>(userFileMetadata.BlobId.ToString()))
				.ReadAsStreamAsync();

			//File path where the file will be stored on the server.
			var filePathToDownloadFrom = Path.GetTempPath();

			//File stream will be created with the right filepath + file name
			var destionationWriteStream = new FileStream(filePathToDownloadFrom + userFileMetadata.FileName, FileMode.Create);
			azureBlobStream.CopyTo(destionationWriteStream);
			byte[] bytes;

			//Memory stream will be used to convert the filestram to a byte array so it can be written
			using (var memoryStream = new MemoryStream())
			{
				azureBlobStream.CopyTo(memoryStream);
				bytes = memoryStream.ToArray();
			}

			await destionationWriteStream.WriteAsync(bytes, 0, bytes.Length);
			destionationWriteStream.Close();

			var fileDownloadInfo = new FileDownloadInfo()
			{
				FileName = userFileMetadata.FileName,
				FileType = userFileMetadata.FileType,
				RootPath = filePathToDownloadFrom
			};
			return fileDownloadInfo;
		}

		public async Task<UserFile> AddUserFileAsync(IFormFile file, Guid owner)
		{
			//the goal of this method is to send 2 requests. one to add the blob and one to add the actual userfile.

			//Send file to blob
			var resultFromAzure = await _httpService.PostRequestAsync<FileStream>(file);

			//now that we got a response from azure we need to serialize it to a string before converting it to a guid!
			var deserializedResponse = JsonConvert.DeserializeObject<string>(await resultFromAzure.ReadAsStringAsync());
			var blobGuid = new Guid(deserializedResponse);

			//Now that we have all of the required data we are going to create a new userfile object
			var userFileToUpload = new UserFile()
			{
				BlobId = blobGuid,
				Description = "", //not implemented yet!
				FileName = file.FileName,
				FileType = file.ContentType,
				OwnerId = owner
				
			};
			var result = await _httpService.PostRequestAsync<UserFile>(userFileToUpload);
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

		public async Task<IEnumerable<UserFile>> GetFilesFromUserAsync(Guid id)
		{
			var result = await _httpService.GetOneRequestAsync<UserFile>(id.ToString(), "user/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}

		public async Task<IEnumerable<UserFile>> GetFilesFromUserAsync(string id)
		{
			var result = await _httpService.GetOneRequestAsync<UserFile>(id, "user/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}

		public async Task<IEnumerable<UserFile>> GetFilesFromUserAsync(User user)
		{
			var id = user.UserId.ToString();
			var result = await _httpService.GetOneRequestAsync<UserFile>(id, "user/");
			return JsonConvert.DeserializeObject<IEnumerable<UserFile>>(await result.ReadAsStringAsync());
		}
	}
}