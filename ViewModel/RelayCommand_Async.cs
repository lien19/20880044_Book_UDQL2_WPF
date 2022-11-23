using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _20880044_Book.ViewModel
{
    internal class RelayCommand_Async: ICommand
    {
        private readonly Func<object, Task> callback;
        private readonly Action<Exception> onException;
        private bool isExecuting;

        public bool IsExecuting
        {
            get => isExecuting;
            set
            {
                isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
        public event EventHandler CanExecuteChanged;

        public RelayCommand_Async(Func<object, Task> callback, Action<Exception> onException = null)
        {
            this.callback = callback;
            this.onException = onException;
        }

        public bool CanExecute(object parameter) => !IsExecuting;

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                await callback(parameter);
            }
            catch (Exception e)
            {
                onException?.Invoke(e);
            }

            IsExecuting = false;
        }

















        //protected readonly Func<Task> _asyncExecute;
        //protected readonly Func<bool> _canExecute;

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}
        //public RelayCommand_Async(Func<Task> execute): this(execute, null) { }
        //public RelayCommand_Async(Func<Task> asyncExecute, Func<bool> canExecute)
        //{
        //    _asyncExecute = asyncExecute;
        //    _canExecute = canExecute;
        //}
        //public bool CanExecute(object parameter)
        //{
        //    if (_canExecute == null) return true;
        //    return _canExecute();
        //}
        //public async void Execute(object parameter)
        //{
        //    await ExecuteAsync(parameter);
        //    // notify the UI that the commands can execute changed may have changed
        //    RaiseCanExecuteChanged();
        //}
        //protected virtual async Task ExecuteAsync(object parameter)
        //{
        //    await _asyncExecute();
        //}
        //public void RaiseCanExecuteChanged()
        //{
        //    CommandManager.InvalidateRequerySuggested();
        //}
    }
}
