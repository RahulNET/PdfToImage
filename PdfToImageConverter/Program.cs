using ImageMagick;
using PdfToImageConverter.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PdfToImageConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Stopwatch watch = new Stopwatch();
            watch.Start();
            //ProcessSequentially();
            ProcessInParallel();
            watch.Stop();

            Console.WriteLine($"Total time elapsed: {watch.ElapsedMilliseconds}");
        }


        private static void ProcessInParallel()
        {
            var sourceFolderPath = @"C:\Users\RdLab\source\repos\PdfToImageConverter\PdfToImageConverter\Data\";
            var outPath = @"C:\Users\RdLab\source\repos\PdfToImageConverter\PdfToImageConverter\Output\";
            IDefineRasterization rasterizer = new ImageMagickBasedRasterizer(outPath);

            var allPdfFiles = Directory.EnumerateFiles(sourceFolderPath, "*.pdf");
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

            var result = Parallel.ForEach(allPdfFiles, options, aPdfFile => rasterizer.Rasterize(aPdfFile));
        }

        private static void ProcessSequentially()
        {
            var sourceFolderPath = @"C:\Users\RdLab\source\repos\PdfToImageConverter\PdfToImageConverter\Data\";
            var outPath = @"C:\Users\RdLab\source\repos\PdfToImageConverter\PdfToImageConverter\Output\";
            IDefineRasterization rasterizer = new ImageMagickBasedRasterizer(outPath);

            var allPdfFiles = Directory.EnumerateFiles(sourceFolderPath, "*.pdf");

            foreach (var file in allPdfFiles)
            {
                rasterizer.Rasterize(file);
            }
        }
    }
}
