using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ArchiApp.NET
{
    public static class ResizeHttpTrigger
    {
        [FunctionName("ResizeHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            byte[]  targetImageBytes;

            string wStr = req.Query["w"];
            string hStr = req.Query["h"];
            
            int w;
            int h;
        

            if (string.IsNullOrEmpty(wStr) || !int.TryParse(wStr, out w)) {
                return new BadRequestObjectResult("Please provide a valid integer value for the query parameter 'w'.");
            }

            if (string.IsNullOrEmpty(hStr) || !int.TryParse(hStr, out h)) {
                return new BadRequestObjectResult("Please provide a valid integer value for the query parameter 'h'.");
            }

            using (var inputStream = new MemoryStream())
            {
                await req.Body.CopyToAsync(inputStream);
                inputStream.Position = 0;

                using (var image = Image.Load(inputStream))
                {
                    image.Mutate(x => x.Resize(w, h));

                    using (var outputStream = new MemoryStream()) 
                    {
                        image.SaveAsJpeg(outputStream);
                        targetImageBytes = outputStream.ToArray();
                    }
                }
            }

            return new FileContentResult(targetImageBytes, "image/jpeg")
            {
                FileDownloadName = "resized-image.jpeg"
            };
        }
    }
}
