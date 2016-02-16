using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;
using System.Drawing.Imaging;

//************Application to Convert standard Images into ASCII **************//
//****************By: Thinathayalan Ganesan **********************************//
//**************Blog: http://CyberSannyasi.blogspot.com **********************//

namespace ImageToASCIIconverter
{
    public partial class Form1 : Form
    {
        TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel htmlPanel;
        public Form1()
        {

            InitializeComponent();

            htmlPanel = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
            
            htmlPanel.Dock = DockStyle.Fill;
            groupBox1.Controls.Add(htmlPanel);
            //Controls.Add(htmlPanel);
        }

        private string[] _AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", "&nbsp;" };
        //private string[] _AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", "#", "#" };
        private string _Content;

        
        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void btnConvertToAscii_Click(object sender, EventArgs e)
        {
            btnConvertToAscii.Enabled = false;
            //Load the Image from the specified path
            Bitmap image = new Bitmap(txtPath.Text, true);
            //Resize the image...
            //I've used a trackBar to emulate Zoom In / Zoom Out feature
            //This value sets the WIDTH, number of characters, of the text image
            image = GetReSizedImage(image, this.trackBar.Value);

            //Convert the resized image into ASCII
            _Content = ConvertToAscii(image);

            //Enclose the final string between <pre> tags to preserve its formatting
            //and load it in the browser control
            string text = "<div><pre style=\"background-color:#0E1517;\">" + "<Font size=0>" + _Content + "</Font></pre><div>";

            browserMain.DocumentText = text;
            htmlPanel.Text = text;
            File.WriteAllText("c:\\test\\test.html", text);
            
            saveToImage(Text);

            btnConvertToAscii.Enabled = true;
        }



        private string ConvertToAscii(Bitmap image)
        {
            Boolean toggle = false;
            StringBuilder sb = new StringBuilder();
            bool test = false;

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
                if (!toggle)
                {
                    sb.Append("<BR>");
                    toggle = true;
                }
                else
                {
                toggle = false;
            }
        }
            return sb.ToString();
        }
        private Bitmap GetReSizedImage(Bitmap inputBitmap, int asciiWidth)
        {
            int asciiHeight = 0;
            //Calculate the new Height of the image from its width
            asciiHeight = (int)Math.Ceiling((double)inputBitmap.Height * asciiWidth / inputBitmap.Width);

            //Create a new Bitmap and define its resolution
            Bitmap result = new Bitmap(asciiWidth, asciiHeight);
            Graphics g = Graphics.FromImage((Image)result);
            //The interpolation mode produces high quality images 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(inputBitmap, 0, 0, asciiWidth, asciiHeight);
            g.Dispose();
            return result;
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult diag = openFileDialog1.ShowDialog();
            if (diag == DialogResult.OK)
            {
                txtPath.Text = openFileDialog1.FileName;
            }
        }


        //private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    saveFileDialog1.Filter = "Text File (*.txt)|.txt|HTML (*.htm)|.htm";
        //    DialogResult diag = saveFileDialog1.ShowDialog();
        //    if (diag == DialogResult.OK)
        //    {
        //        if (saveFileDialog1.FilterIndex == 1)
        //        {
        //            //If the format to be saved is HTML
        //            //Replace all HTML spaces to standard spaces
        //            //and all linebreaks to CarriageReturn, LineFeed
        //            _Content = _Content.Replace("&nbsp;", " ").Replace("<BR>", "\r\n");
        //        }
        //        else
        //        {
        //            //use <pre></pre> tag to preserve formatting when viewing it in browser
        //            _Content = "<pre>" + _Content + "</pre>";
        //        }
        //        StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
        //        sw.Write(_Content);
        //        sw.Flush();
        //        sw.Close();
        //    }
        //}
        private void saveToImage(string html) {
                
                Image image = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImage(html);            
                image.Save("c:\\test\\image.png", ImageFormat.Png);
                Console.WriteLine("image saved");
            }

    }
}