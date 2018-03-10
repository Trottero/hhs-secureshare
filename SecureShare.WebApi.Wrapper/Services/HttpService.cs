using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
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

		public async Task<string> GetAllRequest<T>()
		{
			var requestUrl = CheckEntityType<T>();
			HttpResponseMessage response = await _client.GetAsync(requestUrl);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> GetOneRequest<T>(Guid entityId)
		{
			var requestUrl = CheckEntityType<T>();
			HttpResponseMessage response = await _client.GetAsync(requestUrl + entityId.ToString());
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> PostRequest<T>(Entity entity)
		{
			var requestUrl = CheckEntityType<T>();
			var stringContent = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _client.PostAsync(requestUrl, stringContent);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> DeleteRequest<T>(Guid entityId)
		{
			var requestUrl = CheckEntityType<T>();
			HttpResponseMessage response = await _client.DeleteAsync(requestUrl + entityId.ToString());
			return await response.Content.ReadAsStringAsync();
		}

		private Uri CheckEntityType<T>()
		{
	
			if (typeof(T) == typeof(User)) return new Uri(_options.UserUrl);
			if (typeof(T) == typeof(UserFile)) return new Uri(_options.FileUrl);
			return null; //todo
		}
	}
}