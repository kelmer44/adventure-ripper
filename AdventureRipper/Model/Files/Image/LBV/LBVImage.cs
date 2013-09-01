using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventureRipper.Model.Files.Image.LBV
{
    class LBVImage: ImageFile
    {
        private BinaryReader BinaryReader { get; set; }
        private Pixel[] palette;
        private byte[] rawPixels;
        private Pixel[,] realPixels;

        private LBVImage()
        {
            Width = 320;
            Height = 200;
            Bpp = 8;
            palette = new Pixel[256];
            rawPixels = new byte[Width * Height];
            realPixels = new Pixel[Width, Height];
        }

        public LBVImage(byte[] data, String fileName, String resourceName):this()
        {
            this.FileName = fileName;
            this.FilePath = resourceName + ":" + fileName;

            BinaryReader = new BinaryReader(new MemoryStream(data));
            BinaryReader.BaseStream.Seek(20,SeekOrigin.Begin);
            ReadPalette();
            this.Width = BinaryReader.ReadUInt16();
            this.Width++;
            this.Height = BinaryReader.ReadUInt16();
            this.Height++;
            BinaryReader.BaseStream.Seek(4, SeekOrigin.Current);
            ReadPixels();
        }

        private void ReadPalette()
        {
            for (int i = 0; i < 256; i++)
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
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    realPixels[i, j] = palette[rawPixels[j * Width + i]];
                }
            }

        }

        public override Bitmap ToBitmap()
        {
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format16bppRgb555);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    int red = realPixels[i, j].R * 4; // read from array
                    int green = realPixels[i, j].G * 4; // read from array
                    int blue = realPixels[i, j].B * 4; // read from array
                    bitmap.SetPixel(i, j, Color.FromArgb(0, red, green, blue));
                }
            return bitmap;
        }


    }
}
