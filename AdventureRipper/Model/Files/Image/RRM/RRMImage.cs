using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventureRipper.Model.Files.Image.RRM
{
    class RRMImage:ImageFile
    {
        private BinaryReader BinaryReader { get; set; }
        private Pixel[] palette;
        private byte[] rawPixels;
        private Pixel[,] realPixels;

        private RRMImage()
        {
            Width = 320;
            Height = 138;
            Bpp = 8;
            palette = new Pixel[256];
            rawPixels = new byte[Width * Height];
            realPixels = new Pixel[Width, Height];
        }

        public RRMImage(byte[] data, String fileName, String resourceName):this()
        {
            this.FileName = fileName;
            this.FilePath = resourceName + ":" + fileName;
        }

        public RRMImage(String filePath):this()
        {
            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
            BinaryReader = new BinaryReader(File.Open(filePath, FileMode.Open));
            int paletteSize = 256*3;
            BinaryReader.BaseStream.Seek(-(Width * Height + paletteSize), SeekOrigin.End);
            ReadPalette();
            ReadPixels();
        }

        private void ReadPalette()
        {
            for(int i=0;i<256;i++)
            {
                palette[i] = new Pixel();
                palette[i].R = BinaryReader.ReadByte();
                palette[i].G = BinaryReader.ReadByte();
                palette[i].B = BinaryReader.ReadByte();
            }
        }

        private void ReadPixels()
        {
            rawPixels = BinaryReader.ReadBytes(rawPixels.Length);
            for (int i = 0; i < Width;i++)
            {
                for(int j = 0; j<Height; j++)
                {
                    realPixels[i,j] = palette[rawPixels[j*320 + i]];
                }
            }
        }

        public Bitmap ToBitmap()
        {
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format16bppRgb555);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {                    
                    int red = realPixels[i,j].R * 4; // read from array
                    int green = realPixels[i, j].G * 4; // read from array
                    int blue = realPixels[i, j].B * 4; // read from array
                    bitmap.SetPixel(i, j, Color.FromArgb(0, red, green, blue));
                }
            return bitmap;
        }
    }
}
