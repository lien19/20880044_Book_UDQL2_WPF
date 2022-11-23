using _20880044_Book.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20880044_Book
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {

        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new LoginViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string username = AppConfig.GetValue(AppConfig.Username)!;
            string password = AppConfig.GetValue(AppConfig.Password)!;

            txtUsername.Text = username;
            txtPassword.Text = password;
        }

        private void imgConfig_Click(object sender, MouseButtonEventArgs e)
        {
            var setting = new SettingsWindow();
            setting.ShowDialog();
        }
    }
}
