using Microsoft.AspNetCore.Mvc;
using YourProject.Services;

namespace YourProject.Controllers
{
    public class BlobController : Controller
    {
        private readonly BlobStorageService _blobService;

        public BlobController(BlobStorageService blobService)
        {
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            var files = await _blobService.ListFilesAsync();
            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
                await _blobService.UploadFileAsync(file);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Download(string fileName)
        {
            var stream = await _blobService.DownloadAsync(fileName);
            return File(stream, "application/octet-stream", fileName);
        }

        public async Task<IActionResult> Delete(string fileName)
        {
            await _blobService.DeleteAsync(fileName);
            return RedirectToAction("Index");
        }
    }
}