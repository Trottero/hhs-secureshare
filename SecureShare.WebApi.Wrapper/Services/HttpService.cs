using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;

namespace SecureShare.WebApi.Wrapper.Services
{
	public class HttpService : IHttpService
	{
		private readonly HttpClient _client;

		public HttpService()
		{
			_client = new HttpClient
			{
				BaseAddress = new Uri("http://localhost:55555/api/")
			};
		}

		public async Task<string> GetOneRequest(string extension, string id)
		{
			try
			{
				HttpResponseMessage response = await _client.GetAsync(extension + "/" + id);

				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException e)
			{
				return e.Message;
			}
		}

		public async Task<string> GetAllRequest(string extension)
		{
			try
			{
				HttpResponseMessage response = await _client.GetAsync(extension);

				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException e)
			{
				return e.Message;
			}
		}

		public async Task<string> PostRequest(string extension, Entity entity)
		{
			try
			{
				var stringContent = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _client.PostAsync(extension, stringContent);
				response.EnsureSuccessStatusCode();

				return await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException e)
			{
				return e.Message;
			}
		}

		public async Task<string> DeleteRequest(string extension, string id)
		{
			try
			{
				HttpResponseMessage response = await _client.DeleteAsync(extension + "/" + id);
				response.EnsureSuccessStatusCode();

				return await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException e)
			{
				return e.Message;
			}
		}
	}
}