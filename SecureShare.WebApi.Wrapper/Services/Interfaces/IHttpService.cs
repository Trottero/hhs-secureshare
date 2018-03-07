using System.Threading.Tasks;
using SecureShare.WebApi.Wrapper.Models;

namespace SecureShare.WebApi.Wrapper.Services.Interfaces
{
	public interface IHttpService
	{
		Task<string> GetOneRequest(string extension, string id);

		Task<string> GetAllRequest(string extension);

		Task<string> PostRequest(string extension, Entity entity);

		Task<string> DeleteRequest(string extension, string id);
	}
}