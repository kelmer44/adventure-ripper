using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureRipper.Model
{
    abstract class AnyFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public override string ToString()
        {
            return FileName;
        }
    }
}
