using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shooty.Web.Models;

namespace Shooty.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["UploadId"] = Guid.NewGuid().ToString();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string UploadId)
        {
            if (file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=shooty;AccountKey=3pH6tYMXEE5A3dYsSF6IsP3S9frELrbSIeHrewW1GFuWfCXQ2IxRTGNluky3tbWhESNw0a7b3Cn3PNYiymfnuw==;EndpointSuffix=core.windows.net");
                    string containerName = UploadId;

                    var containerClient = blobServiceClient.GetBlobContainerClient(UploadId);
                    await containerClient.CreateIfNotExistsAsync();

                    BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    await blobClient.UploadAsync(memoryStream, true);
                    memoryStream.Close();
                }
            }
            return RedirectToAction("Index");
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
