using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuss.Wpf.Controls
{
    public class ModuleInfo
    {
        public ModuleInfo()
        {
        }

        public string MenuName
        {
            get;
            set;
        }

        public string AssemblyFile
        {
            get;
            set;
        }

        public string ClassName
        {
            get;
            set;
        }

        public string StartMethod
        {
            get;
            set;
        }

        private List<ModuleInfo> _moduleChildren;
        public List<ModuleInfo> ModuleChildren
        {
            get
            {
                return _moduleChildren ?? (_moduleChildren = new List<ModuleInfo>());
            }
            set
            {
                _moduleChildren = value;
            }
        }
    }
}
