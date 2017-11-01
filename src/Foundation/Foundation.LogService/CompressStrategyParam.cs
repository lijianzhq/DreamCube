using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DreamCube.Foundation.Log
{
    class CompressStrategyParam
    {
        public string Folder
        {
            get
            {
                return m_folder;
            }
            set
            {
                m_folder = value;
            }
        }

        public List<string> FilePatternCollection
        {
            get
            {
                return m_listFilePattern;
            }
            set
            {
                m_listFilePattern = value;
            }
        }

        private string m_folder = null;

        private List<string> m_listFilePattern = null;
    }
}
