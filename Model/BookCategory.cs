using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace _20880044_Book.Model
{
    public class BookCategory : INotifyPropertyChanged, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; }
        private bool _isReadOnly = true;
        public bool isReadOnly
        {
            get => _isReadOnly;
            set { _isReadOnly = value; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public BookCategory()
        {
            Books = new List<Book>();
        }

        public object GetCellValue(DataGridCellInfo cellInfo)
        {
            if (cellInfo != null)
            {
                var column = cellInfo.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var element = new FrameworkElement() { DataContext = cellInfo.Item };
                    BindingOperations.SetBinding(element, FrameworkElement.TagProperty, column.Binding);
                    var cellValue = element.Tag;

                    if (cellValue != null)
                        return (cellValue);
                }
            }
            return (null);


        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
