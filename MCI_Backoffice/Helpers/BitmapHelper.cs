using MCIGrabberService.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CheckinPortal.BackOffice.Helpers
{
    public class BitmapHelper
    {
        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        public static BitmapImage Base64StringToBitmap(string base64String)
        {
            string functionName = "Base64StringToBitmap";
            string applicationName = "SmartKiosk";
            string description = "Checkin";
            try
            {
                byte[] byteBuffer = Convert.FromBase64String(base64String);
                using (var memory = new System.IO.MemoryStream(byteBuffer))
                {
                    memory.Position = 0;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    return bitmapImage;
                }
            }
            catch(Exception ex)
            {
                LogHelper.Instance.Error(ex,"",functionName, applicationName, description);
                return null;
            }
        }

        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
          using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }

        public static string BitmapToBase64String(Bitmap bmp, ImageFormat imageFormat)
        {
            string base64String = string.Empty;
            MemoryStream memoryStream = new MemoryStream();
            bmp.Save(memoryStream, imageFormat);
            memoryStream.Position = 0;
            byte[] byteBuffer = memoryStream.ToArray();
            memoryStream.Close();
            base64String = Convert.ToBase64String(byteBuffer);
            byteBuffer = null;
            return base64String;
        }



        public static  byte[] GetBytesFromImage(string imageFile)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(imageFile);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }
    }
}
