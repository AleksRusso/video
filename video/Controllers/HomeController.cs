using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using video.Models;

namespace video.Controllers
{
    public class HomeController : Controller
    {
        public readonly ApplicationContext db;
        private readonly ILogger<HomeController> _logger;
        string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "newFolder");

        public HomeController(ILogger<HomeController> logger, ApplicationContext db)
        {
            _logger = logger;
            this.db = db;
        }
        public IActionResult UploadVideo()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadVideo(IFormFile videoFile, string str)
        {
            if (videoFile == null || videoFile.Length <= 0)
            {
                return RedirectToAction("Error");
            }

            var fileName = videoFile.FileName;
            var filePath = Path.Combine(_uploadFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            var video = new Video
            {
                Title = fileName,
                Description = str,
                UploadDate = DateTime.Now,
                FilePath = filePath,
            };

            db.Videos.Add(video);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var videos = db.Videos.ToList();
            return View(videos);
        }

        public IActionResult DownloadVideo(int id)
        {
            var video = db.Videos.Where(p => p.Id == id).FirstOrDefault();
            return PhysicalFile(video.FilePath, "video/mp4", "video.mp4");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}