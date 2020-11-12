using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace FTPApp.Models.Utilities
{
    public class Converter
    {
        public static Image Base64ToImage(string base64String)
        {
            byte[] ImageBytes = Convert.FromBase64String(base64String.Trim());
            var ms = new MemoryStream(ImageBytes, 0, ImageBytes.Length);
            ms.Write(ImageBytes, 0, ImageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}