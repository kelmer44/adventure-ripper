using System.Collections.Generic;
using System.IO;
using AdventureRipper.Model.Files;

namespace AdventureRipper.Model.Resource
{
    abstract class Resource : AnyFile
    {
        public string Header { get; protected set; }

        public int NFiles { get; protected set; }

        protected Resource(string fileName)
        {
            FilePath = fileName;
            FileName = Path.GetFileName(fileName);
            BinaryReader = new BinaryReader(File.Open(fileName, FileMode.Open));
        }

        public List<FileEntry> Files { get; set; }

        public BinaryReader BinaryReader { get; set; }

        protected abstract void ReadHeader();
        protected abstract bool CheckHeader();
        protected abstract void ReadFileTable();
        public abstract byte[] GetFile(int nFile);
    }
}
