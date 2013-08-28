using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdventureRipper.Model.Files;

namespace AdventureRipper.Model.Resource.LIB
{
    class LibResource : Resource
    {
        public LibResource(BinaryReader b)
            : base(b)
        {
            if (CheckHeader())
            {
                ReadFileTable();
            }
        }

        public string Header { get; private set; }

        public byte NFiles { get; private set; }

        protected override void ReadHeader()
        {
            Header = new string(this.BinaryReader.ReadChars(3));
            byte dummy = this.BinaryReader.ReadByte();
            NFiles = this.BinaryReader.ReadByte();
        }

        protected override bool CheckHeader()
        {
            if (BinaryReader.BaseStream.Length > 6)
            {
                ReadHeader();
                if (Header.Equals("LIB"))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void ReadFileTable()
        {
            this.Files = new List<FileEntry>(NFiles);
            for (int i = 0; i < NFiles; i++)
            {
                FileEntry fileEntry = new FileEntry();


                byte dummy = BinaryReader.ReadByte();
                fileEntry.FileName = new String(BinaryReader.ReadChars(12)).Replace("\0", string.Empty);
                dummy = BinaryReader.ReadByte();
                byte[] offset = BinaryReader.ReadBytes(3);
                fileEntry.FileOffset = offset[2] << 16 + offset[1] << 8 + offset[0];

                this.Files.Add(fileEntry);
            }
        }
    }
}
