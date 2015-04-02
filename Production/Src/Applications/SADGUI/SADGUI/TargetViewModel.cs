using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SAD.Core.Data;

namespace SADGUI
{
    public class TargetViewModel : ViewModelBase
    {
        private Targets m_Target;

        public TargetViewModel(Targets target)
        {
            m_Target = target;
            KillTargetCommand = new MyCommands(KillTarget);
        }
        private void KillTarget()
        {
            //Do some stuff....

            m_Target.IsAlive = false;
        }

        public Targets Target
        {
            get
            {
                return m_Target;
            } set { m_Target = value; }
        }

        public ICommand KillTargetCommand { get; set; }
    }

}
