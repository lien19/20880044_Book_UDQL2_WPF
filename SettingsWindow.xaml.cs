using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace _20880044_Book
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtServer.Text = AppConfig.GetValue(AppConfig.Server);
            txtInstance.Text = AppConfig.GetValue(AppConfig.Instance);
            txtDatabase.Text = AppConfig.GetValue(AppConfig.Database);

        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            AppConfig.SetValue(AppConfig.Server, txtServer.Text);
            AppConfig.SetValue(AppConfig.Instance, txtInstance.Text);
            AppConfig.SetValue(AppConfig.Database, txtDatabase.Text);

            DialogResult = true;
        }

        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            string connString = AppConfig.ConnectionString()!;
            var conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                conn.Close();
                MessageBox.Show("Kết nối thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại");
            }

        }
    }
}
