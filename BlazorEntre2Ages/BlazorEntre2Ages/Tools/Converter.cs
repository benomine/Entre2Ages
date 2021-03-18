using System;
using System.Drawing;
using System.IO;

namespace BlazorEntre2Ages.Tools
{
    public class Converter
    {
        private static readonly ImageConverter _converter = new();

        public static byte[] ImageToByteArray(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                return memoryStream.ToArray();
            }
        }

        public static Bitmap BitmapFromByteArray(byte[] byteArray)
        {
            Bitmap bm = (Bitmap)_converter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
                               bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f), 
                    (int)(bm.VerticalResolution + 0.5f));
            }
            return bm;
        }

        public static string ImageToStringBase64(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                var array = memoryStream.ToArray();
                var bas64 = Convert.ToBase64String(array);
                return bas64;
            }
        }

        public static Image Base64ToBitmap(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                var image = Image.FromStream(ms, true);
                return image;
            }
        }
    }
}
