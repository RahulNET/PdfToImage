using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PdfToImageConverter.Interfaces;

namespace PdfToImageConverter
{
    public class ImageMagickBasedRasterizer : IDefineRasterization
    {
        MagickReadSettings rasterizerSettings;

        public ImageMagickBasedRasterizer(string outputBasePath, OutputImageFormat outputImageFormat = OutputImageFormat.Jpeg, bool ignoreIfTargetFileExists = false)
        {
            
            rasterizerSettings = new MagickReadSettings();
            rasterizerSettings.Density = new Density(300, 300);
            OutputBasePath = outputBasePath;
            OutputImageFormat = outputImageFormat;
            IgnoreIfTargetFileExists = ignoreIfTargetFileExists;
        }

        public string OutputBasePath { get; }
        public OutputImageFormat OutputImageFormat { get; }
        public bool IgnoreIfTargetFileExists { get; }

        public List<string> Rasterize(string pdfFileNameWithPath)
        {
            var generatedFileNamesWithPath = new List<string>();

            if (!File.Exists(pdfFileNameWithPath))
                throw new FileNotFoundException(pdfFileNameWithPath);

            if (!Directory.Exists(OutputBasePath))
                Directory.CreateDirectory(OutputBasePath);

            var pdfFileName = Path.GetFileNameWithoutExtension(pdfFileNameWithPath);


            using (var imageCollection = new MagickImageCollection())
            {
                imageCollection.Read(pdfFileNameWithPath, rasterizerSettings);

                var page = 1;
                var outputFileNameExtension = ".jpg";



                foreach (var image in imageCollection)
                {
                    switch (OutputImageFormat)
                    {
                        case OutputImageFormat.Jpeg:
                            image.Format = MagickFormat.Jpeg;
                            break;
                        case OutputImageFormat.Png:
                            image.Format = MagickFormat.Png;
                            outputFileNameExtension = ".png";
                            break;
                        case OutputImageFormat.bmp:
                            image.Format = MagickFormat.Bmp;
                            outputFileNameExtension = ".bmp";
                            break;
                        default:
                            throw new NotSupportedException($"Output image format not supported: {OutputImageFormat}");
                    }

                    var pageFileNameWithPath = Path.Combine(OutputBasePath, pdfFileName + "_" + page + outputFileNameExtension);

                    if (!IgnoreIfTargetFileExists && File.Exists(pageFileNameWithPath))
                        throw new Exception($"File {pageFileNameWithPath} already exists and argument {nameof(IgnoreIfTargetFileExists)} is set to {IgnoreIfTargetFileExists}");

                    image.Write(pageFileNameWithPath);

                    generatedFileNamesWithPath.Add(pageFileNameWithPath);

                    page++;
                }

            }

            return generatedFileNamesWithPath;
        }


    }
}
