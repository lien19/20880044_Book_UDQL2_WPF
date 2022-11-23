using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20880044_Book.ViewModel
{
    internal class Commands
    {
        private DelegateCommand<object> _OpenCaseCommand;
        public ICommand OpenCaseCommand
        {
            get
            {
                if (_OpenCaseCommand == null)
                {
                    _OpenCaseCommand = new DelegateCommand<object>(p =>
                    {
                        MessageBox.Show(string.Format("Parameter is {0}", p.ToString()));
                    }, p => true);
                }
                return _OpenCaseCommand;
            }
        }
    }

}
