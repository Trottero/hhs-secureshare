using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces
{
	public interface IHttpService
	{
		Task<HttpContent> GetAllRequestAsync<T>();
		Task<HttpContent> GetOneRequestAsync<T>(string entityId);
	    Task<HttpContent> PostRequestAsync<T>(IFormFile formFile);
        Task<HttpContent> PostRequestAsync<T>(Entity entity);
		Task<HttpContent> DeleteRequestAsync<T>(string entityId);
	    Task<HttpContent> GetOneRequestAsync<T>(string entityId, string extension);
    }
}