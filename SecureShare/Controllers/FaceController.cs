using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SecureShare.Website.ViewModels;

namespace SecureShare.Website.Controllers
{
    public class FaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authentication(FaceData fd)
        {
            FileReader fileReader = new FileReader();
            Task task = fileReader.resultAsync(fd.pathToDir, fd.pathToUser, fd.userName);
            
            while (true)
            {
                if (task.IsCompleted)
                {
                    ViewData["Result"] = fileReader.PersonVerifyResult;
                    return View();
                }

                Task.Delay(1000);
            }
        }

        public bool AuthenticationBool(FaceData fd)
        {
            FileReader fileReader = new FileReader();
            Task task = fileReader.resultAsync(fd.pathToDir, fd.pathToUser, fd.userName);

            while (true)
            {
                if (task.IsCompleted)
                {
                    return fileReader.isIdentical;
                }
                Task.Delay(1000);
            }
        }
    }
}