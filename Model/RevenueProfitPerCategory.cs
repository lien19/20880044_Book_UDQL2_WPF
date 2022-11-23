using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20880044_Book.Model
{
    public class RevenueProfitPerCategory: INotifyPropertyChanged
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double Revenue { get; set; }
        public double Profit { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
