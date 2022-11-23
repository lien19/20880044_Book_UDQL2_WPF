using _20880044_Book.Model;
using Aspose.Cells;
//using DevExpress.Mvvm;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xaml.Behaviors;
using Nito.Mvvm;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using LiveCharts;

namespace _20880044_Book.ViewModel
{
    public class BookViewModel : INotifyPropertyChanged    
    {
        #region Variable
        //BindingList or ObservableCollection
        public BindingList<Book> Books { get; set; }
        public ICommand cmdCreate { get; set; }
        public ICommand cmdUpdate { get; set; }
        public ICommand cmdDelete { get; set; }
        public ICommand cmdSearch { get; set; }
        public ICommand cmdImport { get; set; }
        public ICommand cmdCancel { get; set; }
        public ICommand cmdSave { get; set; }
        public SeriesCollection a { get; set; }
        public List<string> LabelY   { get; set; }
        public Func<double, string> Formatter { get; set; }

        public ICommand cmdDoubleClick { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        //-------------------------------
        SqlConnection conn = null;
        #endregion
        //-------------------------------
        public BookViewModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //connect database
            string connString = AppConfig.ConnectionString()!;
            conn              = new SqlConnection(connString);
            conn.Open();
            Load();
            List<double> e = new List<double> { 1, 3, 29, 100 };
            ChartValues<double> q = new ChartValues<double>();
            q.Add(e[1]);
            //-------------------------------
            LabelY = new List<string>() { "jan", "feb", "mar", "apr" };
            Formatter = value => value.ToString("N");
            
            var b = new LiveCharts.SeriesCollection()
                {
                    //new LineSeries()
                    //{
                    //    Values = new ChartValues<double> { 1, 3, 29, 100 }
                    //},
                    new RowSeries()
                    {
                        Values = new ChartValues<double> {1, 3, 29, 100}
                    }
                };

            a = b;
            //cmdCreate = new RelayCommand();
            cmdUpdate = new RelayCommand(Update);
            cmdDelete = new RelayCommand(Delete);
            //cmdSearch = new RelayCommand();

            cmdImport = new RelayCommand(Import);

            //cmdCancel = new RelayCommand();
            //cmdSave   = new RelayCommand();
            cmdDoubleClick = new RelayCommand(Test);
            //conn.Close();
        }


        //===================================================================================================
        //===================================================================================================
        public void Load()
        {
            Books = new BindingList<Book>();
            
            //ExecuteReader all rows
            var sql     = "select * from Item";
            var command = new SqlCommand(sql, conn);
            var reader  = command.ExecuteReader();
            //tạo Books để binding vào màn hình
            while (reader.Read())
            {
                var item = new Book()
                {
                    //Id = id, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, CategoryId = categoryId
                    Id = (int)reader["Id"],
                    Name            = (string)reader["Name"],
                    SellPrice       = (double)reader["SellPrice"],
                    DiscountedPrice = (double)reader["DiscountedPrice"],
                    ImagePath       = (string)reader["ImagePath"],
                    BuyPrice        = (double)reader["BuyPrice"],
                    StartQtyBalance = (double)reader["StartQtyBalance"],
                    QtyIn           = (double)reader["QtyIn"],
                    QtyOut          = (double)reader["QtyOut"],
                    EndQtyBalance   = (double)reader["EndQtyBalance"],
                    CategoryId      = (int)reader["CategoryId"],
                };
                Books.Add(item);
            }
            reader.Close();
        }
        public void Import(object param)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var screen = new OpenFileDialog();
            
            if (screen.ShowDialog() == true)
            {
                var fileName = screen.FileName;
                var workbook = new Workbook(fileName);
                var tabs     = workbook.Worksheets;

                for (int i = 0; i < tabs.Count; i++)
                {
                    //Insert dữ liệu vào Bảng Product
                    var col  = "A";
                    var row  = 2;
                    var cell = tabs[i].Cells[$"{col}{row}"]; //ô đầu tiên cần import


                    while (cell.Value != null) //import từng dòng trong file excel
                    {
                        var id              = tabs[i].Cells[$"A{row}"].IntValue;
                        var name            = tabs[i].Cells[$"B{row}"].StringValue;
                        var sellPrice       = tabs[i].Cells[$"C{row}"].DoubleValue;
                        var discountedPrice = tabs[i].Cells[$"D{row}"].DoubleValue;
                        var imagePath       = tabs[i].Cells[$"E{row}"].StringValue;
                        var buyPrice        = tabs[i].Cells[$"F{row}"].DoubleValue;
                        var startQtyBalance = tabs[i].Cells[$"G{row}"].DoubleValue;
                        var qtyIn           = tabs[i].Cells[$"H{row}"].DoubleValue;
                        var qtyOut          = tabs[i].Cells[$"I{row}"].DoubleValue;
                        var endQtyBalance   = tabs[i].Cells[$"J{row}"].DoubleValue;
                        var categoryId      = tabs[i].Cells[$"K{row}"].IntValue;

                        Debug.WriteLine($"{name}-{sellPrice}");
                        //var item = new Book() { Id = id, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, CategoryId = categoryId };

                        // Chèn vào trong CSDL
                        string sql = "insert into Item(Id, Name, SellPrice, DiscountedPrice, ImagePath,BuyPrice,StartQtyBalance,QtyIn,QtyOut,EndQtyBalance, CategoryId) values(@Id, @Name, @SellPrice, @DiscountedPrice, @ImagePath,@BuyPrice,@StartQtyBalance,@QtyIn,@QtyOut,@EndQtyBalance, @CategoryId);";
                        SqlCommand command = new SqlCommand(sql, conn);
                        command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = id;
                        command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = name;
                        command.Parameters.Add("SellPrice", System.Data.SqlDbType.Float).Value = sellPrice;
                        command.Parameters.Add("DiscountedPrice", System.Data.SqlDbType.Float).Value = discountedPrice;
                        command.Parameters.Add("ImagePath", System.Data.SqlDbType.NVarChar).Value = imagePath;
                        command.Parameters.Add("BuyPrice", System.Data.SqlDbType.Float).Value = buyPrice;
                        command.Parameters.Add("StartQtyBalance", System.Data.SqlDbType.Float).Value = startQtyBalance;
                        command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value = qtyIn;
                        command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = qtyOut;
                        command.Parameters.Add("EndQtyBalance", System.Data.SqlDbType.Float).Value = endQtyBalance;
                        command.Parameters.Add("CategoryId", System.Data.SqlDbType.Int).Value = categoryId;
                        command.ExecuteNonQuery();


                        row++;
                        cell = tabs[i].Cells[$"{col}{row}"];
                    }
                }
                //================Refresh screen========================================
                Load();
                MessageBox.Show("Data imported");
            }
        }
        public void Update(object obj)
        {
            var item           = (Book)obj;
            // update vào trong CSDL
            string sql         = "update Item set Name = @Name, SellPrice=@SellPrice, DiscountedPrice=@DiscountedPrice, ImagePath=@ImagePath,BuyPrice=@BuyPrice, StartQtyBalance=@StartQtyBalance, QtyIn=@QtyIn, QtyOut=@QtyOut, EndQtyBalance=@EndQtyBalance, CategoryId=@CategoryId where Id = @Id;";
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value                = item.Id;
            command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value         = item.Name;
            command.Parameters.Add("SellPrice", System.Data.SqlDbType.Float).Value       = item.SellPrice;
            command.Parameters.Add("DiscountedPrice", System.Data.SqlDbType.Float).Value = item.DiscountedPrice;
            command.Parameters.Add("ImagePath", System.Data.SqlDbType.NVarChar).Value    = item.ImagePath;
            command.Parameters.Add("BuyPrice", System.Data.SqlDbType.Float).Value        = item.BuyPrice;
            command.Parameters.Add("StartQtyBalance", System.Data.SqlDbType.Float).Value = item.StartQtyBalance;
            command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value           = item.QtyIn;
            command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value          = item.QtyOut;
            command.Parameters.Add("EndQtyBalance", System.Data.SqlDbType.Float).Value   = item.EndQtyBalance;
            command.Parameters.Add("CategoryId", System.Data.SqlDbType.Int).Value        = item.CategoryId;
            int rowCount       = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Succeeded");
            else MessageBox.Show("Failed");
            //---------------------------------------
            Load();
        }
        public void Delete(object obj)
        {
            var item           = (Book)obj;
            // delete vào trong CSDL
            string sql         = "delete Item where Id = @Id;";
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;

            int rowCount       = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Succeeded");
            else MessageBox.Show("Failed");
            //---------------------------------------
            Load();
        }
        //public int Search(object obj)
        //{
        //    var item           = (Book)obj;
        //    var sql = string.Format("select i.Id, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath,i.BuyPrice,i.StartQtyBalance,i.QtyIn,i.QtyOut,i.EndQtyBalance, i.CategoryId from [Category] as c left join [Item] as i on c.Id = i.CategoryId" );
        //    using (var command = new SqlCommand(sql, conn))
        //    using (var reader  = command.ExecuteReader())
        //    {
        //        //tạo Book để binding vào màn hình
        //        int i = 0;
        //        //BookCategories[i].Books = new List<Book>();
        //        while (reader.Read())
        //        {
        //            var id              = (int)reader["Id"];
        //            var name            = (string)reader["Name"];
        //            var sellPrice       = (double)reader["SellPrice"];
        //            var discountedPrice = (double)reader["DiscountedPrice"];
        //            var imagePath       = (string)reader["ImagePath"];
        //            var buyPrice        = (double)reader["BuyPrice"];
        //            var startQtyBalance = (double)reader["StartQtyBalance"];
        //            var qtyIn           = (double)reader["QtyIn"];
        //            var qtyOut          = (double)reader["QtyOut"];
        //            var endQtyBalance   = (double)reader["EndQtyBalance"];
        //            var categoryId      = (int)reader["CategoryId"];
        //            var item = new Book() { Id = id, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice=buyPrice, StartQtyBalance=startQtyBalance, QtyIn=qtyIn, QtyOut=qtyOut, EndQtyBalance=endQtyBalance, CategoryId = categoryId };
        //            if (categoryId == Book[i].Id)
        //                BookCategories[i].Books.Add(item);
        //            else
        //            {
        //                i++;
        //                //BookCategories[i].Books = new List<Book>();
        //                BookCategories[i].Books.Add(item);
        //            }
        //        }
        //        //reader.Close();
        //    }
        //}
        public bool IsUsed()
        {
            //Kiểm tra Sản phẩm có dùng Loại hàng chưa
            return false;
        }

        //===================================================================================================
        //===================================================================================================











        //public SeriesCollection ad
        //{
        //    get
        //    {
        //        return new LiveCharts.SeriesCollection()
        //        {
        //            new LineSeries()
        //            {
        //                Values = new ChartValues<double> {1, 3, 29, 100}
        //            },
        //            new ColumnSeries()
        //            {
        //                Values = new ChartValues<double> {1, 3, 29, 100}
        //            }
        //        };             
        //    }
        //}













        private void InvokeRequestCloseDialog(RequestCloseDialogEventArgs e)
        {
            //var handler = RequestCloseDialog;
            //if (handler != null)
            //    handler(this, e);
        }
        public void Test(object param)
        {
            //var screen = new OpenFileDialog();
            //if (screen.ShowDialog() == true) { }

            if (param == null) return;
            //MessageBox.Show("hello");
            //MessageBox.Show(GetCellValue((DataGridCellInfo)param).ToString());
        }
        public bool canTest(object param)
        {
            return true;
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

    }
    class Load_Book : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public void Execute(object? parameter)
        {
            BookViewModel bvm = new BookViewModel();
            bvm.Books = new BindingList<Book>()
            {
                new Book(){ Id = 4, Name = "Phượng hoàng lửa", SellPrice = 50000, ImagePath = "", DiscountedPrice = 450000, CategoryId = 1 },
                new Book(){ Id = 5, Name = "Hoàng tử nhỏ", SellPrice = 30000, ImagePath = "", DiscountedPrice = 250000, CategoryId = 2 },
            };
            
        }
    }

    //-------------------------------------
    public interface IUIWindowDialogService
    {
        bool? ShowDialog(string title, object datacontext);
    }

    public class WpfUIWindowDialogService : IUIWindowDialogService
    {
        public bool? ShowDialog(string title, object datacontext)
        {
            var win = new Window();
            win.Title = title;
            win.DataContext = datacontext;

            return win.ShowDialog();
        }
    }
    public class RequestCloseDialogEventArgs : EventArgs
    {
        public bool DialogResult { get; set; }
        public RequestCloseDialogEventArgs(bool dialogresult)
        {
            this.DialogResult = dialogresult;
        }
    }

    public interface IDialogResultVMHelper
    {
        event EventHandler<RequestCloseDialogEventArgs> RequestCloseDialog;
    }



    //------------------------

    //public event EventHandler<RequestCloseDialogEventArgs> RequestCloseDialog;
    //public BindingList<Book> ResetData
    //{
    //    get
    //    {
    //        books = new BindingList<Book>()
    //        {
    //            new Book(){ Id = 4, Name = "Phượng hoàng lửa", Price = 50000, ImagePath = "", DiscountedPrice = 450000, CategoryId = 1 },
    //            new Book(){ Id = 5, Name = "Hoàng tử nhỏ", Price = 30000, ImagePath = "", DiscountedPrice = 250000, CategoryId = 2 },
    //        };
    //        return books;
    //    }
    //}

    //public ICommand cmdUpdate
    //{
    //    get 
    //    {
    //        return mUpdate ?? (mUpdate = new RelayCommand(AddItem));
    //        //if (mUpdate == null) mUpdate = new Update_Book();
    //        //return mUpdate;
    //    }
    //    set { mUpdate = value; }
    //}

}
