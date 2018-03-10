using System;
using System.Net.Http;
using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces
{
	public interface IHttpService
	{
		Task<string> GetAllRequest<T>();
		Task<string> GetOneRequest<T>(Guid entityId);
		Task<string> PostRequest<T>(Entity entity);
		Task<string> DeleteRequest<T>(Guid entityId);
	}
}