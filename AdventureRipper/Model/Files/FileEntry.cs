using System;

namespace AdventureRipper.Model.Files
{
    class FileEntry
    {
        public FileEntry()
        {
            
        }


        public string FileName { get; set; }
        public int FileOffset { get; set; }
        public byte[] Data { get; set; }
        public static String Icon = "file.png";

    }
}
