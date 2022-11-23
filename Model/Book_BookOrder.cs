using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20880044_Book.Model
{
    public class Book_BookOrder: INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Book Book { get; set; } //ItemId
        public double OrderPrice { get; set; }
        public double QtyIn { get; set; }
        public double QtyOut { get; set; }
        public double UnitCost { get; set; }
        public double Revenue { get; set; }
        public double Cost { get; set; }
        public double Profit { get; set; }



        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
