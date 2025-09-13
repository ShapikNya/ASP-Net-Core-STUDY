using Microsoft.AspNetCore.Mvc;

namespace ASP_study.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {

        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] IFormFileCollection files)
        {
            var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
            Directory.CreateDirectory(uploadPath);

            foreach (var file in files)
            {
                var path = Path.Combine(uploadPath, file.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return Ok($"Uploaded {files.Count} files");
        }

    }
}
