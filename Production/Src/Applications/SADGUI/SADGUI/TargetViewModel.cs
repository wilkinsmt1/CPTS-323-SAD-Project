using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAD.Core.Data;

namespace SADGUI
{
    class TargetViewModel : ViewModelBase
    {
        private Targets m_target;

        public TargetViewModel(Targets target)
        {
            m_target = target;
        }
        public Targets Target
        {
            get { return m_target; }
        }
    }
}
