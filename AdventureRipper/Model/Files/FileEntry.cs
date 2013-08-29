using System;
using System.IO;

namespace AdventureRipper.Model.Files
{
    class FileEntry : AnyFile
    {
        public FileEntry()
        {

        }

        public int ResourceIdx { get; set; }
        public int FileOffset { get; set; }
        public byte[] Data { get; set; }
        public static String Icon = "file.png";

        protected byte[] DataFromFilename(String filename)
        {
            var b = new BinaryReader(File.Open(filename, FileMode.Open));
            return b.ReadBytes(Convert.ToInt32(b.BaseStream.Length));
        }
    }
}
