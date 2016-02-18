using NReco.ImageGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ImageToASCIIconverter
{
    class Console
    {
        private static string[] _AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", "&nbsp;" };
        private static string Content;

        static void Main(string[] args)
        {
            System.Console.WriteLine("woop woop");

            args = new String[] { "c:\\test\\" };
            string path="";
            string[] files = Directory.GetFiles(args[0]);
            if (Directory.Exists(Path.GetDirectoryName(files[0]) + "\\" + "output"))
            {
                Directory.Delete(Path.GetDirectoryName(files[0]) + "\\" + "output",true);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(files[0]) + "\\"+"output");

            for (int i = 0; i < files.Length && files[i].EndsWith(".png"); i++)
            {
                System.Console.WriteLine("Starting Rendering" + files[i]);
                path = files[i];
                Bitmap imageToConvert = new Bitmap(path, true);
                

                Content = ConvertToAscii(imageToConvert);
                string text = "<div><pre style=\"background-color:#0E1517;\"><Font size=0>" + Content + "</Font></pre><div>";
                //saveToImage(text, Path.GetDirectoryName(path) + "\\output\\" + i + ".png");
                saveToImage(text, Path.GetDirectoryName(path) + "\\output\\" + Path.GetFileName(path) + ".png");
                System.Console.WriteLine("Done Rendering" + files[i]);
            }
        }
        
        private static string ConvertToAscii(Bitmap image)
        {
            bool toggle = false;
            StringBuilder sb = new StringBuilder();

            for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                    Color pixelColor = image.GetPixel(w, h);

                    //Average out the RGB components to find the Gray Color
                    string hex = ColorTranslator.ToHtml(pixelColor);

                    int red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    int blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    Color grayColor = Color.FromArgb(red, green, blue);

                    if (!toggle)
                    {
                        int index = (((grayColor.R + grayColor.G + grayColor.B) / 3) * 10 / 255);
                        string t = "<font style=\"color:" + hex + ";\">" + _AsciiChars[index] + "</font>";
                        sb.Append(t);
                    }
                }
                if (!toggle){   sb.Append("<BR>");  toggle = true;  }
                else        {   toggle = false;                     }
            }
            return sb.ToString();
        }
        static HtmlToImageConverter htmlToImageConv = new NReco.ImageGenerator.HtmlToImageConverter();

        private static void saveToImage(string html, string path)
        {
            File.WriteAllBytes(path, htmlToImageConv.GenerateImage(html, ImageFormat.Bmp));
        }
    }
}
