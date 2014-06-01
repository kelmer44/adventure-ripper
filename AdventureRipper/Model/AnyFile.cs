using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureRipper.Model
{
    public abstract class AnyFile
    {
        public enum SupportedFileformats
        {
            HPF,
            LIB,

        }



        public string FilePath { get; set; }
        public string FileName { get; set; }
        public override string ToString()
        {
            return FileName;
        }
    }
}
