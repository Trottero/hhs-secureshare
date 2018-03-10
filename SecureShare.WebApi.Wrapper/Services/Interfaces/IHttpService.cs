using System;
using System.Net.Http;
using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces
{
	public interface IHttpService
	{
		Task<string> GetAllRequestAsync<T>();
		Task<string> GetOneRequestAsync<T>(string entityId);
		Task<string> PostRequestAsync<T>(Entity entity);
		Task<string> DeleteRequestAsync<T>(string entityId);
	}
}