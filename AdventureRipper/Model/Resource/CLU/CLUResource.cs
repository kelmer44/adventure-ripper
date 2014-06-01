using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureRipper.Model.Resource.CLU
{
    class CluResource: Resource
    {
        public UInt32 BaseId { get; set; }
        public UInt32 NumSects { get; set; }

        public CluResource() : base()
        {
        }


        protected override void ReadHeader()
        {
            throw new NotImplementedException();
        }

        protected override bool CheckHeader()
        {
            throw new NotImplementedException();
        }

        protected override void ReadFileTable()
        {
            throw new NotImplementedException();
        }

        public override byte[] GetFile(int nFile)
        {
            throw new NotImplementedException();
        }
    }
}
