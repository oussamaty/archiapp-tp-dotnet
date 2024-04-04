using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace sample;
class Program
{
    public static void Main(string[] args)
    {
        Personne personne = new Personne("oussama", 22);
        string output = JsonConvert.SerializeObject(personne, Formatting.Indented);
        Console.WriteLine(output);

        string srcDir = @"images";
        string dstDir = @"resized";

        var imgPaths = Directory.GetFiles(srcDir, "*.jpeg");

        foreach (var imgPath in imgPaths)
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