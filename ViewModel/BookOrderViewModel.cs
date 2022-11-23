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
//using System.Windows.Forms;
using System.Windows.Input;

namespace _20880044_Book.ViewModel
{
    public class BookOrderViewModel : INotifyPropertyChanged
    {
        #region Binding Variable
        public BindingList<BookOrder> BookOrders { get; set; }
        public BindingList<Book> Books { get; set; }
        public BindingList<Book_BookOrder> OrderDetails { get; set; }
        //----create new order
        public BookOrder OrderCreate { get; set; }
        public Book_BookOrder SelectedOrderDetail_Create { get; set; }
        public int selectedDetailCreateIndex { get; set; }
        private DateTime _selectedOrderDate = DateTime.Today;
        public DateTime selectedOrderDate
        {
            get { return _selectedOrderDate; }
            set { _selectedOrderDate = value; }
        }
        //combobox
        public Book SelectedBook_Create{ get; set; }
        public int selectedBookCreateIndex { get; set; }

        //-----update order: combobox
        public BookOrder SelectedOrder { get; set; }
        public Book_BookOrder SelectedOrderDetail { get; set; }
        //combobox
        public Book SelectedBook_Update { get; set; }
        public int selectedBookUpdateIndex { get; set; }
        #endregion

        //---
        BookCategoryViewModel vmBookCategory = new BookCategoryViewModel();

        //--------------Visibility
        #region Variable ICommand Visibility
        public ICommand cmdVisibility_Create { get; set; }
        public ICommand cmdVisibility_Update { get; set; }
        public ICommand cmdVisibility_CancelCreate { get; set; }
        public ICommand cmdVisibility_CancelUpdate { get; set; }
        #endregion

        //--------------CRUD
        #region Variable ICommand CRUD
        public ICommand cmdImport { get; set; }
        //update
        public ICommand cmdDeleteRow { get; set; }
        public ICommand cmdAddRow { get; set; }
        public ICommand cmdUpdate { get; set; }
        public ICommand cmdSelectionChanged_Books { get; set; }
        //create
        public ICommand cmdDeleteRow_Create { get; set; }
        public ICommand cmdAddRow_Create { get; set; }
        public ICommand cmdCreate { get; set; }
        public ICommand cmdSelectionChanged_Books_Create { get; set; }
        //delete
        public ICommand cmdDeleteOrder { get; set; }
        public ICommand cmdDeleteOrderDetail { get; set; }
        
        public ICommand cmdSaveOrder { get; set; }
        public ICommand cmdSaveOrderDetail { get; set; }
        public ICommand cmdExpander { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        //--------------DataGrid - format
        #region Variable Visibility
        private Visibility _isVisibleCreate = Visibility.Collapsed;
        public Visibility isVisibleCreate
        {
            get => _isVisibleCreate;
            set { _isVisibleCreate = value; }
        }
        private Visibility _isVisibleUpdate = Visibility.Collapsed;
        public Visibility isVisibleUpdate
        {
            get => _isVisibleUpdate;
            set { _isVisibleUpdate = value; }
        }
        private Visibility _isVisibleOrderEdit = Visibility.Collapsed;
        public Visibility isVisibleOrderEdit
        {
            get => _isVisibleOrderEdit;
            set { _isVisibleOrderEdit = value; }
        }
        private Visibility _isVisibleOrderDetailEdit = Visibility.Collapsed;
        public Visibility isVisibleOrderDetailEdit
        {
            get => _isVisibleOrderDetailEdit;
            set { _isVisibleOrderDetailEdit = value; }
        }

        private bool _isExpanded = false;
        public bool isExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; }
        }
        #endregion

        //--------------
        SqlConnection conn = null;
        
        //--------------Paging/Search
        #region Variable Paging
        public DateTime start_SellQtyPerItem { get; set; }
        public DateTime end_SellQtyPerItem { get; set; }
        public ICommand cmdSearch { get; set; }
        public ICommand cmdPagingPrev { get; set; }
        public ICommand cmdPagingNext { get; set; }
        public ICommand cmdSelectionChanged { get; set; }
        public int selectedPageIndex { get; set; }
        public BindingList<string> pages { get; set; }
        List<BookOrder> _viewModel;
        List<BookOrder> _viewModel2;
        int _rowsPerPage = 5;
        int _totalItems = 0;
        int _totalPages = 0;
        int _currentPage = 0;
        private string _keyword = String.Empty;
        public string keyword
        {
            get => _keyword;
            set { _keyword = value; }
        }
        #endregion
        //-------------------------------
        public BookOrderViewModel()
        {
            //SelectedBook_Update = new Book();
            //---------
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //connect database
            string connString = AppConfig.ConnectionString()!;
            conn = new SqlConnection(connString);
            conn.Open();
            Load();            
            //-------------------------------
            
            //-------------------------------visibility   
            cmdVisibility_Create = new RelayCommand(Visibility_Create);
            cmdVisibility_Update = new RelayCommand(Visibility_Update);
            cmdVisibility_CancelCreate = new RelayCommand(Visibility_CancelCreate);
            cmdVisibility_CancelUpdate = new RelayCommand(Visibility_CancelUpdate);
            //-------------------------------CRUD database
            //----Update
            cmdUpdate    = new RelayCommand(btnSave_OrderUpdate);
            cmdDeleteRow = new RelayCommand(DeleteRow_Update);
            cmdAddRow    = new RelayCommand(AddRow_update);
            cmdSelectionChanged_Books = new RelayCommand(cbSelectionChanged_Books);
            //----Create
            cmdCreate           = new RelayCommand(btnSave_OrderCreate);
            cmdDeleteRow_Create = new RelayCommand(DeleteRow_Create);
            cmdAddRow_Create    = new RelayCommand(AddRow_Create);
            cmdSelectionChanged_Books_Create = new RelayCommand(cbSelectionChanged_Books_Create);
            //-----Delete
            cmdDeleteOrder = new RelayCommand(DeleteOrder);
            //-----Import
            cmdImport = new RelayCommand(Import);

            //cmdExpander = new RelayCommand(Expander);
        }
        public void Load()
        {
            #region Load Orders
            BookOrders = new BindingList<BookOrder>();
            Books = vmBookCategory.GetAll_Book();
            OrderDetails = getAll_OrderDetails();
            OrderCreate = new BookOrder();

            //================ExecuteReader all rows - Order table
            var sql     = string.Format("select * from [Order] order by Id");
            var command = new SqlCommand(sql, conn);
            var reader  = command.ExecuteReader();

            //tạo BookOrders để binding vào màn hình
            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var name = (string)reader["Name"];
                var orderDate = (DateTime)reader["OrderDate"];
                var customerId = (string)reader["CustomerId"];

                var item = new BookOrder() { Id = id, Name = name, OrderDate = orderDate, CustomerId = customerId };
                BookOrders.Add(item);
            }
            reader.Close();
            //================ExecuteReader all rows - Book_BookOrder table
            foreach (var item in BookOrders)
            {
                sql = string.Format("SELECT io.Id, io.OrderId, io.OrderPrice, io.QtyIn, io.QtyOut, io.UnitCost, io.Revenue, io.Cost, io.Profit, io.ItemId, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath, i.BuyPrice,i.StartQtyBalance,i.QtyIn as iQtyIn,i.QtyOut as iQtyOut,i.EndQtyBalance, i.CategoryId ,o.Id, o.Name, o.OrderDate, o.CustomerId FROM [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN [Item] as i ON i.Id = io.ItemId WHERE io.OrderId = @OrderId");
                command = new SqlCommand(sql, conn);
                command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = item.Id;
                reader = command.ExecuteReader();

                //tạo Book để binding vào màn hình
                while (reader.Read())
                {
                    var id = (int)reader["Id"];
                    var orderId = (int)reader["OrderId"];
                    var orderPrice = (double)reader["OrderPrice"];
                    var qtyIn_Order = (double)reader["QtyIn"];
                    var qtyOut_Order = (double)reader["QtyOut"];
                    var unitCost = (double)reader["UnitCost"];
                    var revenue = (double)reader["Revenue"];
                    var cost = (double)reader["Cost"];
                    var profit = (double)reader["Profit"];

                    //var book_OrderId    = (int)reader["Id"];
                    var itemId = (int)reader["ItemId"];
                    var name = (string)reader["Name"];
                    var sellPrice = (double)reader["SellPrice"];
                    var discountedPrice = (double)reader["DiscountedPrice"];
                    var imagePath = (string)reader["ImagePath"];
                    var buyPrice = (double)reader["BuyPrice"];
                    var startQtyBalance = (double)reader["StartQtyBalance"];
                    var qtyIn = (double)reader["iQtyIn"];
                    var qtyOut = (double)reader["iQtyOut"];
                    var endQtyBalance = (double)reader["EndQtyBalance"];
                    var categoryId = (int)reader["CategoryId"];

                    var itemBook = new Book() { Id = itemId, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice = buyPrice, StartQtyBalance = startQtyBalance, QtyIn = qtyIn, QtyOut = qtyOut, EndQtyBalance = endQtyBalance, CategoryId = categoryId};
                    var itemBook_Order = new Book_BookOrder() { Id = id, OrderId = orderId, OrderPrice = orderPrice, QtyIn = qtyIn_Order, QtyOut = qtyOut_Order, UnitCost = unitCost, Revenue = revenue, Cost = cost, Profit = profit, Book = itemBook };
                    item.OrderDetail.Add(itemBook_Order);
                }
                reader.Close();
            }
            //================bỏ
            //sql = string.Format("SELECT o.Id, o.Name, o.OrderDate, io.OrderId, io.OrderPrice, io.ItemId, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath, i.CategoryId FROM [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN Item as i ON i.Id = io.ItemId ORDER BY o.Id");
            sql     = string.Format("SELECT io.Id, io.OrderId, io.OrderPrice, io.QtyIn, io.QtyOut, io.UnitCost, io.Revenue, io.Cost, io.Profit, io.ItemId, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath, i.BuyPrice,i.StartQtyBalance,i.QtyIn as iQtyIn,i.QtyOut as iQtyOut,i.EndQtyBalance, i.CategoryId ,o.Id, o.Name, o.OrderDate, o.CustomerId FROM [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN [Item] as i ON i.Id = io.ItemId ORDER BY o.Id");
            command = new SqlCommand(sql, conn);
            reader  = command.ExecuteReader();

            //tạo Book để binding vào màn hình
            int i = 0;
            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var orderId = (int)reader["OrderId"];
                var orderPrice = (double)reader["OrderPrice"];
                var qtyIn_Order = (double)reader["QtyIn"];
                var qtyOut_Order = (double)reader["QtyOut"];
                var unitCost = (double)reader["UnitCost"];
                var revenue = (double)reader["Revenue"];
                var cost    = (double)reader["Cost"];
                var profit  = (double)reader["Profit"];
                
                //var book_OrderId    = (int)reader["Id"];
                var itemId = (int)reader["ItemId"];
                var name   = (string)reader["Name"];
                var sellPrice = (double)reader["SellPrice"];
                var discountedPrice = (double)reader["DiscountedPrice"];
                var imagePath = (string)reader["ImagePath"];
                var buyPrice  = (double)reader["BuyPrice"];
                var startQtyBalance = (double)reader["StartQtyBalance"];
                var qtyIn  = (double)reader["iQtyIn"];
                var qtyOut = (double)reader["iQtyOut"];
                var endQtyBalance = (double)reader["EndQtyBalance"];
                var categoryId = (int)reader["CategoryId"];
                var index = 0;
                foreach (var item in Books)
                    if (item.Id == itemId)
                        index = item.Index;

                var itemBook = new Book() { Id = itemId, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice = buyPrice, StartQtyBalance = startQtyBalance, QtyIn = qtyIn, QtyOut = qtyOut, EndQtyBalance = endQtyBalance, CategoryId = categoryId, Index=index };
                var itemBook_Order = new Book_BookOrder() { Id = id, OrderId = orderId, OrderPrice = orderPrice, QtyIn = qtyIn_Order, QtyOut = qtyOut_Order, UnitCost = unitCost, Revenue = revenue, Cost = cost, Profit = profit, Book = itemBook };                
                if (orderId == BookOrders[i].Id)
                    BookOrders[i].OrderDetail.Add(itemBook_Order);
                else
                {
                    i++;
                    BookOrders[i].OrderDetail.Add(itemBook_Order);
                }
            }
            reader.Close();
            #endregion
            //-------------------------------paging
            #region Paging / Search
            //BindingList<BookOrder> _viewModel = GetAll();
            start_SellQtyPerItem = new DateTime(2022, 1, 1);
            end_SellQtyPerItem = DateTime.Today;


            _viewModel = new List<BookOrder>(BookOrders);
            _viewModel2 = new List<BookOrder>(BookOrders);

            _totalItems = _viewModel.Count;
            _totalPages = calcTotalPages(_totalItems, _rowsPerPage);

            _currentPage = 1;

            createPagingInfo();  //thêm source vào combobox
            selectedPageIndex = 0;
            updateCurrentView();

            cmdPagingNext = new RelayCommand(btnNext_Click);
            cmdPagingPrev = new RelayCommand(btnPrevious_Click);
            cmdSelectionChanged = new RelayCommand(cbPages_SelectionChanged);
            cmdSearch = new RelayCommand(txtSearch_TextChanged);
            #endregion
        }
        public BindingList<BookOrder> GetAll()
        {
            var BookOrders = new BindingList<BookOrder>();

            //================ExecuteReader all rows - Order table
            var sql = string.Format("select * from [Order]");
            var command = new SqlCommand(sql, conn);
            var reader = command.ExecuteReader();

            //tạo BookOrders để binding vào màn hình
            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var name = (string)reader["Name"];
                var orderDate = (DateTime)reader["OrderDate"];
                var customerId = (string)reader["CustomerId"];

                var item = new BookOrder() { Id = id, Name = name, OrderDate = orderDate, CustomerId = customerId };
                BookOrders.Add(item);
            }
            reader.Close();
            //================ExecuteReader all rows - Book_BookOrder table
            //sql = string.Format("SELECT o.Id, o.Name, o.OrderDate, io.OrderId, io.OrderPrice, io.ItemId, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath, i.CategoryId FROM [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN Item as i ON i.Id = io.ItemId ORDER BY o.Id");

            sql = string.Format("SELECT io.Id, io.OrderId, io.OrderPrice, io.QtyIn, io.QtyOut, io.ItemId, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath, i.BuyPrice,i.StartQtyBalance,i.QtyIn as iQtyIn,i.QtyOut as iQtyOut,i.EndQtyBalance, i.CategoryId ,o.Id, o.Name, o.OrderDate, o.CustomerId FROM [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN [Item] as i ON i.Id = io.ItemId ORDER BY o.Id");
            command = new SqlCommand(sql, conn);
            reader = command.ExecuteReader();

            //tạo Book để binding vào màn hình
            int i = 0;
            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var orderId = (int)reader["OrderId"];
                var orderPrice = (double)reader["OrderPrice"];
                var qtyIn_Order = (double)reader["QtyIn"];
                var qtyOut_Order = (double)reader["QtyOut"];

                //var book_OrderId    = (int)reader["Id"];
                var itemId = (int)reader["ItemId"];
                var name = (string)reader["Name"];
                var sellPrice = (double)reader["SellPrice"];
                var discountedPrice = (double)reader["DiscountedPrice"];
                var imagePath = (string)reader["ImagePath"];
                var buyPrice = (double)reader["BuyPrice"];
                var startQtyBalance = (double)reader["StartQtyBalance"];
                var qtyIn = (double)reader["iQtyIn"];
                var qtyOut = (double)reader["iQtyOut"];
                var endQtyBalance = (double)reader["EndQtyBalance"];
                var categoryId = (int)reader["CategoryId"];
                var index = 0;
                foreach (var item in Books)
                    if (item.Id == itemId)
                        index = item.Index;

                var itemBook = new Book() { Id = itemId, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice = buyPrice, StartQtyBalance = startQtyBalance, QtyIn = qtyIn, QtyOut = qtyOut, EndQtyBalance = endQtyBalance, CategoryId = categoryId, Index = index };
                var itemBook_Order = new Book_BookOrder() { Id = id, OrderId = orderId, OrderPrice = orderPrice, QtyIn = qtyIn_Order, QtyOut = qtyOut_Order, Book = itemBook };
                if (orderId == BookOrders[i].Id)
                    BookOrders[i].OrderDetail.Add(itemBook_Order);
                else
                {
                    i++;
                    BookOrders[i].OrderDetail.Add(itemBook_Order);
                }
            }
            reader.Close();
            return BookOrders;
        }
        public BindingList<Book_BookOrder> getAll_OrderDetails()
        {
            BindingList<Book_BookOrder> lst = new BindingList<Book_BookOrder>();
            var sql = string.Format("SELECT io.Id, io.OrderId, io.OrderPrice, io.QtyIn, io.QtyOut, io.ItemId, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath, i.BuyPrice,i.StartQtyBalance,i.QtyIn as iQtyIn,i.QtyOut as iQtyOut,i.EndQtyBalance, i.CategoryId ,o.Id, o.Name, o.OrderDate, o.CustomerId FROM [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN [Item] as i ON i.Id = io.ItemId ORDER BY o.Id");
            var command = new SqlCommand(sql, conn);
            var reader = command.ExecuteReader();

            //tạo Book để binding vào màn hình
            int i = 0;
            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var orderId = (int)reader["OrderId"];
                var orderPrice = (double)reader["OrderPrice"];
                var qtyIn_Order = (double)reader["QtyIn"];
                var qtyOut_Order = (double)reader["QtyOut"];

                //var book_OrderId    = (int)reader["Id"];
                var itemId = (int)reader["ItemId"];
                var name = (string)reader["Name"];
                var sellPrice = (double)reader["SellPrice"];
                var discountedPrice = (double)reader["DiscountedPrice"];
                var imagePath = (string)reader["ImagePath"];
                var buyPrice = (double)reader["BuyPrice"];
                var startQtyBalance = (double)reader["StartQtyBalance"];
                var qtyIn = (double)reader["iQtyIn"];
                var qtyOut = (double)reader["iQtyOut"];
                var endQtyBalance = (double)reader["EndQtyBalance"];
                var categoryId = (int)reader["CategoryId"];
                var index = 0;
                foreach (var item in Books)
                    if (item.Id == itemId)
                        index = item.Index;

                var itemBook = new Book() { Id = itemId, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice = buyPrice, StartQtyBalance = startQtyBalance, QtyIn = qtyIn, QtyOut = qtyOut, EndQtyBalance = endQtyBalance, CategoryId = categoryId, Index = index };
                var itemBook_Order = new Book_BookOrder() { Id = id, OrderId = orderId, OrderPrice = orderPrice, QtyIn = qtyIn_Order, QtyOut = qtyOut_Order, Book = itemBook };
                lst.Add(itemBook_Order);
            }
            reader.Close();

            return lst;
        }

        //===================================================================================================
        //===================================================================================================CRUD
        //-----------------Visibility function
        #region Visibility function
        public void Visibility_Create(object obj)
        {
            if (isVisibleCreate == Visibility.Collapsed) isVisibleCreate = Visibility.Visible;
            else isVisibleCreate = Visibility.Collapsed;
            isVisibleUpdate = Visibility.Collapsed;
        }
        public void Visibility_Update(object obj)
        {
            if (isVisibleUpdate == Visibility.Collapsed) isVisibleUpdate = Visibility.Visible;
            else isVisibleUpdate = Visibility.Collapsed;
            isVisibleCreate = Visibility.Collapsed;
        }
        public void Visibility_CancelCreate(object obj)
        {
            if (isVisibleCreate == Visibility.Visible) isVisibleCreate = Visibility.Collapsed;
        }
        public void Visibility_CancelUpdate(object obj)
        {
            if (isVisibleUpdate == Visibility.Visible) isVisibleUpdate = Visibility.Collapsed;
        }
        #endregion

        //-----------------CRUD database
        public int findMaxId_DetailOrder()
        {
            if(OrderDetails == null || OrderDetails.Count == 0)
                return 0;
            int maxId;
            var sql = "select max(Id) as max from [Item_Order]";
            var command = new SqlCommand(sql, conn);
            //command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = SelectedOrder.Id;
            maxId = (int)command.ExecuteScalar();
            return maxId;
        }
        public int findMaxId_Order()
        {
            if(BookOrders == null || BookOrders.Count == 0)
                return 0;
            int maxId;
            var sql = "select max(Id) as max from [Order]";
            var command = new SqlCommand(sql, conn);
            //command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = SelectedOrder.Id;
            maxId = (int)command.ExecuteScalar();
            return maxId;
        }
        public void UpdateQuantity_Create(Book_BookOrder item)
        {
            //var item = SelectedBook;
            // update vào trong CSDL
            var sql = "update Item set QtyIn=@QtyIn, QtyOut=@QtyOut, EndQtyBalance=@EndQtyBalance where Id = @Id;";
            var command = new SqlCommand(sql, conn);

            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Book.Id;
            command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value = item.Book.QtyIn + item.QtyIn;
            command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = item.Book.QtyOut + item.QtyOut;
            command.Parameters.Add("EndQtyBalance", System.Data.SqlDbType.Float).Value = item.Book.EndQtyBalance + item.QtyIn - item.QtyOut;
            
            int rowCount = command.ExecuteNonQuery();
            //if (rowCount == 1) MessageBox.Show("Saved");
            //else MessageBox.Show("Not Saved");
            //---------------------------------------
            //Load();
        }
        public void UpdateQuantity_Delete(Book_BookOrder item)
        {
            //var item = SelectedBook;
            // update vào trong CSDL
            var sql = "update [Item] set QtyOut=@QtyOut, EndQtyBalance=@EndQtyBalance where Id = @Id;";
            var command = new SqlCommand(sql, conn);

            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Book.Id;
            command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = item.Book.QtyOut - item.QtyOut;
            command.Parameters.Add("EndQtyBalance", System.Data.SqlDbType.Float).Value = item.Book.EndQtyBalance + item.QtyOut;

            int rowCount = command.ExecuteNonQuery();
            //if (rowCount == 1) MessageBox.Show("Saved");
            //else MessageBox.Show("Not Saved");
            //---------------------------------------
            //Load();
        }
        public double CalcQuantity_Update(Book_BookOrder item)
        {
            var sql     = "select sum(QtyOut) from [Item_Order] where ItemId=@ItemId  ";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("ItemId", System.Data.SqlDbType.Int).Value = item.Book.Id;
            var sum   = (double)command.ExecuteScalar();
            return sum;
        }
        public void UpdateQuantity_Update(Book_BookOrder item)
        {
            item.Book.QtyOut = CalcQuantity_Update(item);
            // update vào trong CSDL
            var sql = "update Item set StartQtyBalance=@StartQtyBalance, QtyIn=@QtyIn, QtyOut=@QtyOut, EndQtyBalance=@EndQtyBalance where Id = @Id;";
            var command = new SqlCommand(sql, conn);

            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Book.Id;
            command.Parameters.Add("StartQtyBalance", System.Data.SqlDbType.Float).Value = item.Book.StartQtyBalance;
            command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value = item.Book.QtyIn;
            command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = item.Book.QtyOut;
            command.Parameters.Add("EndQtyBalance", System.Data.SqlDbType.Float).Value = item.Book.StartQtyBalance + item.Book.QtyIn - item.Book.QtyOut;

            int rowCount = command.ExecuteNonQuery();
            //if (rowCount == 1) MessageBox.Show("Saved");
            //else MessageBox.Show("Not Saved");
            //---------------------------------------
            //Load();
        }
        #region Create function
        //-----Create Command
        public void cbSelectionChanged_Books_Create(object obj)
        {
            SelectedOrderDetail_Create.Book = new Book();
            SelectedOrderDetail_Create.Book = Books[selectedBookCreateIndex];
            SelectedOrderDetail_Create.OrderPrice = Books[selectedBookCreateIndex].SellPrice;
            SelectedOrderDetail_Create.QtyOut = 1;
        }
        public void DeleteRow_Create(object obj)
        {
            OrderCreate.OrderDetail.Remove(OrderCreate.OrderDetail[selectedDetailCreateIndex]);
        }
        public void AddRow_Create(object obj)
        {
            Book_BookOrder detail = new Book_BookOrder();
            OrderCreate.OrderDetail.Add(detail);
        }
        public void btnSave_OrderCreate(object obj)
        {
            //insert bảng Order
            int maxId_Order = findMaxId_Order();
            OrderCreate.Id  = maxId_Order + 1;
            OrderCreate.OrderDate = selectedOrderDate;
            int countdb     = dbInsertOrder(OrderCreate);
            messageCount0(countdb, "Can not save Order");

            //insert bảng Detail Order
            int maxId_Detail = findMaxId_DetailOrder();
            foreach (var item in OrderCreate.OrderDetail)
            {
                item.OrderId = OrderCreate.Id;
                item.UnitCost = item.Book.BuyPrice;
                item.Revenue = item.QtyOut * item.OrderPrice;
                item.Cost = item.QtyOut * item.UnitCost;
                item.Profit = item.Revenue - item.Cost;

                item.Id      = maxId_Detail + 1;
                maxId_Detail++;

                countdb = CreateOne_OrderDetail(item);
                messageCount0(countdb, "Can not save Order");
                //Update quantity bảng Item
                UpdateQuantity_Create(item);
            }
            MessageBox.Show("Created");
            //--------
            Load();
            vmBookCategory.Load();
        }
        //-----create Order
        public int dbInsertOrder(BookOrder item)
        {
            string sql = "insert into [Order] values(@Id, @Name, @OrderDate, @CustomerId);";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;
            command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = item.Name;
            command.Parameters.Add("OrderDate", System.Data.SqlDbType.DateTime).Value = item.OrderDate;
            command.Parameters.Add("CustomerId", System.Data.SqlDbType.NVarChar).Value = "";

            int rowCount = command.ExecuteNonQuery();
            return rowCount;
        }
        public int CreateOne_OrderDetail(Book_BookOrder item)
        {            
            // update table Item_Order vào trong CSDL
            string sql = "insert into [Item_Order] values(@Id, @OrderId, @ItemId, @OrderPrice, @QtyIn, @QtyOut, @UnitCost, @Revenue, @Cost, @Profit);";
            var command = new SqlCommand(sql, conn);

            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;
            command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = item.OrderId;
            command.Parameters.Add("ItemId", System.Data.SqlDbType.Int).Value = item.Book.Id;
            command.Parameters.Add("OrderPrice", System.Data.SqlDbType.Float).Value = item.OrderPrice;
            command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value = item.QtyIn;
            command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = item.QtyOut;
            command.Parameters.Add("UnitCost", System.Data.SqlDbType.Float).Value = item.UnitCost;
            command.Parameters.Add("Revenue", System.Data.SqlDbType.Float).Value = item.Revenue;
            command.Parameters.Add("Cost", System.Data.SqlDbType.Float).Value = item.Cost;
            command.Parameters.Add("Profit", System.Data.SqlDbType.Float).Value = item.Profit;

            int rowCount = command.ExecuteNonQuery();
            return rowCount;
        }
        #endregion

        #region Update function
        //-----Update Command
        public void cbSelectionChanged_Books(object obj)
        {
            SelectedOrderDetail.Book = new Book();
            SelectedOrderDetail.Book = Books[selectedBookUpdateIndex];
            SelectedOrderDetail.OrderPrice = Books[selectedBookUpdateIndex].SellPrice;
            SelectedOrderDetail.QtyOut = 1;
        }
        public void DeleteRow_Update(object obj)
        {
            foreach (var order in BookOrders)
                foreach (var detail in order.OrderDetail)
                    if (detail.Id == SelectedOrderDetail.Id)
                    {
                        SelectedOrder.OrderDetail.Remove(detail);
                        return;
                    }
        }
        public void AddRow_update (object obj)
        {
            Book_BookOrder detail = new Book_BookOrder();
            detail.OrderId = SelectedOrder.Id;
            SelectedOrder.OrderDetail.Add(detail);
        }
        public void btnSave_OrderUpdate (object obj)
        {
            //BookOrder item = SelectedOrder;
            //-----------Lưu bảng Order
            int countdb = dbUpdateOrder(SelectedOrder);
            messageCount0(countdb, "Can not save Order");

            //-----------Kiểm tra OrderDetail cũ trong bàng Item_Order của Order đó
            int maxId = findMaxId_DetailOrder();
            foreach (var item in SelectedOrder.OrderDetail)
            {
                item.UnitCost = item.Book.BuyPrice;
                item.Revenue = item.QtyOut * item.OrderPrice;
                item.Cost    = item.QtyOut * item.UnitCost;
                item.Profit = item.Revenue - item.Cost;

                //Nếu item.Id == 0, insert detail mới
                if (item.Id == 0)
                {
                    item.Id = maxId+1;
                    maxId++;
                    
                    countdb = CreateOne_OrderDetail(item);
                    messageCount0(countdb, "Can not save Order");
                }
                else
                {
                    //Nếu item.Id =! 0, update detail 
                    countdb = UpdateOne_OrderDetail(item);
                    messageCount0(countdb, "Can not save Order");
                }
                UpdateQuantity_Update(item);
            }
            //count số dòng trong db. Nếu countdbSelect > SelectedOrder.OrderDetail.Count => xóa bớt dòng trong db
            var sql     = "select count(*) as count from [Item_Order] WHERE OrderId = @OrderId";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = SelectedOrder.Id;
            var countdbSelect = (int)command.ExecuteScalar();
            //reader.Close();            

            if (countdbSelect > SelectedOrder.OrderDetail.Count)
            {
                BindingList<Book_BookOrder> lstDetailDatabase = getOne_OrderDetail(SelectedOrder.Id);
                foreach (var db in lstDetailDatabase)
                {
                    int j;
                    for (j = 0; j < SelectedOrder.OrderDetail.Count; j++)
                        if (db.Id == SelectedOrder.OrderDetail[j].Id)
                            break;
                    if(j == SelectedOrder.OrderDetail.Count)
                    {
                        countdb = DeleteOneOrderDetail(db.Id);
                        messageCount0(countdb, "Can not save Order");
                    }
                }
            }
            //---------
            MessageBox.Show("Book Updated");
        }
        //-----update Order
        public void messageCount0(int countdb, string message)
        {
            if (countdb == 0)
            {
                MessageBox.Show(message);
                return;
            }
        }
        public BindingList<Book_BookOrder> getOne_OrderDetail(int Id)
        {
            var details = new BindingList<Book_BookOrder>();

            //================ExecuteReader all rows - Book_BookOrder table
            var sql     = string.Format("SELECT io.Id, io.OrderId, io.OrderPrice, io.QtyIn, io.QtyOut, io.ItemId, i.Name, i.SellPrice, i.DiscountedPrice, i.ImagePath, i.BuyPrice,i.StartQtyBalance,i.QtyIn,i.QtyOut,i.EndQtyBalance, i.CategoryId ,o.Id, o.Name, o.OrderDate, o.CustomerId FROM [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN [Item] as i ON i.Id = io.ItemId where OrderId = @OrderId ORDER BY o.Id ");
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = Id;
            var reader  = command.ExecuteReader();

            //tạo Book để binding vào màn hình
            while (reader.Read())
            {
                var id = (int)reader["Id"];
                var orderId = (int)reader["OrderId"];
                var orderPrice = (double)reader["OrderPrice"];
                var qtyIn_Order = (double)reader["QtyIn"];
                var qtyOut_Order = (double)reader["QtyOut"];

                //var book_OrderId    = (int)reader["Id"];
                var itemId = (int)reader["ItemId"];
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

                var itemBook = new Book() { Id = itemId, Name = name, SellPrice = sellPrice, DiscountedPrice = discountedPrice, ImagePath = imagePath, BuyPrice = buyPrice, StartQtyBalance = startQtyBalance, QtyIn = qtyIn, QtyOut = qtyOut, EndQtyBalance = endQtyBalance, CategoryId = categoryId };
                var itemBook_Order = new Book_BookOrder() { Id = id, OrderId = orderId, OrderPrice = orderPrice, QtyIn = qtyIn_Order, QtyOut = qtyOut_Order, Book = itemBook };
                details.Add(itemBook_Order);
            }
            reader.Close();

            return details;
        }
        public int dbUpdateOrder(BookOrder item)
        {            
            // update table Order vào trong CSDL
            string sql  = "update [Order] set Name = @Name, OrderDate=@OrderDate, CustomerId=@CustomerId where Id = @Id;";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value              = item.Id;
            command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value       = item.Name;
            command.Parameters.Add("OrderDate", System.Data.SqlDbType.DateTime).Value  = item.OrderDate;
            command.Parameters.Add("CustomerId", System.Data.SqlDbType.NVarChar).Value = item.CustomerId;
            
            int rowCount = command.ExecuteNonQuery();
            return rowCount;
        }
        public int UpdateOne_OrderDetail(Book_BookOrder item)
        {            
            // update table Item_Order vào trong CSDL
            var sql = "update [Item_Order] set OrderId=@OrderId, ItemId = @ItemId, OrderPrice = @OrderPrice, QtyIn = @QtyIn, QtyOut=@QtyOut, UnitCost=@UnitCost, Revenue=@Revenue, Cost=@Cost, Profit=@Profit where Id = @Id;";
            var command = new SqlCommand(sql, conn);

            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;
            command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = item.OrderId;
            command.Parameters.Add("ItemId", System.Data.SqlDbType.Int).Value = item.Book.Id;
            command.Parameters.Add("OrderPrice", System.Data.SqlDbType.Float).Value = item.OrderPrice;
            command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value = item.QtyIn;
            command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = item.QtyOut;
            command.Parameters.Add("UnitCost", System.Data.SqlDbType.Float).Value = item.UnitCost;
            command.Parameters.Add("Revenue", System.Data.SqlDbType.Float).Value = item.Revenue;
            command.Parameters.Add("Cost", System.Data.SqlDbType.Float).Value = item.Cost;
            command.Parameters.Add("Profit", System.Data.SqlDbType.Float).Value = item.Profit;

            int rowCount = command.ExecuteNonQuery();
            return rowCount;
        }
        #endregion

        #region Delete function
        //-----delete Command
        public void DeleteOrder(object obj)
        {
            int countdb = DeleteOneOrder(SelectedOrder.Id);
            messageCount0(countdb, "Can not delete Order");

            foreach (var item in SelectedOrder.OrderDetail)
                UpdateQuantity_Delete(item); //Update quantity bảng Item

            countdb = DeleteAllOrderDetail(SelectedOrder.Id);
            messageCount0(countdb, "Can not delete Order");
            
            MessageBox.Show("Deleted");
            //----
            Load();
            //vmBookCategory.Load();


            //if (obj == null)
            //{
            //    MessageBox.Show("Select a row to delete");
            //    return;
            //}
            //var item    = (BookOrder)obj;
            //// xóa vào trong CSDL
            //string sql  = "delete [Order] where Id = @Id;";
            //var command = new SqlCommand(sql, conn);
            //command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = item.Id;

            //int rowCount = command.ExecuteNonQuery();
            //if (rowCount == 1) MessageBox.Show("Deleted");
            //else MessageBox.Show("Not Deleted");
            ////---------------------------------------
            //Load();
        }
        //----delete Order
        public int DeleteOneOrder(int Id)
        {
            // xóa vào trong CSDL
            string sql = "delete [Order] where Id = @Id;";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = Id;

            int rowCount = command.ExecuteNonQuery();
            return rowCount;
        }
        public int DeleteAllOrderDetail(int OrderId)
        {
            // xóa vào trong CSDL
            string sql  = "delete [Item_Order] where OrderId = @OrderId;";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = OrderId;

            int rowCount = command.ExecuteNonQuery();
            return rowCount;
        }
        public int DeleteOneOrderDetail(int Id)
        {
            // xóa vào trong CSDL
            string sql = "delete [Item_Order] where Id = @Id;";
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = Id;

            int rowCount = command.ExecuteNonQuery();
            return rowCount;
        }
        #endregion

        #region Import function
        //-----Import command
        public void Import(object param)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                var fileName = screen.FileName;
                var workbook = new Workbook(fileName);
                var tabs = workbook.Worksheets;
                //table Order
                for (int i = 0; i < tabs.Count; i++)
                {
                    //Insert dữ liệu vào Bảng Product
                    var col = "A";
                    var row = 2;
                    var cell = tabs[i].Cells[$"{col}{row}"]; //ô đầu tiên cần import

                    while (cell.Value != null) //import từng dòng trong file excel
                    {
                        //table Order
                        var id = tabs[i].Cells[$"A{row}"].IntValue;
                        var name = tabs[i].Cells[$"B{row}"].StringValue;
                        var orderDate = tabs[i].Cells[$"C{row}"].DateTimeValue;
                        var customerId = tabs[i].Cells[$"D{row}"].StringValue;

                        // Chèn vào trong CSDL
                        string sql = "insert into [Order] values(@Id, @Name, @OrderDate, @CustomerId);";
                        SqlCommand command = new SqlCommand(sql, conn);
                        command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = id;
                        command.Parameters.Add("Name", System.Data.SqlDbType.NVarChar).Value = name;
                        command.Parameters.Add("OrderDate", System.Data.SqlDbType.DateTime).Value = orderDate;
                        command.Parameters.Add("CustomerId", System.Data.SqlDbType.NVarChar).Value = customerId;
                        command.ExecuteNonQuery();

                        row++;
                        cell = tabs[i].Cells[$"{col}{row}"];
                    }
                }
                //table Item_Order
                for (int i = 0; i < tabs.Count; i++)
                {
                    //Insert dữ liệu vào Bảng Product
                    var col = "G";
                    var row = 2;
                    var cell = tabs[i].Cells[$"{col}{row}"]; //ô đầu tiên cần import

                    while (cell.Value != null) //import từng dòng trong file excel
                    {
                        //table Order
                        var id = tabs[i].Cells[$"G{row}"].IntValue;
                        var orderId = tabs[i].Cells[$"H{row}"].IntValue;
                        var itemId = tabs[i].Cells[$"I{row}"].IntValue;
                        var orderPrice = tabs[i].Cells[$"J{row}"].DoubleValue;
                        var qtyIn = tabs[i].Cells[$"K{row}"].DoubleValue;
                        var qtyOut = tabs[i].Cells[$"L{row}"].DoubleValue;
                        var unitCost = tabs[i].Cells[$"M{row}"].DoubleValue;
                        var revenue = tabs[i].Cells[$"N{row}"].DoubleValue;
                        var cost = tabs[i].Cells[$"O{row}"].DoubleValue;
                        var profit = tabs[i].Cells[$"P{row}"].DoubleValue;

                        // Chèn vào trong CSDL
                        string sql = "insert into Item_Order values(@Id, @OrderId, @ItemId, @OrderPrice, @QtyIn, @QtyOut, @UnitCost, @Revenue, @Cost, @Profit);";
                        SqlCommand command = new SqlCommand(sql, conn);
                        command.Parameters.Add("Id", System.Data.SqlDbType.Int).Value = id;
                        command.Parameters.Add("OrderId", System.Data.SqlDbType.Int).Value = orderId;
                        command.Parameters.Add("ItemId", System.Data.SqlDbType.Int).Value = itemId;
                        command.Parameters.Add("OrderPrice", System.Data.SqlDbType.Float).Value = orderPrice;
                        command.Parameters.Add("QtyIn", System.Data.SqlDbType.Float).Value = qtyIn;
                        command.Parameters.Add("QtyOut", System.Data.SqlDbType.Float).Value = qtyOut;
                        command.Parameters.Add("UnitCost", System.Data.SqlDbType.Float).Value = unitCost;
                        command.Parameters.Add("Revenue", System.Data.SqlDbType.Float).Value = revenue;
                        command.Parameters.Add("Cost", System.Data.SqlDbType.Float).Value = cost;
                        command.Parameters.Add("Profit", System.Data.SqlDbType.Float).Value = profit;

                        command.ExecuteNonQuery();


                        row++;
                        cell = tabs[i].Cells[$"{col}{row}"];
                    }
                }

                //================Refresh screen========================================
                //Load();
                MessageBox.Show("Data imported");
            }
        }
        #endregion

        
        //===================================================================================================
        //===================================================================================================Paging
        #region Paging function
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
            BookOrders = new BindingList<BookOrder>();
            List<BookOrder> view = new List<BookOrder>();
            view = _viewModel
                .Skip((_currentPage - 1) * _rowsPerPage)
                .Take(_rowsPerPage).ToList();

            foreach (var item in view)
                BookOrders.Add(item);
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
        private void txtSearch_TextChanged(object obj)
        {
            _viewModel = new List<BookOrder>(_viewModel2);
            if (BookOrders.Count > 0)
                _viewModel = _viewModel.Where(y => y.OrderDate >= start_SellQtyPerItem && y.OrderDate <= end_SellQtyPerItem).ToList();

            _totalItems = _viewModel.Count;
            _totalPages = calcTotalPages(_totalItems, _rowsPerPage);
            _currentPage = 1;

            createPagingInfo();  //thêm source vào combobox
            selectedPageIndex = 0;
            updateCurrentView();
        }
        #endregion



    }
}