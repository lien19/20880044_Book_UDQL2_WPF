using Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using _20880044_Book.ViewModel;
using _20880044_Book.Model;

namespace _20880044_Book.View
{
    /// <summary>
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Fluent.RibbonWindow
    {
        public DashboardWindow()
        {
            InitializeComponent();
            //dgBook.DataContext = new Book();
            //gdCategory.DataContext = new Commands();
            //gdCategory.ItemsSource = new List<string>()
            //{
            //    "Cell 1","Cell 2","Cell 3"
            //};
        }
        
    }
}
