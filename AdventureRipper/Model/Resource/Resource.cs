using System.Collections.Generic;
using System.IO;
using AdventureRipper.Model.Files;

namespace AdventureRipper.Model.Resource
{
    abstract class Resource
    {
        protected Resource(BinaryReader b)
        {
            BinaryReader = b;
        }

        public List<FileEntry> Files { get; set; }

        public BinaryReader BinaryReader { get; set; }

        protected abstract void ReadHeader();
        protected abstract bool CheckHeader();
        protected abstract void ReadFileTable();
    }
}
