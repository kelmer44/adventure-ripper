using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureRipper.Model.Files.Image
{
    class Pixel
    {
        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public byte A { get; set; }

        public Pixel()
        {
            
        }

        public Pixel(byte R, byte G, byte B):this()
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
        public Pixel(byte R, byte G, byte B, byte A): this(R,G,B)
        {
            this.A = A;
        }

    }
}
