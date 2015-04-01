using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SADGUI
{
    class MyCommands : ICommand
    {
        private Action m_action;

        public MyCommands(Action action)
        {
            m_action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            m_action();
        }

        public event EventHandler CanExecuteChanged;
    }
}
