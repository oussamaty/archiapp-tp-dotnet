using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;

namespace ResizeParallel;
class Program
{
    public static async Task Main(string[] args)
    {
        string srcDir = @"images";
        string dstDir = @"resized";

        var imgPaths = Directory.GetFiles(srcDir, "*.jpeg");

        Console.WriteLine("Starting parallel processing...");
        Stopwatch stopwatch = Stopwatch.StartNew();
        await Task.WhenAll(imgPaths.Select(imgPath => ProcessImageAsync(imgPath, dstDir)));
        stopwatch.Stop();
        Console.WriteLine($"Parallel processing completed in {stopwatch.ElapsedMilliseconds} milliseconds");
    }

    private static async Task ProcessImageAsync(string imgPath, string dstDir)
    {
        using (Image image = await Image.LoadAsync(imgPath))
        {
            image.Mutate(x => x.Resize(image.Width / 10, image.Height / 10));

            string imgName = Path.GetFileName(imgPath);
            string outputPath = Path.Combine(dstDir, $"resized-{imgName}");

            await image.SaveAsJpegAsync(outputPath);
        }
    }
}