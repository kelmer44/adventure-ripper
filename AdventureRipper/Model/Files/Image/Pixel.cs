using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureRipper.Model.Files.Image
{
    class Pixel
    {
        private byte _r;
        private byte _g;
        private byte _b;

        public byte R
        {
            get { return _r; }
            set { _r = value; }
        }

        public byte G
        {
            get { return _g; }
            set { _g = value; }
        }

        public byte B
        {
            get { return _b; }
            set { _b = value; }
        }
    }
}
