using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;

namespace ResizeSequential;
class Program
{
    public static void Main(string[] args)
    {
        string srcDir = @"images";
        string dstDir= @"resized";

        var imgPaths = Directory.GetFiles(srcDir, "*.jpeg");

        Console.WriteLine("Starting sequential processing...");
        Stopwatch stopwatch = Stopwatch.StartNew();
        foreach (var imgPath in imgPaths)
        {
            ProcessImage(imgPath, dstDir);
        }
        stopwatch.Stop();
        Console.WriteLine($"Sequential processing completed in {stopwatch.ElapsedMilliseconds} milliseconds");
    }

    private static void ProcessImage(string imgPath, string dstDir)
    {
        string imgName = Path.GetFileName(imgPath);
        string outputPath = Path.Combine(dstDir, $"resized-{imgName}");

        using (Image image = Image.Load(imgPath))
        {
            image.Mutate(x => x.Resize(image.Width / 10, image.Height / 10));
            image.Save(outputPath);
        }
    }
}