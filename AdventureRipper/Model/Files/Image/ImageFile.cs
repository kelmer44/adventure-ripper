using System.Drawing;

namespace AdventureRipper.Model.Files.Image
{
    abstract class ImageFile : FileEntry
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int Bpp { get; set; }

        public abstract Bitmap ToBitmap();
    }
}
