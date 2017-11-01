using System;
using System.IO;
using System.Collections.Generic;

namespace DreamCube.Foundation.LogService
{
    class FileCreateTimeComparer : IComparer<string>
    {
        public int Compare(string fileName1,
                            string fileName2)
        {
            FileInfo info1 = new FileInfo(fileName1);
            FileInfo info2 = new FileInfo(fileName2);
            int result = info1.CreationTime.CompareTo(info2.CreationTime);
            info1 = null;
            info2 = null;
            return result;
        }
    }
}
