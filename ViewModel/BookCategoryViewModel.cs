using _20880044_Book.Model;
using Aspose.Cells;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace _20880044_Book.ViewModel
{
    public class BookCategoryViewModel: INotifyPropertyChanged, ICloneable
    {
        #region Variable binding
        //update Category
        public BindingList<BookCategory> BookCategories { get; set; }
        
        public BookCategory SelectedCategory { get; set; }
        private int _selectedCategoryIndex = 0;
        public int selectedCategoryIndex
        {
            get => _selectedCategoryIndex;
            set { _selectedCategoryIndex = value; }
        }
        //create Category
        public BookCategory CategoryCreate { get; set; }
        //update Book
        public BindingList<Book> Books { get; set; }
        public Book SelectedBook { get; set; }
        //create Book
        public Book BookCreate { get; set; }
        public BindingList<BookCategory> lstCategories_Cbbox { get; set; }
        public BookCategory cbSelectedCategory { get; set; }
        private int _cbCategoryIndex = 0;
        public int cbCategoryIndex
        {
            get => _cbCategoryIndex;
            set { _cbCategoryIndex = value; }
        }
        #endregion

        //-------------------------------CRUD variable
        //Visibility
        #region Variable Visibility ICommand
        public ICommand cmdCreateCategory { get; set; }
        public ICommand cmdCreateBook { get; set; }
        public ICommand cmdUpdateCategory { get; set; }
        public ICommand cmdUpdateBook{ get; set; }
        public ICommand cmdCancelCategoryEdit { get; set; }
        public ICommand cmdCancelBookEdit { get; set; }
        public ICommand cmdCancelCategoryCreate { get; set; }
        public ICommand cmdCancelBookCreate { get; set; }
        #endregion
        //CRUD
        #region Variable CRUD ICommand
        public ICommand cmdImport { get; set; }
        public ICommand cmdDeleteCategory { get; set; }
        public ICommand cmdDeleteBook { get; set; }
        public ICommand cmdSaveCategory { get; set; }
        public ICommand cmdSaveBook { get; set; }
        public ICommand cmdSaveCategory_Create { get; set; }
        public ICommand cmdSaveBook_Create { get; set; }
        public ICommand cmdExpander { get; set; }
        public ICommand cmdTest { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Variable Visibility
        private Visibility _isVisibleCategoryEdit = Visibility.Collapsed;
        public Visibility isVisibleCategoryEdit
        {
            get => _isVisibleCategoryEdit;
            set { _isVisibleCategoryEdit = value; }
        }
        private Visibility _isVisibleBookEdit = Visibility.Collapsed;
        public Visibility isVisibleBookEdit
        {
            get => _isVisibleBookEdit;
            set { _isVisibleBookEdit = value; }
        }
        private Visibility _isVisibleCategoryCreate = Visibility.Collapsed;
        public Visibility isVisibleCategoryCreate
        {
            get => _isVisibleCategoryCreate;
            set { _isVisibleCategoryCreate = value; }
        }
        private Visibility _isVisibleBookCreate = Visibility.Collapsed;
        public Visibility isVisibleBookCreate
        {
            get => _isVisibleBookCreate;
            set { _isVisibleBookCreate = value; }
        }
        private bool _isExpanded = false;
        public bool isExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; }
        }
        #endregion

        //-------------------------------SQL
        SqlConnection conn = null;

        //-------------------------------Paging variable
        #region Variable Paging/Search
        private string _keyword = String.Empty;
        public string keyword
        {
            get => _keyword;
            set { _keyword = value; }
        }
        public ICommand cmdSearch { get; set; }
        public ICommand cmdPagingPrev { get; set; }
        public ICommand cmdPagingNext { get; set; }
        public ICommand cmdSelectionChanged { get; set; }
        public ICommand cmdSelectionChanged_Category { get; set; }
        public int selectedPageIndex { get; set; }
        public BindingList<string> pages { get; set; }
        List<BookCategory> _viewModel;
        List<BookCategory> _viewModel2;
        int _rowsPerPage = 5;
        int _totalItems = 0;
        int _totalPages = 0;
        int _currentPage = 0;
        #endregion

        //-------------------------------Constructor
        public BookCategoryViewModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //connect database
            string connString = AppConfig.ConnectionString()!;
            conn = new SqlConnection(connString);
            conn.Open();
            Load();
            //-------------------------------
            //BookCategories = GetAll();            
            selectedCategoryIndex = 0;
            
            //-------------------------------visibility
            #region Visibility
            cmdUpdateCategory = new RelayCommand(UpdateCategory);
            cmdUpdateBook     = new RelayCommand(UpdateBook);
            cmdCancelCategoryEdit   = new RelayCommand(CancelCategoryEdit);
            cmdCancelBookEdit       = new RelayCommand(CancelBookEdit);
            cmdCancelCategoryCreate = new RelayCommand(CancelCategoryCreate);
            cmdCancelBookCreate     = new RelayCommand(CancelBookCreate);
            cmdCreateCategory = new RelayCommand(CreateCategory);
            cmdCreateBook     = new RelayCommand(CreateBook);
            #endregion

            //-------------------------------CRUD database
            cmdSaveCategory   = new RelayCommand(SaveCategoryEdit);
            cmdSaveBook       = new RelayCommand(SaveBookEdit);
            cmdSaveCategory_Create = new RelayCommand(SaveCategory_Create);
            cmdSaveBook_Create = new RelayCommand(SaveBook_Create);
            cmdDeleteCategory = new RelayCommand(DeleteCategory);
            cmdDeleteBook     = new RelayCommand(DeleteBook);
            cmdImport         = new RelayCommand(Import);

            cmdExpander = new RelayCommand(Expander);
            //cmdTest = new RelayCommand(Test);
        }

        //===================================================================================================
        //===================================================================================================
        public void Load()
        {            
            //----------------Category
            BookCategories = new BindingList<BookCategory>();
            lstCategories_Cbbox = getAll_Category();
            Books = GetAll_Book();
            CategoryCreate = new BookCategory();
            BookCreate = new Book();
            #region Load data
            //ExecuteReader all rows
            var sql     = "select * from [Category] order by Id";
            var command = new SqlCommand(sql, conn);
            var reader = command.ExecuteReader();
            //tạo BookCategories để binding vào màn hình
            while (reader.Read())
            {
                var id   = (int)reader["Id"];
                var name = (string)reader["Name"];
                var books = new List<Book>();
                var item = new BookCategory() { Id = id, Name = name, Books = books };
                BookCategories.Add(item);
            }
            reader.Close();

            //ExecuteReader all rows - Book table
            sql = string.Format("select i.Id, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath,i.BuyPrice,i.StartQtyBalance,i.QtyIn,i.QtyOut,i.EndQtyBalance, i.CategoryId, c.Id as cId from [Category] as c left join [Item] as i on c.Id = i.CategoryId  where i.Id <> 0 order by cId");
            using (command = new SqlCommand(sql, conn))
            using (reader  = command.ExecuteReader())   //dùng using thì ko cần reader.close()
            {
                //tạo Book để binding vào màn hình
                int i = 0;
                int indexBook=0;
                //BookCategories[i].Books = new List<Book>();
                while (reader.Read())
                {
                    var id              = (int)reader["Id"];
                    var name            = (string)reader["Name"];
                    var sellPrice       = (double)reader["SellPrice"];
                    var discountedPrice = (double)reader["DiscountedPrice"];
                    var imagePath       = (string)reader["ImagePath"];
                    var buyPrice        = (double)reader["BuyPrice"];
                    var startQtyBalance = (double)reader["StartQtyBalance"];
                    var qtyIn           = (double)reader["QtyIn"];
                    var qtyOut          = (double)reader["QtyOut"];
                    var endQtyBalance   = (double)reader["EndQtyBalance"];
                    var categoryId      = (int)reader["CategoryId"];

                    var index = indexBook++;
                    var item = new Book() { Id = id, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice=buyPrice, StartQtyBalance=startQtyBalance, QtyIn=qtyIn, QtyOut=qtyOut, EndQtyBalance=endQtyBalance, CategoryId = categoryId, Index=index };
                    if (categoryId == BookCategories[i].Id)
                        BookCategories[i].Books.Add(item);
                    else
                    {
                        i++;
                        //BookCategories[i].Books = new List<Book>();
                        BookCategories[i].Books.Add(item);
                    }
                }
                //reader.Close();
            }
            #endregion
            //-------------------------------paging / search
            #region Paging/Search
            //SelectedCategory = BookCategories[0];
            if (Books == null)
                return;
            selectedCategoryIndex = 0;
            _viewModel = new List<BookCategory>(BookCategories);
            _viewModel2 = GetAll().ToList();

            _totalItems = _viewModel[selectedCategoryIndex].Books.Count;
            _totalPages = calcTotalPages(_totalItems, _rowsPerPage);

            _currentPage = 1;
            createPagingInfo();  //thêm source vào combobox
            selectedPageIndex = 0;
            updateCurrentView();

            cmdPagingNext = new RelayCommand(btnNext_Click);
            cmdPagingPrev = new RelayCommand(btnPrevious_Click);
            cmdSelectionChanged = new RelayCommand(cbPages_SelectionChanged);
            cmdSelectionChanged_Category = new RelayCommand(cbCategory_SelectionChanged);
            cmdSearch = new RelayCommand(txtSearch_TextChanged);
            #endregion
        }
        public BindingList<BookCategory> getAll_Category()
        {
            //----------------Category
            var lst = new BindingList<BookCategory>();

            //ExecuteReader all rows
            var sql = "select * from [Category]";
            var command = new SqlCommand(sql, conn);
            var reader = command.ExecuteReader();
            //tạo BookCategories để binding vào màn hình
            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var name = (string)reader["Name"];
                var item = new BookCategory() { Id = id, Name = name };
                lst.Add(item);
            }
            reader.Close();
            return lst;
        }
        public BindingList<BookCategory> GetAll()
        {
            //----------------Category
            var lst = new BindingList<BookCategory>();

            //ExecuteReader all rows
            var sql = "select * from [Category]";
            var command = new SqlCommand(sql, conn);
            var reader = command.ExecuteReader();
            //tạo BookCategories để binding vào màn hình
            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var name = (string)reader["Name"];
                var item = new BookCategory() { Id = id, Name = name };
                lst.Add(item);
            }
            reader.Close();

            //ExecuteReader all rows - Book table
            sql = string.Format("select i.Id, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath,i.BuyPrice,i.StartQtyBalance,i.QtyIn,i.QtyOut,i.EndQtyBalance, i.CategoryId from [Category] as c left join [Item] as i on c.Id = i.CategoryId where i.Id <> 0");
            using (command = new SqlCommand(sql, conn))
            using (reader = command.ExecuteReader())   //dùng using thì ko cần reader.close()
            {
                //tạo Book để binding vào màn hình
                int i = 0;
                //BookCategories[i].Books = new List<Book>();
                while (reader.Read())
                {
                    var id = (int)reader["Id"];
                    var name = (string)reader["Name"];
                    var sellPrice = (double)reader["SellPrice"];
                    var discountedPrice = (double)reader["DiscountedPrice"];
                    var imagePath = (string)reader["ImagePath"];
                    var buyPrice = (double)reader["BuyPrice"];
                    var startQtyBalance = (double)reader["StartQtyBalance"];
                    var qtyIn = (double)reader["QtyIn"];
                    var qtyOut = (double)reader["QtyOut"];
                    var endQtyBalance = (double)reader["EndQtyBalance"];
                    var categoryId = (int)reader["CategoryId"];
                    var item = new Book() { Id = id, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice = buyPrice, StartQtyBalance = startQtyBalance, QtyIn = qtyIn, QtyOut = qtyOut, EndQtyBalance = endQtyBalance, CategoryId = categoryId };
                    if (categoryId == lst[i].Id)
                        lst[i].Books.Add(item);
                    else
                    {
                        i++;
                        //BookCategories[i].Books = new List<Book>();
                        lst[i].Books.Add(item);
                    }
                }
                //reader.Close();
            }
            return lst;
        }
        public BindingList<Book> GetAll_Book()
        {
            //----------------Category
            var lst = new BindingList<Book>();
            int i = 0;
            //ExecuteReader all rows - Book table
            var sql = string.Format("select * from [Item]");
            using (var command = new SqlCommand(sql, conn))
            using (var reader = command.ExecuteReader())   //dùng using thì ko cần reader.close()
            {
                while (reader.Read())
                {
                    var id = (int)reader["Id"];
                    var name = (string)reader["Name"];
                    var sellPrice = (double)reader["SellPrice"];
                    var discountedPrice = (double)reader["DiscountedPrice"];
                    var imagePath = (string)reader["ImagePath"];
                    var buyPrice = (double)reader["BuyPrice"];
                    var startQtyBalance = (double)reader["StartQtyBalance"];
                    var qtyIn = (double)reader["QtyIn"];
                    var qtyOut = (double)reader["QtyOut"];
                    var endQtyBalance = (double)reader["EndQtyBalance"];
                    var categoryId = (int)reader["CategoryId"];
                    var index = i;

                    var item = new Book() { Id = id, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice = buyPrice, StartQtyBalance = startQtyBalance, QtyIn = qtyIn, QtyOut = qtyOut, EndQtyBalance = endQtyBalance, CategoryId = categoryId, Index=index };
                    lst.Add(item);
                    i++;
                }
            }
            return lst;
        }

        //-------------------Visibility
        #region Visibility function
        public void UpdateCategory(object obj)
        {
            if (isVisibleCategoryEdit == Visibility.Collapsed) isVisibleCategoryEdit = Visibility.Visible;
            else isVisibleCategoryEdit = Visibility.Collapsed;
            isVisibleBookCreate = Visibility.Collapsed;
            isVisibleBookEdit = Visibility.Collapsed;
            isVisibleCategoryCreate = Visibility.Collapsed;
        }
        public void UpdateBook(object obj)
        {
            if (isVisibleBookEdit == Visibility.Collapsed) isVisibleBookEdit = Visibility.Visible;
            else isVisibleBookEdit = Visibility.Collapsed;
            isVisibleBookCreate = Visibility.Collapsed;
            isVisibleCategoryEdit = Visibility.Collapsed;
            isVisibleCategoryCreate = Visibility.Collapsed;
        }
        public void CreateCategory(object obj)
        {
            if (isVisibleCategoryCreate == Visibility.Collapsed) isVisibleCategoryCreate = Visibility.Visible;
            else isVisibleCategoryCreate = Visibility.Collapsed;
            isVisibleBookCreate = Visibility.Collapsed;
            isVisibleBookEdit = Visibility.Collapsed;
            isVisibleCategoryEdit = Visibility.Collapsed;
        }
        public void CreateBook(object obj)
        {
            if (isVisibleBookCreate == Visibility.Collapsed) isVisibleBookCreate = Visibility.Visible;
            else isVisibleBookCreate = Visibility.Collapsed;
            isVisibleBookEdit = Visibility.Collapsed;
            isVisibleCategoryEdit = Visibility.Collapsed;
            isVisibleCategoryCreate = Visibility.Collapsed;
        }
        public void CancelCategoryEdit(object obj)
        {
            if (isVisibleCategoryEdit == Visibility.Visible) isVisibleCategoryEdit = Visibility.Collapsed;
        }
        public void CancelBookEdit(object obj)
        {
            if (isVisibleBookEdit == Visibility.Visible) isVisibleBookEdit = Visibility.Collapsed;
        }
        public void CancelCategoryCreate(object obj)
        {
            if (isVisibleCategoryCreate == Visibility.Visible) isVisibleCategoryCreate = Visibility.Collapsed;
        }
        public void CancelBookCreate(object obj)
        {
            if (isVisibleBookCreate == Visibility.Visible) isVisibleBookCreate = Visibility.Collapsed;
        }
        #endregion

        //-------------------
        #region Create function
        public int findMaxId_Category()
        {
            if(BookCategories == null || BookCategories.Count == 0)
                return 0;
            int maxId;
            var sql = "select max(Id) as max from [Category]";
            var command = new SqlCommand(sql, conn);
            //command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = SelectedOrder.Id;
            maxId = (int)command.ExecuteScalar();
            return maxId;
        }
        public int findMaxId_Book()
        {
            if (Books == null || Books.Count == 0)
                return 0;
            int maxId;
            var sql = "select max(Id) as max from [Item]";
            var command = new SqlCommand(sql, conn);
            //command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = SelectedOrder.Id;
            maxId = (int)command.ExecuteScalar();
            return maxId;
        }
        public void SaveCategory_Create(object obj)
        {
            var item = CategoryCreate;
            item.Id = findMaxId_Category() + 1;
            // insert vào trong CSDL
            string sql = "insert into Category(Id, Name) values(@Id, @Name);";

            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;
            command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = item.Name;

            int rowCount = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Updated");
            else MessageBox.Show("Not Updated");
            //---------------------------------------
            Load();
        }
        public void SaveBook_Create(object obj)
        {
            var item = BookCreate;
            item.Id = findMaxId_Book() + 1;
            item.CategoryId = cbSelectedCategory.Id;

            // insert vào trong CSDL
            string sql = "insert into [Item](Id, Name, SellPrice, DiscountedPrice, ImagePath,BuyPrice,StartQtyBalance,QtyIn,QtyOut,EndQtyBalance, CategoryId) values(@Id, @Name, @SellPrice, @DiscountedPrice, @ImagePath,@BuyPrice,@StartQtyBalance,@QtyIn,@QtyOut,@EndQtyBalance, @CategoryId);";
            var command = new SqlCommand(sql, conn);
            
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;
            command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = item.Name;
            command.Parameters.Add("SellPrice", System.Data.SqlDbType.Float).Value = item.SellPrice;
            command.Parameters.Add("DiscountedPrice", System.Data.SqlDbType.Float).Value = item.DiscountedPrice;
            command.Parameters.Add("ImagePath", System.Data.SqlDbType.NVarChar).Value = item.ImagePath;
            command.Parameters.Add("BuyPrice", System.Data.SqlDbType.Float).Value = item.BuyPrice;
            command.Parameters.Add("StartQtyBalance", System.Data.SqlDbType.Float).Value = item.StartQtyBalance;
            command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value = item.QtyIn;
            command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = item.QtyOut;
            command.Parameters.Add("EndQtyBalance", System.Data.SqlDbType.Float).Value = item.EndQtyBalance;
            command.Parameters.Add("CategoryId", System.Data.SqlDbType.Int).Value = item.CategoryId;

            int rowCount = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Saved");
            else MessageBox.Show("Not Saved");
            //---------------------------------------
            Load();
        }
        #endregion

        #region Update function
        public void SaveCategoryEdit(object obj)
        {
            var item = SelectedCategory;
            //// update vào trong CSDL
            var sql     = "update Category set Name = @Name where Id = @Id;";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value        = item.Id;
            command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = item.Name;
            command.ExecuteNonQuery();

            int rowCount = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Updated");
            else MessageBox.Show("Not Updated");
            //---------------------------------------
            //Load();
        }
        public void SaveCategory2(object obj)
        {
            ////Nếu nút save trong cùng dòng selected thì dùng hàm này để lưu (datacontext)
            if (obj == null)
            {
                MessageBox.Show("Select a row to update");
                return;
            }
            var btn  = obj as Button;
            var item = (BookCategory)btn.DataContext;
            //// update vào trong CSDL
            var sql     = "update Category set Name = @Name where Id = @Id;";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;
            command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = item.Name;
            command.ExecuteNonQuery();

            int rowCount = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Updated");
            else MessageBox.Show("Not Updated");
            //---------------------------------------
            Load();
        }
        public void SaveBookEdit(object obj)
        {
            if (SelectedBook == null)
            {
                MessageBox.Show("Select a row to update");
                return;
            }            
            var item = SelectedBook;
            // update vào trong CSDL
            var sql = "update Item set Name = @Name, SellPrice=@SellPrice, DiscountedPrice=@DiscountedPrice, ImagePath=@ImagePath,BuyPrice=@BuyPrice, StartQtyBalance=@StartQtyBalance, QtyIn=@QtyIn, QtyOut=@QtyOut, EndQtyBalance=@EndQtyBalance, CategoryId=@CategoryId where Id = @Id;";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;
            command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = item.Name;
            command.Parameters.Add("SellPrice", System.Data.SqlDbType.Float).Value = item.SellPrice;
            command.Parameters.Add("DiscountedPrice", System.Data.SqlDbType.Float).Value = item.DiscountedPrice;
            command.Parameters.Add("ImagePath", System.Data.SqlDbType.NVarChar).Value = item.ImagePath;
            command.Parameters.Add("BuyPrice", System.Data.SqlDbType.Float).Value = item.BuyPrice;
            command.Parameters.Add("StartQtyBalance", System.Data.SqlDbType.Float).Value = item.StartQtyBalance;
            command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value = item.QtyIn;
            command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = item.QtyOut;
            command.Parameters.Add("EndQtyBalance", System.Data.SqlDbType.Float).Value = item.EndQtyBalance;
            command.Parameters.Add("CategoryId", System.Data.SqlDbType.Int).Value = item.CategoryId;
            int rowCount = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Saved");
            else MessageBox.Show("Not Saved");
            //---------------------------------------
            //Load();
        }
        public void SaveBook2(object obj)
        {
            if (obj == null)
            {
                MessageBox.Show("Select a row to update");
                return;
            }
            var btn  = obj as Button;
            var item = (Book)btn.DataContext;
            // update vào trong CSDL
            var sql  = "update Item set Name = @Name, SellPrice=@SellPrice, DiscountedPrice=@DiscountedPrice, ImagePath=@ImagePath,BuyPrice=@BuyPrice, StartQtyBalance=@StartQtyBalance, QtyIn=@QtyIn, QtyOut=@QtyOut, EndQtyBalance=@EndQtyBalance, CategoryId=@CategoryId where Id = @Id;";
            var command = new SqlCommand(sql, conn);
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
            int rowCount = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Succeeded");
            else MessageBox.Show("Failed");
            //---------------------------------------
            Load();
        }
        #endregion

        #region Delete function
        public void DeleteCategory(object obj)
        {
            var item = SelectedCategory;
            if (IsUsed(item))
            {
                MessageBox.Show("Not deleted. Category has been used.");
                return;
            }
            // Chèn vào trong CSDL
            string sql  = "delete Category where Id = @Id;";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;

            int rowCount       = command.ExecuteNonQuery();
            if (rowCount == 1) MessageBox.Show("Deleted");
            else MessageBox.Show("Not Deleted");
            //---------------------------------------
            Load();
        }
        public void DeleteBook(object obj)
        {
            var item = SelectedBook;
            if(item == null)
            {
                MessageBox.Show("Please select an item to delete");
                return;
            }
            if (IsUsed(item))
            {
                MessageBox.Show("Not deleted. Book has been used.");
                return;
            }
            // delete vào trong CSDL
            string sql = "delete Item where Id = @Id;";
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;

            int rowCount = command.ExecuteNonQuery();
            if (rowCount != 0) MessageBox.Show("Deleted");
            else MessageBox.Show("Not Deleted");
            //---------------------------------------
            Load();
        }
        public bool IsUsed(BookCategory cat)
        {
            //Kiểm tra Sản phẩm có dùng Loại hàng chưa
            if (cat.Books.Count > 0)
                return true;
            return false;
        }
        public bool IsUsed(Book item)
        {
            var sql     = "select * from [Item_Order] WHERE ItemId = @ItemId";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("ItemId", System.Data.SqlDbType.Int).Value = item.Id;
            var reader  = command.ExecuteReader();

            if(reader.Read())
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }
        #endregion

        #region Import function
        public void Import(object param)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                var fileName = screen.FileName;
                var workbook = new Workbook(fileName);
                var tabs = workbook.Worksheets;

                for (int i = 0; i < tabs.Count; i++)
                {
                    //Insert dữ liệu vào Bảng Product
                    var col = "A";
                    var row = 2;
                    var cell = tabs[i].Cells[$"{col}{row}"]; //ô đầu tiên cần import


                    while (cell.Value != null) //import từng dòng trong file excel
                    {
                        var id = tabs[i].Cells[$"A{row}"].IntValue;
                        var name = tabs[i].Cells[$"B{row}"].StringValue;

                        Debug.WriteLine($"{name}-");
                        var item = new BookCategory() { Id = id, Name = name };

                        // Chèn vào trong CSDL
                        string sql = "insert into Category(Id, Name) values(@Id, @Name);";
                        SqlCommand command = new SqlCommand(sql, conn);
                        command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = id;
                        command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = name;
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
        #endregion

        //===================================================================================================
        //===================================================================================================Paging
        #region Paging functions
        //Mỗi trang hiển thị 5 items: rowsPerPage = 5
        //Có tổng cộng 22 items     : totalItems  = 22
        //==> Tổng số trang là      : totalPages  = totalItems / rowsPerPage + (totalItems % rowsPerPage == 0 ? 0 : 1)
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Trang 2: Bỏ qua 1 trang, 1 * 5
        //Trang 3: Bỏ qua 2 trang, 2 * 5
        //Trang 4: Bỏ qua 3 trang, 3 * 5
        //Trang i: Bỏ qua i-1 trang, (i-1) * 5
        //Ds.Skip (i-1) * rowsPerPage
        //  .Take (rowsPerPage)
        private void createPagingInfo()
        {
            //Tạo ra danh sách các trang cho comboBox: vd 1/5, 2/5, 3/5, 4/5, 5/5
            pages = new BindingList<string>();
            for (int i = 1; i <= _totalPages; i++)
                pages.Add($"{i}/{_totalPages}");
        }
        private int calcTotalPages(int totalItems, int rowsPerPage)
        {
            return totalItems / rowsPerPage +
                (totalItems % rowsPerPage == 0 ? 0 : 1);
        }
        private void updateCurrentView()
        {//BINDING
            var view = new List<Book>();
            view = _viewModel[selectedCategoryIndex].Books
                .Skip((_currentPage - 1) * _rowsPerPage)
                .Take(_rowsPerPage).ToList();

            Books = new BindingList<Book>(view);  ///Vì dòng này nên phải dùng thêm biến _viewModel
        }
        private void cbCategory_SelectionChanged(object sender)
        {
            if (selectedCategoryIndex == -1)
                return;

            _viewModel = new List<BookCategory>(BookCategories);
            _totalItems = _viewModel[selectedCategoryIndex].Books.Count;
            _totalPages = calcTotalPages(_totalItems, _rowsPerPage);

            _currentPage = 1;

            createPagingInfo();  //thêm source vào combobox
            selectedPageIndex = 0;
            updateCurrentView();
        }
        private void cbPages_SelectionChanged(object sender)
        {
            _currentPage = selectedPageIndex + 1;
            
            updateCurrentView();
        }
        private void btnNext_Click(object sender)
        {
            if (selectedPageIndex != _totalPages - 1)
            {
                _currentPage = (selectedPageIndex + 1) + 1;
                selectedPageIndex = _currentPage - 1;
                                
                updateCurrentView();
            }
        }
        private void btnPrevious_Click(object sender)
        {
            if (selectedPageIndex != 0)
            {
                _currentPage = (selectedPageIndex + 1) - 1;
                selectedPageIndex = _currentPage - 1;

                updateCurrentView();
            }
        }
        #endregion

        #region Search function
        private void txtSearch_TextChanged(object sender)
        {
            _viewModel[selectedCategoryIndex].Books = new List<Book>(_viewModel2[selectedCategoryIndex].Books.ToList());
            var view = new List<Book>();
            if (BookCategories.Count > 0)
                view = _viewModel[selectedCategoryIndex].Books.Where(y => y.Name.ToUpper().Contains(keyword.ToUpper())).ToList();

            _viewModel[selectedCategoryIndex].Books = new List<Book>(view);

            _totalItems = _viewModel[selectedCategoryIndex].Books.Count;
            _totalPages = calcTotalPages(_totalItems, _rowsPerPage);
            _currentPage = 1;

            createPagingInfo();  //thêm source vào combobox
            selectedPageIndex = 0;
            updateCurrentView();

            //view = view
            //    .Skip((_currentPage - 1) * _rowsPerPage)
            //    .Take(_rowsPerPage).ToList();

            //Books = new BindingList<Book>(view);
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion

        //===================================================================================================
        //===================================================================================================










        //=============Bỏ
        public void Expander(object param)
        {
            if (param is Expander expander)
            {
                var row = DataGridRow.GetRowContainingElement(expander);
                if (expander.IsExpanded)
                {
                    row.DetailsVisibility = Visibility.Collapsed;
                    isExpanded = false;
                }
                else
                {
                    row.DetailsVisibility = Visibility.Visible;
                    isExpanded = true;
                }
            }
        }
        public void button(object param)
        {
            var btn = param as Button;
            var drv = (BookCategory)btn.DataContext;
            MessageBox.Show(drv.Id.ToString());

            //var dt  = DateTime.Now;
            //drv["Id"] = 1;
        }
        public void click_SelectedItem(object sender)
        {
            var cat     = (BookCategory)sender;
            var sql     = string.Format("select * from [Category] WHERE Id = @ID");
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = cat.Id;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var item = new BookCategory()
                    {
                        //Id = id, Name = name, Price = price, DiscountedPrice = discountedPrice, ImagePath = imagePath, CategoryId = categoryId
                        Id   = (int)reader["Id"],
                        Name = (string)reader["Name"],
                    };
                    SelectedCategory = item;
                    MessageBox.Show(SelectedCategory.Name.ToString());
                }
            }
        }
        public void Test(object param)
        {
            if (param == null) return;
            MessageBox.Show(GetCellValue((DataGridCellInfo)param).ToString());
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

       
        //private void OnGroupChange(object sender)
        //{
        //    var a = (DataGridCellInfo) sender;

        //    DataGridRow row = null;
        //    for (var visible = (Visual)sender; visible != null; visible = VisualTreeHelper.GetParent(visible) as Visual)
        //    {
        //        if (visible.GetType() != typeof(DataGridRow))
        //            continue;

        //        row = (DataGridRow)visible;
        //        var appName = (ExtenedApplicationFile)row.Item;
        //        ((ApplicationsViewModel)DataContext).SelectedApplicationFile = appName;

        //        break;
        //    }

        //    if (row != null)
        //    {
        //        Visibility currentVisibility = row.DetailsVisibility;
        //        CollapseGroupDetails();
        //        row.DetailsVisibility = currentVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        //    }
        //    else
        //    {
        //        CollapseGroupDetails();
        //    }

        //    Applications.UpdateLayout();
        //}
        //private void CollapseGroupDetails(DataGrid dg)
        //{
        //    foreach (object item in dg.ItemsSource)
        //    {
        //        if (!(dg.ItemContainerGenerator.ContainerFromItem(item) is DataGridRow row) || row.DetailsVisibility != Visibility.Visible)
        //            continue;

        //        row.DetailsVisibility = Visibility.Collapsed;
        //        break;
        //    }
        //}








        //public List<BookCategory> BookCategories
        //{
        //    get
        //    {
        //        bookCategories = new List<BookCategory>()
        //        {
        //            new BookCategory(){ Id = 1, Name = "Tiểu thuyết" },
        //            new BookCategory(){ Id = 2, Name = "Truyện thiếu nhi" },
        //        };
        //        return bookCategories;
        //    }
        //    set { bookCategories = value; }
        //}




    }

}
