using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20880044_Book
{
    public static class AppConfig
    {
        public static string Server   = "Server";
        public static string Instance = "Instance";
        public static string Database = "Database";
        public static string Username = "Username";
        public static string Password = "Password";
        public static string Entropy  = "Entropy";

        public static string? GetValue(string key)
        {
            string? value = ConfigurationManager.AppSettings[key];
            return value;
        }

        public static void SetValue(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings[key].Value = value;

            configFile.Save(ConfigurationSaveMode.Minimal);

            ConfigurationManager.RefreshSection("appSettings");
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static string? ConnectionString()
        {
            string? result = "";
            var builder = new SqlConnectionStringBuilder();
            string? server   = GetValue(Server);
            string? instance = GetValue(Instance);
            string? database = GetValue(Database);
            string? username = GetValue(Username);
            string? password = GetValue(Password);

            builder.DataSource = $"{server}\\{instance}";
            builder.UserID     = username;
            builder.Password   = password;
            builder.InitialCatalog     = database;
            builder.IntegratedSecurity = true;
            builder.ConnectTimeout     = 3; // s

            result = builder.ToString();
            return result;
        }



    }
}
