using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;

namespace sample;
class Program
{
    static void Main(string[] args)
    {
        Personne personne = new Personne("oussama", 22);
        string output = JsonConvert.SerializeObject(personne, Formatting.Indented);
        // Console.WriteLine(output);

        string srcDirSequential = "images";
        string srcDirParallel = "images_parallel";
        string dstDirParallel = @"resized\parallel";
        string dstDirSequential = @"resized\sequential";

        var imageSequentialFiles = Directory.GetFiles(srcDirSequential, "*.jpeg");
        var imageParallelFiles = Directory.GetFiles(srcDirParallel, "*.jpeg");

        // Parallel Processing
        Console.WriteLine("Starting parallel processing...");
        Stopwatch stopwatchParallel = Stopwatch.StartNew();
        Parallel.ForEach(imageParallelFiles, imagePath =>
        {
            ProcessImage(imagePath, dstDirParallel);
        });
        stopwatchParallel.Stop();
        Console.WriteLine($"Parallel processing completed in {stopwatchParallel.ElapsedMilliseconds} milliseconds");

        // Sequential Processing
        Console.WriteLine("Starting sequential processing...");
        Stopwatch stopwatchSequential = Stopwatch.StartNew();
        foreach (var imagePath in imageSequentialFiles)
        {
            ProcessImage(imagePath, dstDirSequential);
        }
        stopwatchSequential.Stop();
        Console.WriteLine($"Sequential processing completed in {stopwatchSequential.ElapsedMilliseconds} milliseconds");
    }

    static void ProcessImage(string imagePath, string dstDir)
    {
        string fileName = Path.GetFileName(imagePath);
        string outputPath = Path.Combine(dstDir, $"resized-{fileName}");

        using (Image image = Image.Load(imagePath))
        {
            image.Mutate(x => x.Resize(image.Width / 10, image.Height / 10));
            image.Save(outputPath);
        }
    }
}


class Personne {
    public string nom { get; set; }
    public int age { get; set; }

    public Personne(string nom, int age) {
        this.nom = nom;
        this.age = age;
    }

    public string Hello(bool isLowercase) {
        string msg = $"hello {nom}, you are {age}";
        if (isLowercase) {
            return msg;
        }

        return msg.ToUpper();
    }
}