using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventureRipper.Model.Files.Image.VGS
{
    class VGSImage:ImageFile
    {
        private Boolean paletteProvided = false;
        private Pixel[] palette;

        private BinaryReader BinaryReader { get; set; }
        private BinaryReader PaletteReader { get; set; }

        private List<Pixel[,]> images; 
        private VGSImage()
        {
            palette = new Pixel[256];
        }

        public VGSImage(String filePath)
            : this()
        {
            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
            BinaryReader = new BinaryReader(File.Open(filePath, FileMode.Open));

            ReadPalette();
            ReadPixels();
        }

        private void ReadPalette()
        {
            PaletteReader = new BinaryReader(File.Open("D:\\ScummVM\\Sherlock Holmes\\HOLMES2\\Util\\DARTBD.PAL", FileMode.Open));
            for (int i = 0; i < 256; i++)
            {
                palette[i] = new Pixel();
                palette[i].R = PaletteReader.ReadByte();
                palette[i].G = PaletteReader.ReadByte();
                palette[i].B = PaletteReader.ReadByte();
            }
        }

        private void ReadPixels()
        {
            this.Width = BinaryReader.ReadUInt16();
            this.Height = BinaryReader.ReadUInt16();
            this.Width++;
            this.Height++;
            //jump over 4 unknown bytes
            this.BinaryReader.BaseStream.Seek(4, SeekOrigin.Current);
            int nImages = 0;
            if(BinaryReader.BaseStream.Length > Width*Height+8)
            {
                BinaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                Console.Out.WriteLine("Assuming multiple images of size in file");
                while(BinaryReader.BaseStream.Length>BinaryReader.BaseStream.Position)
                {
                    BinaryReader.BaseStream.Seek(8, SeekOrigin.Current);
                    UInt16 imageSize = BinaryReader.ReadUInt16();
                    BinaryReader.BaseStream.Seek(imageSize - 10, SeekOrigin.Current);
                    nImages++;
                }
                Console.WriteLine("Num of images: " + nImages);
                BinaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                BinaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                Console.WriteLine("Assuming single image file");
                nImages = 1;
            }
            images = new List<Pixel[,]>(nImages);
            if (paletteProvided)
            {
                Console.Out.WriteLine("Using palette file");
            }
            else
            {
                Console.Out.WriteLine("Using grayscale palette");    
            }
            

            for(int i=0;i<nImages;i++)
            {
                Console.Out.WriteLine("Processing image #" + i);
                Pixel[,] currentImage = new Pixel[Width,Height];
                BinaryReader.BaseStream.Seek(8, SeekOrigin.Current);


                UInt16 imageSize=0;
                byte transColor = 0;
                byte runLength = 0;
                byte toReplace = 0;
                byte b = 0;

                if(nImages>1)
                {
                    imageSize = BinaryReader.ReadUInt16();
                     transColor = BinaryReader.ReadByte();
                }



                for(int y=0; y<Height;y++)
                {
                    runLength = 0;
                    toReplace = 0;
                    for(int x = 0; x<Width;x++)
                    {
                        if(nImages > 1 && runLength ==0)
                        {
                            toReplace = BinaryReader.ReadByte();
                            b = transColor;
                            runLength = BinaryReader.ReadByte();
                        }

                        if(toReplace>0)
                        {
                            toReplace--;
                        }
                        else
                        {
                            b = BinaryReader.ReadByte();
                            if(nImages>1)
                            {
                                runLength--;
                            }
                        }

                        if(paletteProvided /*Has palette*/)
                        {
                            Console.Out.WriteLine("palette");
                        }
                        else
                        {
                            currentImage[x, y + (i)] = new Pixel(); ;
                            currentImage[x, y + (i)].R = palette[b].R;
                            currentImage[x, y + (i)].G = palette[b].G;
                            currentImage[x, y + (i)].B = palette[b].B;
                        }


                    }

                }
                images.Add(currentImage);

            }

        }
        
        public Bitmap ToBitmap()
        {
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format16bppRgb555);
            Pixel[,] realPixels = images[0];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {                    
                    int red = realPixels[i,j].R*4; // read from array
                    int green = realPixels[i, j].G * 4; // read from array
                    int blue = realPixels[i, j].B * 4; // read from array
                    bitmap.SetPixel(i, j, Color.FromArgb(0, red, green, blue));
                }
            return bitmap;
        }
    }
}
