using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;

namespace SecureShare.WebApi.Wrapper.Services
{
	public class HttpService : IHttpService
	{
		private readonly HttpClient _client;
		private readonly ApiUrls _options;

		public HttpService(IOptions<ApiUrls> optionsAccessor)
		{
			_client = new HttpClient();
			_options = optionsAccessor.Value;
		}

		public async Task<HttpContent> GetAllRequestAsync<T>()
		{
			var requestUrl = GetEntityRequestUri<T>();
			HttpResponseMessage response = await _client.GetAsync(requestUrl);
			return response.Content;
		}

		public async Task<HttpContent> GetOneRequestAsync<T>(string entityId)
		{
			var requestUrl = GetEntityRequestUri<T>();
			HttpResponseMessage response = await _client.GetAsync(requestUrl + entityId);
			return response.Content;
		}

		public async Task<HttpContent> PostRequestAsync<T>(Entity entity)
		{
			var requestUrl = GetEntityRequestUri<T>();
			var stringContent = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _client.PostAsync(requestUrl, stringContent);

			return response.Content;
		}

		public async Task<HttpContent> DeleteRequestAsync<T>(string entityId)
		{
			var requestUrl = GetEntityRequestUri<T>();
			HttpResponseMessage response = await _client.DeleteAsync(requestUrl + entityId);
			return response.Content;
		}

		private Uri GetEntityRequestUri<T>()
		{
			if (typeof(T) == typeof(User)) return new Uri(_options.UserUrl);
			if (typeof(T) == typeof(UserFile)) return new Uri(_options.FileUrl);
			if (typeof(T) == typeof(FileStream)) return new Uri(_options.BlobFileUrl);
			throw new NullReferenceException();
		}
	}
}