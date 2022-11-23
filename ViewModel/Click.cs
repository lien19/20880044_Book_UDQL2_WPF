using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using Windows.UI.Xaml.Controls;

namespace _20880044_Book.ViewModel
{
    public class Click : DependencyObject
    {
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
          DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(Click), new UIPropertyMetadata(null,
           (o, e) =>
           {
               if (o is Button)
                   (o as Button).Command = e.NewValue as ICommand;
           }));

        //private static void OnCommandPropertyChanged(DependencyObject d,
        // DependencyPropertyChangedEventArgs e)
        //{
        //    var control = d as ListViewBase;
        //    if (control != null)
        //        control.ItemClick += OnItemClick;
        //}

        //private static void OnItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var control = sender as ListViewBase;
        //    var command = GetCommand(control);

        //    if (command != null && command.CanExecute(e.ClickedItem))
        //        command.Execute(e.ClickedItem);
        //}


    }
}
