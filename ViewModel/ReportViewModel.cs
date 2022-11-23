using _20880044_Book.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace _20880044_Book.ViewModel
{
    public class ReportViewModel:INotifyPropertyChanged
    {
        #region Variable SeriesCollection
        public SeriesCollection rowchartSeries_SellQtyPerItem_Day { get; set; }
        public SeriesCollection rowchartSeries_SellQtyPerItem_Week{ get; set; }
        public SeriesCollection rowchartSeries_SellQtyPerItem_Month { get; set; }
        public SeriesCollection rowchartSeries_SellQtyPerItem_Year { get; set; }
        public SeriesCollection piechartSeries_Revenue_Day { get; set; }
        public SeriesCollection piechartSeries_Profit_Day { get; set; }
        public SeriesCollection piechartSeries_Revenue_Week { get; set; }
        public SeriesCollection piechartSeries_Profit_Week { get; set; }
        public SeriesCollection piechartSeries_Revenue_Month { get; set; }
        public SeriesCollection piechartSeries_Profit_Month { get; set; }
        public SeriesCollection piechartSeries_Revenue_Year { get; set; }
        public SeriesCollection piechartSeries_Profit_Year { get; set; }
        #endregion
        public Func<ChartPoint, string> PointLabel { get; set; }
        public List<string> labelY_SellQtyPerItem { get; set; }
        public Func<double, string> Formatter { get; set; }
        public DateTime start_RevenueProfit { get; set; }
        public DateTime end_RevenueProfit { get; set; }
        public DateTime start_SellQtyPerItem { get; set; }
        public DateTime end_SellQtyPerItem { get; set; }

        #region Variable ICommand
        //-------------------------------
        public ICommand cmdReport_RevenueProfitInDay { get; set; }
        public ICommand cmdReport_RevenueProfitInWeek { get; set; }
        public ICommand cmdReport_RevenueProfitInMonth { get; set; }
        public ICommand cmdReport_RevenueProfitInYear { get; set; }
        //-------------------------------
        public ICommand cmdReport_SellQtyPerItemInDay { get; set; }
        public ICommand cmdReport_SellQtyPerItemInWeek { get; set; }
        public ICommand cmdReport_SellQtyPerItemInMonth { get; set; }
        public ICommand cmdReport_SellQtyPerItemInYear { get; set; }
        #endregion

        #region Variable Visibility
        //-------------------------------
        private Visibility _isVisible_RevenueProfitInDay = Visibility.Collapsed;
        public Visibility isVisible_RevenueProfitInDay
        {
            get => _isVisible_RevenueProfitInDay;
            set { _isVisible_RevenueProfitInDay = value; }
        }
        private Visibility _isVisible_RevenueProfitInWeek = Visibility.Collapsed;
        public Visibility isVisible_RevenueProfitInWeek
        {
            get => _isVisible_RevenueProfitInWeek;
            set { _isVisible_RevenueProfitInWeek = value; }
        }
        private Visibility _isVisible_RevenueProfitInMonth = Visibility.Collapsed;
        public Visibility isVisible_RevenueProfitInMonth
        {
            get => _isVisible_RevenueProfitInMonth;
            set { _isVisible_RevenueProfitInMonth = value; }
        }
        private Visibility _isVisible_RevenueProfitInYear = Visibility.Collapsed;
        public Visibility isVisible_RevenueProfitInYear
        {
            get => _isVisible_RevenueProfitInYear;
            set { _isVisible_RevenueProfitInYear = value; }
        }
        //-------------------------------
        private Visibility _isVisible_SellQtyPerItemInDay = Visibility.Collapsed;
        public Visibility isVisible_SellQtyPerItemInDay
        {
            get => _isVisible_SellQtyPerItemInDay;
            set { _isVisible_SellQtyPerItemInDay = value; }
        }
        private Visibility _isVisible_SellQtyPerItemInWeek = Visibility.Collapsed;
        public Visibility isVisible_SellQtyPerItemInWeek
        {
            get => _isVisible_SellQtyPerItemInWeek;
            set { _isVisible_SellQtyPerItemInWeek = value; }
        }
        private Visibility _isVisible_SellQtyPerItemInMonth = Visibility.Collapsed;
        public Visibility isVisible_SellQtyPerItemInMonth
        {
            get => _isVisible_SellQtyPerItemInMonth;
            set { _isVisible_SellQtyPerItemInMonth = value; }
        }
        private Visibility _isVisible_SellQtyPerItemInYear = Visibility.Collapsed;
        public Visibility isVisible_SellQtyPerItemInYear
        {
            get => _isVisible_SellQtyPerItemInYear;
            set { _isVisible_SellQtyPerItemInYear = value; }
        }
        #endregion
        public int dashboard_CountTotalItems { get; set; }
        public int dashboard_CountNewOrderInWeek { get; set; }
        public int dashboard_CountNewOrderInMonth { get; set; }
        public List<Book> dashboard_Top5ItemsOutOfStock { get; set; }

        SqlConnection conn = null;

        public event PropertyChangedEventHandler? PropertyChanged;

        //-------------------------------
        public void Load_RevenueProfit()
        {            
            //-------------------------------PieChart
            DateTime currentdate;
            DateTime startdate;
            if (isVisible_RevenueProfitInDay == Visibility.Visible)
            {
                currentdate = end_RevenueProfit;
                startdate   = start_RevenueProfit;
                List<RevenueProfitPerCategory> RevenueProfitPerCategories = Report_RevenueProfit(startdate, currentdate);
                PiechartSeries_RevenueProfitPerCategory_Day(RevenueProfitPerCategories);
            }
            else
            {
                currentdate = DateTime.Today;
                if (isVisible_RevenueProfitInWeek == Visibility.Visible)
                {
                    var timeSpan = new TimeSpan(6, 0, 0, 0);
                    startdate = currentdate - timeSpan;
                    List<RevenueProfitPerCategory> RevenueProfitPerCategories = Report_RevenueProfit(startdate, currentdate);
                    PiechartSeries_RevenueProfitPerCategory_Week(RevenueProfitPerCategories);
                }
                else if (isVisible_RevenueProfitInMonth == Visibility.Visible)
                {
                    var timeSpan = new TimeSpan(30, 0, 0, 0);
                    startdate = currentdate - timeSpan;
                    List<RevenueProfitPerCategory> RevenueProfitPerCategories = Report_RevenueProfit(startdate, currentdate);
                    PiechartSeries_RevenueProfitPerCategory_Month(RevenueProfitPerCategories);
                }
                else
                {
                    var timeSpan = new TimeSpan(365, 0, 0, 0);
                    startdate = currentdate - timeSpan;
                    List<RevenueProfitPerCategory> RevenueProfitPerCategories = Report_RevenueProfit(startdate, currentdate);
                    PiechartSeries_RevenueProfitPerCategory_Year(RevenueProfitPerCategories);
                }
            }
            //-------------------------------RowChart
        }
        public void Load_SellQtyPerItem()
        {
            //-------------------------------PieChart
            DateTime currentdate;
            DateTime startdate;
            //-------------------------------RowChart
            if (isVisible_SellQtyPerItemInDay == Visibility.Visible)
            {
                currentdate = end_SellQtyPerItem;
                startdate   = start_SellQtyPerItem;
                List<Book> lstSellQtyPerItem = Report_SellQtyPerItem(startdate, currentdate);
                RowchartSeries_SellQtyPerItem_Day(lstSellQtyPerItem);
            }
            else
            {
                currentdate = DateTime.Today;
                if (isVisible_SellQtyPerItemInWeek == Visibility.Visible)
                {
                    var timeSpan = new TimeSpan(6, 0, 0, 0);
                    startdate = currentdate - timeSpan;
                    List<Book> lstSellQtyPerItem = Report_SellQtyPerItem(startdate, currentdate);
                    RowchartSeries_SellQtyPerItem_Week(lstSellQtyPerItem);
                }
                else if (isVisible_SellQtyPerItemInMonth == Visibility.Visible)
                {
                    var timeSpan = new TimeSpan(30, 0, 0, 0);
                    startdate = currentdate - timeSpan;
                    List<Book> lstSellQtyPerItem = Report_SellQtyPerItem(startdate, currentdate);
                    RowchartSeries_SellQtyPerItem_Month(lstSellQtyPerItem);
                }
                else
                {
                    var timeSpan = new TimeSpan(365, 0, 0, 0);
                    startdate = currentdate - timeSpan;
                    List<Book> lstSellQtyPerItem = Report_SellQtyPerItem(startdate, currentdate);
                    RowchartSeries_SellQtyPerItem_Year(lstSellQtyPerItem);
                }
            }

        }
        public ReportViewModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //-------------------------------connect database
            string connString = AppConfig.ConnectionString()!;
            conn = new SqlConnection(connString);
            conn.Open();
            //-------------------------------Dashboard
            dashboard_CountTotalItems      = Dashboard_CountTotalItems();
            dashboard_CountNewOrderInWeek  = Dashboard_CountNewOrderInWeek();
            dashboard_CountNewOrderInMonth = Dashboard_CountNewOrderInMonth();
            dashboard_Top5ItemsOutOfStock  = Dashboard_Top5ItemsOutOfStock();

            //-------------------------------button chart
            start_RevenueProfit  = new DateTime(2022, 1, 1);
            end_RevenueProfit    = DateTime.Today;
            start_SellQtyPerItem = new DateTime(2022, 1, 1);
            end_SellQtyPerItem   = DateTime.Today;
            //-------------------------------PieChart
            isVisible_RevenueProfitInMonth = Visibility.Visible;
            isVisible_SellQtyPerItemInMonth = Visibility.Visible;
            Load_RevenueProfit();
            Load_SellQtyPerItem();
            //-------------------------------Command PieChart
            cmdReport_RevenueProfitInDay = new RelayCommand(Report_RevenueProfitInDay);
            cmdReport_RevenueProfitInWeek = new RelayCommand(Report_RevenueProfitInWeek);
            cmdReport_RevenueProfitInMonth = new RelayCommand(Report_RevenueProfitInMonth);
            cmdReport_RevenueProfitInYear = new RelayCommand(Report_RevenueProfitInYear);

            cmdReport_SellQtyPerItemInDay = new RelayCommand(Report_SellQtyPerItemInDay);
            cmdReport_SellQtyPerItemInWeek = new RelayCommand(Report_SellQtyPerItemInWeek);
            cmdReport_SellQtyPerItemInMonth = new RelayCommand(Report_SellQtyPerItemInMonth);
            cmdReport_SellQtyPerItemInYear = new RelayCommand(Report_SellQtyPerItemInYear);
        }

        //===================================================================================================REPORT
        //===================================================================================================
        public int Dashboard_CountTotalItems()
        {
            //================ExecuteReader all rows - Item table
            var sql = string.Format("select count(Id) as count from [Item]");
            var command = new SqlCommand(sql, conn);
            var reader = command.ExecuteReader();
            var count = 0;
            //reader
            while (reader.Read())
            {
                count = (int)reader["count"];
            }
            reader.Close();
            return count;
        }
        public int Dashboard_CountNewOrderInWeek()
        {
            //================ExecuteReader all rows - Item table
            var currentdate = DateTime.Today;
            var timeSpan = new TimeSpan(6, 0, 0, 0);
            DateTime startdate = currentdate - timeSpan;

            var sql = string.Format("select count(Id) as count from [Order] where OrderDate >= @startdate and OrderDate <= @currentdate");
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("startdate", System.Data.SqlDbType.DateTime).Value = startdate;
            command.Parameters.Add("currentdate", System.Data.SqlDbType.DateTime).Value = currentdate;
            var reader = command.ExecuteReader();
            var count = 0;
            //reader
            while (reader.Read())
            {
                count = (int)reader["count"];
            }
            reader.Close();
            return count;
        }
        public int Dashboard_CountNewOrderInMonth()
        {
            //================ExecuteReader all rows - Item table
            var currentdate = DateTime.Today;
            var timeSpan = new TimeSpan(30, 0, 0, 0);
            DateTime startdate = currentdate - timeSpan;

            var sql = string.Format("select count(Id) as count from [Order] where OrderDate >= @startdate and OrderDate <= @currentdate");
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("startdate", System.Data.SqlDbType.DateTime).Value = startdate;
            command.Parameters.Add("currentdate", System.Data.SqlDbType.DateTime).Value = currentdate;
            var reader = command.ExecuteReader();
            var count = 0;
            //reader
            while (reader.Read())
            {
                count = (int)reader["count"];
            }
            reader.Close();
            return count;
        }
        public List<Book> Dashboard_Top5ItemsOutOfStock()
        {
            List<Book> Top5ItemsOutOfStock = new List<Book>();
            //================ExecuteReader all rows - Item table
            var minStock = 5;

            var sql = string.Format("select TOP 5 * from [Item] where EndQtyBalance < @EndQtyBalance order by EndQtyBalance, Name");
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("EndQtyBalance", System.Data.SqlDbType.Float).Value = minStock;
            var reader = command.ExecuteReader();

            //reader
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
                Top5ItemsOutOfStock.Add(item);
            }
            reader.Close();
            return Top5ItemsOutOfStock;
        }
        //-------------------------------
        #region Report_RevenueProfit
        public void Report_RevenueProfitInDay(object obj)
        {
            if(isVisible_RevenueProfitInDay == Visibility.Collapsed)
            {
                isVisible_RevenueProfitInDay = Visibility.Visible;
                isVisible_RevenueProfitInWeek = Visibility.Collapsed;
                isVisible_RevenueProfitInMonth = Visibility.Collapsed;
                isVisible_RevenueProfitInYear = Visibility.Collapsed;
            }
            Load_RevenueProfit();
        }
        public void Report_RevenueProfitInWeek(object obj)
        {
            if (isVisible_RevenueProfitInWeek == Visibility.Collapsed)
            {
                isVisible_RevenueProfitInWeek = Visibility.Visible;
                isVisible_RevenueProfitInDay = Visibility.Collapsed;
                isVisible_RevenueProfitInMonth = Visibility.Collapsed;
                isVisible_RevenueProfitInYear = Visibility.Collapsed;
            }
            Load_RevenueProfit();
        }
        public void Report_RevenueProfitInMonth(object obj)
        {
            if (isVisible_RevenueProfitInMonth == Visibility.Collapsed)
            {
                isVisible_RevenueProfitInMonth = Visibility.Visible;
                isVisible_RevenueProfitInDay = Visibility.Collapsed;
                isVisible_RevenueProfitInWeek = Visibility.Collapsed;

                isVisible_RevenueProfitInYear = Visibility.Collapsed;
            }
            Load_RevenueProfit();
        }
        public void Report_RevenueProfitInYear(object obj)
        {
            if (isVisible_RevenueProfitInYear == Visibility.Collapsed)
            {
                isVisible_RevenueProfitInDay   = Visibility.Collapsed;
                isVisible_RevenueProfitInWeek  = Visibility.Collapsed;
                isVisible_RevenueProfitInMonth = Visibility.Collapsed;
                isVisible_RevenueProfitInYear  = Visibility.Visible;
            }
            Load_RevenueProfit();
        }
        public void PiechartSeries_RevenueProfitPerCategory_Day(List<RevenueProfitPerCategory> RevenueProfitPerCategories)
        {
            if (RevenueProfitPerCategories.Count <= 0)
            {
                return;
            }
            piechartSeries_Revenue_Day = new SeriesCollection();
            piechartSeries_Profit_Day = new SeriesCollection();
            for (int i = 0; i < RevenueProfitPerCategories.Count; i++)
            {
                piechartSeries_Revenue_Day.Add(new PieSeries()
                {
                    Values = new ChartValues<double> { RevenueProfitPerCategories[i].Revenue },
                    Title  = RevenueProfitPerCategories[i].CategoryName,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} {1} ({2:P})", chartPoint.Y, Environment.NewLine, chartPoint.Participation)
                });
                piechartSeries_Profit_Day.Add(new PieSeries()
                {
                    Values = new ChartValues<double> { RevenueProfitPerCategories[i].Profit },
                    Title  = RevenueProfitPerCategories[i].CategoryName,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} {1} ({2:P})", chartPoint.Y, Environment.NewLine, chartPoint.Participation),
                });
            }
        }
        public void PiechartSeries_RevenueProfitPerCategory_Week(List<RevenueProfitPerCategory> RevenueProfitPerCategories)
        {
            if (RevenueProfitPerCategories.Count <= 0)
            {
                return;
            }
            piechartSeries_Revenue_Week = new SeriesCollection();
            piechartSeries_Profit_Week = new SeriesCollection();
            for (int i = 0; i < RevenueProfitPerCategories.Count; i++)
            {
                piechartSeries_Revenue_Week.Add(new PieSeries()
                {
                    Values = new ChartValues<double> { RevenueProfitPerCategories[i].Revenue },
                    Title = RevenueProfitPerCategories[i].CategoryName,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} {1} ({2:P})", chartPoint.Y, Environment.NewLine, chartPoint.Participation)
                });
                piechartSeries_Profit_Week.Add(new PieSeries()
                {
                    Values = new ChartValues<double> { RevenueProfitPerCategories[i].Profit },
                    Title = RevenueProfitPerCategories[i].CategoryName,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} {1} ({2:P})", chartPoint.Y, Environment.NewLine, chartPoint.Participation),
                });
            }
        }
        public void PiechartSeries_RevenueProfitPerCategory_Month(List<RevenueProfitPerCategory> RevenueProfitPerCategories)
        {
            if (RevenueProfitPerCategories.Count <= 0)
            {
                return;
            }
            piechartSeries_Revenue_Month = new SeriesCollection();
            piechartSeries_Profit_Month = new SeriesCollection();
            for (int i = 0; i < RevenueProfitPerCategories.Count; i++)
            {
                piechartSeries_Revenue_Month.Add(new PieSeries()
                {
                    Values = new ChartValues<double> { RevenueProfitPerCategories[i].Revenue },
                    Title = RevenueProfitPerCategories[i].CategoryName,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} {1} ({2:P})", chartPoint.Y, Environment.NewLine, chartPoint.Participation)
                });
                piechartSeries_Profit_Month.Add(new PieSeries()
                {
                    Values = new ChartValues<double> { RevenueProfitPerCategories[i].Profit },
                    Title = RevenueProfitPerCategories[i].CategoryName,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} {1} ({2:P})", chartPoint.Y, Environment.NewLine, chartPoint.Participation),
                });
            }
        }
        public void PiechartSeries_RevenueProfitPerCategory_Year(List<RevenueProfitPerCategory> RevenueProfitPerCategories)
        {
            if (RevenueProfitPerCategories.Count <= 0)
            {
                return;
            }
            piechartSeries_Revenue_Year = new SeriesCollection();
            piechartSeries_Profit_Year = new SeriesCollection();
            for (int i = 0; i < RevenueProfitPerCategories.Count; i++)
            {
                piechartSeries_Revenue_Year.Add(new PieSeries()
                {
                    Values = new ChartValues<double> { RevenueProfitPerCategories[i].Revenue },
                    Title = RevenueProfitPerCategories[i].CategoryName,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} {1} ({2:P})", chartPoint.Y, Environment.NewLine, chartPoint.Participation)
                });
                piechartSeries_Profit_Year.Add(new PieSeries()
                {
                    Values = new ChartValues<double> { RevenueProfitPerCategories[i].Profit },
                    Title = RevenueProfitPerCategories[i].CategoryName,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} {1} ({2:P})", chartPoint.Y, Environment.NewLine, chartPoint.Participation),
                });
            }
        }
        public List<RevenueProfitPerCategory> Report_RevenueProfit(DateTime startdate, DateTime currentdate)
        {
            //================ExecuteReader all rows - Item table
            List<RevenueProfitPerCategory> RevenueProfitPerCategoryList = new List<RevenueProfitPerCategory>();

            var sql     = string.Format("select c.Id, c.Name, sum(io.Revenue) as RevenuePerCategory, sum(io.Profit) as ProfitPerCategory from [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN [Item] as i ON i.Id = io.ItemId JOIN [Category] as c ON c.Id = i.CategoryId where OrderDate >= @startdate and OrderDate <= @currentdate GROUP BY c.Id, c.Name ORDER BY c.Id");
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("startdate", System.Data.SqlDbType.DateTime).Value = startdate;
            command.Parameters.Add("currentdate", System.Data.SqlDbType.DateTime).Value = currentdate;
            var reader = command.ExecuteReader();

            //reader
            while (reader.Read())
            {
                var id   = (int)reader["Id"];
                var name = (string)reader["Name"];
                var revenuePerCategory = (double)reader["RevenuePerCategory"];
                var profitPerCategory  = (double)reader["ProfitPerCategory"];

                var item = new RevenueProfitPerCategory() { CategoryId = id, CategoryName = name, Revenue = revenuePerCategory, Profit = profitPerCategory };
                RevenueProfitPerCategoryList.Add(item);
            }
            reader.Close();
            return RevenueProfitPerCategoryList;
        }
        #endregion
        //-------------------------------
        #region Report_SellQtyPerItem
        public void Report_SellQtyPerItemInDay(object obj)
        {
            if (isVisible_SellQtyPerItemInDay == Visibility.Collapsed)
            {
                isVisible_SellQtyPerItemInDay = Visibility.Visible;
                isVisible_SellQtyPerItemInWeek = Visibility.Collapsed;
                isVisible_SellQtyPerItemInMonth = Visibility.Collapsed;
                isVisible_SellQtyPerItemInYear = Visibility.Collapsed;
            }
            Load_SellQtyPerItem();
        }
        public void Report_SellQtyPerItemInWeek(object obj)
        {
            if (isVisible_SellQtyPerItemInWeek == Visibility.Collapsed)
            {
                isVisible_SellQtyPerItemInDay = Visibility.Collapsed;
                isVisible_SellQtyPerItemInWeek = Visibility.Visible;
                isVisible_SellQtyPerItemInMonth = Visibility.Collapsed;
                isVisible_SellQtyPerItemInYear = Visibility.Collapsed;
            }
            Load_SellQtyPerItem();
        }
        public void Report_SellQtyPerItemInMonth(object obj)
        {
            if (isVisible_SellQtyPerItemInMonth == Visibility.Collapsed)
            {
                isVisible_SellQtyPerItemInDay = Visibility.Collapsed;
                isVisible_SellQtyPerItemInWeek = Visibility.Collapsed;
                isVisible_SellQtyPerItemInMonth = Visibility.Visible;
                isVisible_SellQtyPerItemInYear = Visibility.Collapsed;
            }
            Load_SellQtyPerItem();
        }
        public void Report_SellQtyPerItemInYear(object obj)
        {
            if (isVisible_SellQtyPerItemInYear == Visibility.Collapsed)
            {
                isVisible_SellQtyPerItemInDay = Visibility.Collapsed;
                isVisible_SellQtyPerItemInWeek = Visibility.Collapsed;
                isVisible_SellQtyPerItemInMonth = Visibility.Collapsed;
                isVisible_SellQtyPerItemInYear = Visibility.Visible;
            }
            Load_SellQtyPerItem();
        }
        public void RowchartSeries_SellQtyPerItem_Day(List<Book> lstSellQtyPerItem)
        {
            if (lstSellQtyPerItem.Count <= 0)
            {
                return;
            }
            rowchartSeries_SellQtyPerItem_Day = new SeriesCollection();
            var chartValues                   = new ChartValues<double>();
            labelY_SellQtyPerItem             = new List<string>();
            //Formatter = value => value.ToString("N");
            for (int i = 0; i < lstSellQtyPerItem.Count; i++)
            {
                chartValues.Add(lstSellQtyPerItem[i].QtyOut);
                labelY_SellQtyPerItem.Add(lstSellQtyPerItem[i].Name);
            }
            rowchartSeries_SellQtyPerItem_Day.Add(new RowSeries()
            {
                Values = chartValues, DataLabels = true, RowPadding = 1, Title = "Book"
            });
        }
        public void RowchartSeries_SellQtyPerItem_Week(List<Book> lstSellQtyPerItem)
        {
            if (lstSellQtyPerItem.Count <= 0)
            {
                return;
            }
            rowchartSeries_SellQtyPerItem_Week = new SeriesCollection();
            var chartValues                    = new ChartValues<double>();
            labelY_SellQtyPerItem              = new List<string>();
            //Formatter = value => value.ToString("N");
            for (int i = 0; i < lstSellQtyPerItem.Count; i++)
            {
                chartValues.Add(lstSellQtyPerItem[i].QtyOut);
                labelY_SellQtyPerItem.Add(lstSellQtyPerItem[i].Name);
            }
            rowchartSeries_SellQtyPerItem_Week.Add(new RowSeries()
            {
                Values = chartValues, DataLabels = true, RowPadding = 1, Title = "Book", MaxRowHeigth = 20
            });
        }
        public void RowchartSeries_SellQtyPerItem_Month(List<Book> lstSellQtyPerItem)
        {
            if (lstSellQtyPerItem.Count <= 0)
            {
                return;
            }
            rowchartSeries_SellQtyPerItem_Month = new SeriesCollection();
            var chartValues                     = new ChartValues<double>();
            labelY_SellQtyPerItem               = new List<string>();
            //Formatter = value => value.ToString("N");
            for (int i = 0; i < lstSellQtyPerItem.Count; i++)
            {
                chartValues.Add(lstSellQtyPerItem[i].QtyOut);
                labelY_SellQtyPerItem.Add(lstSellQtyPerItem[i].Name);
            }
            rowchartSeries_SellQtyPerItem_Month.Add(new RowSeries()
            {
                Values = chartValues, DataLabels = true, RowPadding = 1, Title = "Book", MaxRowHeigth = 20
            });
        }
        public void RowchartSeries_SellQtyPerItem_Year(List<Book> lstSellQtyPerItem)
        {
            if (lstSellQtyPerItem.Count <= 0)
            {
                return;
            }
            rowchartSeries_SellQtyPerItem_Year = new SeriesCollection();
            var chartValues                    = new ChartValues<double>();
            labelY_SellQtyPerItem              = new List<string>();
            //Formatter = value => value.ToString("N");
            for (int i = 0; i < lstSellQtyPerItem.Count; i++)
            {
                chartValues.Add(lstSellQtyPerItem[i].QtyOut);
                labelY_SellQtyPerItem.Add(lstSellQtyPerItem[i].Name);
            }
            rowchartSeries_SellQtyPerItem_Year.Add(new RowSeries()
            {
                Values = chartValues, DataLabels = true, RowPadding = 1, Title = "Book", MaxRowHeigth = 20
            });
        }
        public List<Book> Report_SellQtyPerItem(DateTime startdate, DateTime currentdate)
        {
            List<Book> lstSellQtyPerItem = new List<Book>();

            var sql     = string.Format("select i.Id, i.Name, sum(io.QtyOut) as QtyOutPerItem from [Order] as o JOIN [Item_Order] as io ON o.Id = io.OrderId JOIN [Item] as i ON i.Id = io.ItemId WHERE OrderDate >= @startdate and OrderDate <= @currentdate AND io.QtyOut > 0 GROUP BY i.Id, i.Name ORDER BY QtyOutPerItem ");
            var command = new SqlCommand(sql, conn);
            command.Parameters.Add("startdate", System.Data.SqlDbType.DateTime).Value = startdate;
            command.Parameters.Add("currentdate", System.Data.SqlDbType.DateTime).Value = currentdate;
            var reader  = command.ExecuteReader();

            //reader
            while (reader.Read())
            {
                var id   = (int)reader["Id"];
                var name = (string)reader["Name"];
                var qtyOutPerItem = (double)reader["QtyOutPerItem"];

                var item = new Book() { Id = id, Name = name, QtyOut = qtyOutPerItem };
                lstSellQtyPerItem.Add(item);
            }
            reader.Close();
            return lstSellQtyPerItem;
        }
        #endregion

    }
}
