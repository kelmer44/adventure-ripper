using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdventureRipper.Model.Files;

namespace AdventureRipper.Model.Resource.LIB
{
    /**
    * Sherlock Holmes Rose tatoo
    */
    class Lib2Resource: Resource
    {
        private int _firstData;

        public Lib2Resource(String filename)
            : base(filename)
        {
            if (CheckHeader())
            {
                ReadFileTable();
            }
        }



        protected override void ReadHeader()
        {
            Header = new string(this.BinaryReader.ReadChars(4));
            NFiles = this.BinaryReader.ReadInt16();
            Int32 dummy = this.BinaryReader.ReadInt32();
            _firstData = this.BinaryReader.ReadInt32();
        }

        protected override bool CheckHeader()
        {
            if (BinaryReader.BaseStream.Length > 14)
            {
                ReadHeader();
                if (Header.StartsWith("LIC"))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void ReadFileTable()
        {
            this.Files = new List<FileEntry>(NFiles);
            int listOffset = _firstData - (17 + 8)*(NFiles + 1);
            
            for (int i = 0; i < NFiles; i++)
            {
                FileEntry thisEntry = new FileEntry();
                BinaryReader.BaseStream.Seek(listOffset + i *(4+1+12), SeekOrigin.Begin);

                thisEntry.ResourceIdx = i;
                thisEntry.FileName = new String(BinaryReader.ReadChars(12)).Replace("\0", string.Empty);
                byte dummy = BinaryReader.ReadByte();
                thisEntry.FileOffset = BinaryReader.ReadInt32();
                this.Files.Add(thisEntry);
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
