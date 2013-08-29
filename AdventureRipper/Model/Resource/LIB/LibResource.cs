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
        public LibResource(String filename)
            : base(filename)
        {
            if (CheckHeader())
            {
                ReadFileTable();
            }
        }



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

                fileEntry.ResourceIdx = i;
                byte dummy = BinaryReader.ReadByte();
                fileEntry.FileName = new String(BinaryReader.ReadChars(12)).Replace("\0", string.Empty);
                dummy = BinaryReader.ReadByte();
                byte[] offset = BinaryReader.ReadBytes(3);
                fileEntry.FileOffset = offset[0] + offset[1] * 256 + offset[2] * 65536;

                this.Files.Add(fileEntry);
            }
        }


        public override byte[] GetFile(int nFile)
        {
            if (nFile < this.NFiles)
            {
                FileEntry f = this.Files[nFile];
                BinaryReader.BaseStream.Seek(f.FileOffset, SeekOrigin.Begin);
                long startPos = f.FileOffset;
                long endPos = 0;
                if (nFile < this.NFiles - 1)
                {
                    endPos = this.Files[nFile + 1].FileOffset;
                }
                else
                {
                    endPos = BinaryReader.BaseStream.Length;
                }
                byte[] bytes = BinaryReader.ReadBytes(Convert.ToInt32(endPos - startPos));
                f.Data = bytes;
                return bytes;
            }
            else
            {
                return null;
            }
        }
    }
}
