using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataBindingList
{
    internal class NumberToCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double number = (double)value;

            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            string currencyString = string.Format(info, "{0:c}", number);
            return currencyString;  // 7812000 => 7.812.000 đ
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
