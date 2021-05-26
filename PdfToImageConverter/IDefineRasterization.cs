using System.Collections.Generic;

namespace PdfToImageConverter.Interfaces
{
    public enum OutputImageFormat
    {
        Jpeg,
        Png,
        bmp
    }

    public interface IDefineRasterization
    {
        
        List<string> Rasterize(string pdfFileNameWithPath);
    }
}