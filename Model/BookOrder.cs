    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20880044_Book.Model
{
    public class BookOrder : INotifyPropertyChanged, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public BindingList<Book_BookOrder> OrderDetail { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public BookOrder()
        {
            OrderDetail = new BindingList<Book_BookOrder>();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
