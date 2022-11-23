using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20880044_Book.Model
{
    public class Book: INotifyPropertyChanged, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double SellPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public string ImagePath { get; set; }

        public double BuyPrice { get; set; }
        public double StartQtyBalance { get; set; }
        public double QtyIn { get; set; }
        public double QtyOut { get; set; }
        public double EndQtyBalance { get; set; }
        public int CategoryId { get; set; }
        public int Index { get; set; }
        public Book()
        {
            //Id = 3; Name = "Phượng hoàng lửa"; SellPrice = 60000; ImagePath = ""; DiscountedPrice = 450000; CategoryId = 1 ;
            //Id = 2; Name = "Hoàng tử nhỏ"; SellPrice = 30000; ImagePath = ""; DiscountedPrice = 250000; CategoryId = 2;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
