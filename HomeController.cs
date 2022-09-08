using System.Web.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace UploadFileMvc.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment Environment;

        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return (IActionResult)View();
        }

        [HttpPost]
        public IActionResult Index(List<IFormFile> postedFiles)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }

            return (IActionResult)View();
        }
    }
}