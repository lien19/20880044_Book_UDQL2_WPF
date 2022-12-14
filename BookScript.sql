USE [master]
GO
/****** Object:  Database [Book]    Script Date: 6/20/2022 11:24:50 PM ******/
CREATE DATABASE [Book]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Book', FILENAME = N'C:\Users\Public\0_lien\QLUD2\repo\20880044_Book\Book.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Book_log', FILENAME = N'C:\Users\Public\0_lien\QLUD2\repo\20880044_Book\Book_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Book] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Book].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Book] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Book] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Book] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Book] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Book] SET ARITHABORT OFF 
GO
ALTER DATABASE [Book] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Book] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Book] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Book] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Book] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Book] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Book] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Book] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Book] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Book] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Book] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Book] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Book] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Book] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Book] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Book] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Book] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Book] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Book] SET  MULTI_USER 
GO
ALTER DATABASE [Book] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Book] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Book] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Book] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Book] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Book] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Book] SET QUERY_STORE = OFF
GO
USE [Book]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 6/20/2022 11:24:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] NULL,
	[Name] [nvarchar](300) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 6/20/2022 11:24:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [int] NULL,
	[Name] [nvarchar](300) NULL,
	[SellPrice] [float] NULL,
	[DiscountedPrice] [float] NULL,
	[ImagePath] [nchar](100) NULL,
	[BuyPrice] [float] NULL,
	[StartQtyBalance] [float] NULL,
	[QtyIn] [float] NULL,
	[QtyOut] [float] NULL,
	[EndQtyBalance] [float] NULL,
	[CategoryId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item_Order]    Script Date: 6/20/2022 11:24:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Order](
	[Id] [int] NULL,
	[OrderId] [int] NULL,
	[ItemId] [int] NULL,
	[OrderPrice] [float] NULL,
	[QtyIn] [float] NULL,
	[QtyOut] [float] NULL,
	[UnitCost] [float] NULL,
	[Revenue] [float] NULL,
	[Cost] [float] NULL,
	[Profit] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 6/20/2022 11:24:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[OrderDate] [datetime] NULL,
	[CustomerId] [nchar](50) NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Category] ([Id], [Name]) VALUES (1, N'Kinh Tế - Khoa học - Kỹ thuật')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (2, N'Tâm lý - Kỹ năng sống')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (3, N'Sách thiếu nhi')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (4, N'Văn học_')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (5, N'Anime')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (7, N'Giáo khoa')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (8, N'Lịch sử')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (6, N'Trinh thám')
GO
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (1, N'_Mô Hình Hồi Quy Và Khám Phá Khoa Học', 150000, 120000, N'/Images/KinhTe_KhoaHoc_KyThuat/1.jpg                                                                ', 75000, 100, 0, 3, 97, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (2, N'Khoa Học Về Nấu Ăn - The Science Of Cooking', 350000, 297500, N'/Images/KinhTe_KhoaHoc_KyThuat/2.jpg                                                                ', 175000, 100, 0, 3, 97, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (3, N'Cách Chinh Phục Toán Và Khoa Học - A Mind For Numbers (Tái Bản 2022)', 169000, 147030, N'/Images/KinhTe_KhoaHoc_KyThuat/3.jpg                                                                ', 84500, 100, 0, 0, 100, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (4, N'Phân Tích Dữ Liệu Với R (Tái Bản 2020)', 250000, 200000, N'/Images/KinhTe_KhoaHoc_KyThuat/4.jpg                                                                ', 125000, 100, 0, 0, 100, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (5, N'Dữ Liệu Lớn', 149000, 119200, N'/Images/KinhTe_KhoaHoc_KyThuat/5.jpg                                                                ', 74500, 100, 0, 4, 96, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (6, N'Nuốt Cá Lớn - Eating The Big Fish (Tái Bản)', 299000, 299000, N'/Images/KinhTe_KhoaHoc_KyThuat/6.jpg                                                                ', 149500, 100, 0, 0, 100, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (7, N'B2B Tinh Gọn - Xây Dựng Sản Phẩm Mà Các Doanh Nghiệp Muốn', 148000, 118400, N'/Images/KinhTe_KhoaHoc_KyThuat/7.jpg                                                                ', 74000, 100, 0, 3, 97, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (8, N'Power Pricing - Chiến Lược Định Giá Đột Phá Thị Trường', 199000, 137310, N'/Images/KinhTe_KhoaHoc_KyThuat/8.jpg                                                                ', 99500, 100, 0, 3, 97, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (9, N'Hướng Dẫn Bài Bải Tối Ưu Hóa Chỉ Số Pay - Per - Click Cho Doanh Nghiệp - Utimate Guide Series', 190000, 152000, N'/Images/KinhTe_KhoaHoc_KyThuat/9.jpg                                                                ', 95000, 100, 0, 3, 97, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (10, N'Cách Tư Duy Và Giao Dịch Như Một Nhà Vô Địch Đầu Tư Chứng Khoán (Tái Bản 2020)', 348000, 348000, N'/Images/KinhTe_KhoaHoc_KyThuat/10.jpg                                                               ', 174000, 100, 0, 3, 97, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (11, N'Thiên Tài Bên Trái, Kẻ Điên Bên Phải (Tái Bản 2021)', 179000, 118140, N'/Images/TamLy_KyNangSong/1.jpg                                                                      ', 89500, 100, 0, 1, 99, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (12, N'Tâm Lý Học - Phác Họa Chân Dung Kẻ Phạm Tội', 145000, 118900, N'/Images/TamLy_KyNangSong/2.jpg                                                                      ', 72500, 100, 0, 3, 97, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (13, N'Luận Về Yêu (Tái Bản 2018)', 79000, 63200, N'/Images/TamLy_KyNangSong/3.jpg                                                                      ', 39500, 100, 0, 1, 99, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (14, N'Mật Mã Sự Sống', 78000, 62400, N'/Images/TamLy_KyNangSong/4.jpg                                                                      ', 39000, 100, 0, 0, 100, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (15, N'Khi Người Ấy Nói Lời Yêu, Có Rất Nhiều Điều Bạn Nên Nghĩ (Tái Bản 2021)', 99000, 81180, N'/Images/TamLy_KyNangSong/5.jpg                                                                      ', 49500, 100, 0, 0, 100, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (16, N'Giải Mã Những Biểu Hiện Cảm Xúc Trên Khuôn Mặt', 60000, 51000, N'/Images/TamLy_KyNangSong/6.jpg                                                                      ', 30000, 100, 0, 0, 100, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (17, N'Thương Lượng Với Quỷ Dữ', 75000, 58500, N'/Images/TamLy_KyNangSong/7.jpg                                                                      ', 37500, 100, 0, 0, 100, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (18, N'Sổ Tay Nhà Thôi Miên II', 145000, 116000, N'/Images/TamLy_KyNangSong/8.jpg                                                                      ', 72500, 100, 0, 0, 100, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (19, N'Lòng Tốt Của Bạn Cần Thêm Đôi Phần Sắc Sảo', 108000, 86400, N'/Images/TamLy_KyNangSong/9.jpg                                                                      ', 54000, 100, 0, 10, 90, 2)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (20, N'Dế Mèn Phiêu Lưu Ký - Tái Bản 2020', 50000, 42500, N'/Images/SachThieuNhi/1.jpg                                                                          ', 25000, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (21, N'Lớp Học Mật Ngữ - Tập 12', 35000, 31500, N'/Images/SachThieuNhi/2.jpg                                                                          ', 17500, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (22, N'Bầu Trời Trong Quả Trứng (Tái Bản 2019)', 50000, 45000, N'/Images/SachThieuNhi/3.jpg                                                                          ', 25000, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (23, N'Thần Thoại Hy Lạp Tập 15: Hành Trình Trở Về Của Odysseus (Tái Bản 2019)', 60000, 54000, N'/Images/SachThieuNhi/4.jpg                                                                          ', 30000, 100, 0, 3, 97, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (24, N'Ehon - Cây Sồi (Tái Bản 2020)', 49000, 39200, N'/Images/SachThieuNhi/5.jpg                                                                          ', 24500, 100, 0, 3, 97, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (25, N'Peter Pan', 350000, 315000, N'/Images/SachThieuNhi/6.jpg                                                                          ', 175000, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (26, N'Trái Tim Của Mẹ (Tái Bản 2021)', 60000, 57000, N'/Images/SachThieuNhi/7.jpg                                                                          ', 30000, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (27, N'Nhật Ký Chú Bé Nhút Nhát - Tập 5: Sự Thật Phũ Phàng (Tái Bản)', 55000, 44000, N'/Images/SachThieuNhi/8.jpg                                                                          ', 27500, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (28, N'Đồng Dao Cho Bé (Tái Bản 2020)', 80000, 69600, N'/Images/SachThieuNhi/9.jpg                                                                          ', 40000, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (29, N'Bé Tập Kể Chuyện - Gà Tơ Đi Học', 10000, 8500, N'/Images/SachThieuNhi/10.jpg                                                                         ', 5000, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (30, N'Hoàng Tử Bé (Tái Bản 2022)', 35000, 33250, N'/Images/SachThieuNhi/11.jpg                                                                         ', 17500, 100, 0, 3, 97, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (31, N'100 Truyện Cổ Tích Việt Nam - Quyển 1', 32000, 25600, N'/Images/SachThieuNhi/12.jpg                                                                         ', 16000, 100, 0, 3, 97, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (32, N'Bé Tập Kể Chuyện - Chuột Nhắt Lười Học', 10000, 8500, N'/Images/SachThieuNhi/13.jpg                                                                         ', 5000, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (33, N'Chuyện Rừng Xanh', 138000, 120060, N'/Images/SachThieuNhi/14.jpg                                                                         ', 69000, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (34, N'Tranh Truyện Lịch Sử Việt Nam: Lý Nam Đế (Tái Bản 2019)', 15000, 12000, N'/Images/SachThieuNhi/15.jpg                                                                         ', 7500, 100, 0, 0, 100, 3)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (35, N'Chuyện Kể Rằng Có Nàng Và Tôi', 72000, 62640, N'/Images/VanHoc/1.jpg                                                                                ', 36000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (36, N'Hai Số Phận - Bìa Cứng', 175000, 147000, N'/Images/VanHoc/2.jpg                                                                                ', 87500, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (37, N'Ra Bờ Suối Ngắm Hoa Kèn Hồng', 145000, 116000, N'/Images/VanHoc/3.jpg                                                                                ', 72500, 100, 0, 1, 99, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (38, N'Cây Chuối Non Đi Giày Xanh (Bìa Mềm) - 2018', 110000, 77000, N'/Images/VanHoc/4.jpg                                                                                ', 55000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (39, N'Thiên Thần Nhỏ Của Tôi (Tái Bản 2022)', 80000, 69600, N'/Images/VanHoc/5.jpg                                                                                ', 40000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (40, N'Những Chàng Trai Xấu Tính (Tái Bản 2022)', 85000, 73950, N'/Images/VanHoc/6.jpg                                                                                ', 42500, 100, 0, 1, 99, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (41, N'Hoa Hồng Xứ Khác (Tái Bản 2022)', 120000, 104400, N'/Images/VanHoc/7.jpg                                                                                ', 60000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (42, N'Tôi Là Bêtô - Phiên Bản Đặc Biệt - Có Minh Họa', 450000, 391500, N'/Images/VanHoc/8.jpg                                                                                ', 225000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (43, N'Con Chó Nhỏ Mang Giỏ Hoa Hồng (Tái Bản 2020)', 95000, 82650, N'/Images/VanHoc/9.jpg                                                                                ', 47500, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (44, N'Người Quảng Đi Ăn Mì Quảng (Tái Bản 2020)', 75000, 63750, N'/Images/VanHoc/10.jpg                                                                               ', 37500, 100, 0, 3, 97, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (54, N'aka', 0, 0, N's                                                                                                   ', 0, 0, 0, 3, -3, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (57, N'h', 0, 0, N'h                                                                                                   ', 0, 0, 0, 0, 0, 1)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (45, N'Con Chim Xanh Biếc Bay Về - Bìa Cứng', 270000, 175500, N'/Images/VanHoc/11.jpg                                                                               ', 135000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (46, N'Bồ Câu Không Đưa Thư (Tái Bản 2019)', 58000, 49300, N'/Images/VanHoc/12.jpg                                                                               ', 29000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (47, N'Buổi Chiều Windows (Tái Bản 2019)', 70000, 59500, N'/Images/VanHoc/13.jpg                                                                               ', 35000, 100, 0, 4, 96, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (48, N'Bong Bóng Lên Trời (Tái Bản 2019)', 58000, 49300, N'/Images/VanHoc/14.jpg                                                                               ', 29000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (49, N'Phòng Trọ Ba Người (Tái Bản 2019)', 72000, 61200, N'/Images/VanHoc/15.jpg                                                                               ', 36000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (50, N'Hạ Đỏ (Tái Bản 2018)', 60000, 43200, N'/Images/VanHoc/16.jpg                                                                               ', 30000, 100, 0, 2, 98, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (51, N'Thằng Quỷ Nhỏ', 70000, 59500, N'/Images/VanHoc/17.jpg                                                                               ', 35000, 100, 0, 0, 100, 4)
INSERT [dbo].[Item] ([Id], [Name], [SellPrice], [DiscountedPrice], [ImagePath], [BuyPrice], [StartQtyBalance], [QtyIn], [QtyOut], [EndQtyBalance], [CategoryId]) VALUES (53, N'Cô Gái Đến Từ Hôm Qua', 80000, 72800, N'/Images/VanHoc/19.jpg                                                                               ', 40000, 4, 0, 0, 4, 4)
GO
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (49, 16, 7, 148000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (50, 17, 10, 348000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (51, 17, 13, 79000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (52, 18, 11, 179000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (53, 18, 37, 145000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (56, 21, 2, 350000, 0, 3, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (1, 1, 7, 148000, 0, 2, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (2, 2, 12, 145000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (3, 3, 54, 200000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (4, 4, 30, 35000, 0, 1, 17500, 35000, 17500, 17500)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (5, 5, 31, 32000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (6, 6, 24, 49000, 0, 1, 24500, 49000, 24500, 24500)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (7, 7, 9, 190000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (8, 8, 1, 150000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (9, 9, 44, 75000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (10, 10, 47, 70000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (11, 11, 23, 60000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (12, 12, 19, 108000, 0, 8, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (13, 13, 8, 199000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (14, 14, 5, 149000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (15, 15, 50, 60000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (54, 19, 5, 149000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (17, 1, 40, 85000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (18, 2, 10, 348000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (19, 3, 12, 145000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (20, 4, 54, 200000, 0, 1, 100000, 200000, 100000, 100000)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (21, 5, 30, 35000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (22, 6, 31, 32000, 0, 1, 16000, 32000, 16000, 16000)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (23, 7, 24, 49000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (24, 8, 9, 190000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (25, 9, 1, 150000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (26, 10, 44, 75000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (27, 11, 47, 70000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (28, 12, 23, 60000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (29, 13, 19, 108000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (30, 14, 8, 199000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (31, 15, 5, 149000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (55, 20, 47, 70000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (48, 1, 5, 149000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (34, 2, 50, 60000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (35, 3, 10, 348000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (36, 4, 12, 145000, 0, 1, 72500, 145000, 72500, 72500)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (37, 5, 54, 200000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (38, 6, 30, 35000, 0, 1, 17500, 35000, 17500, 17500)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (39, 7, 31, 32000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (40, 8, 24, 49000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (41, 9, 9, 190000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (42, 10, 1, 150000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (43, 11, 44, 75000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (44, 12, 47, 70000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (45, 13, 23, 60000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (46, 14, 19, 108000, 0, 1, 0, 0, 0, 0)
INSERT [dbo].[Item_Order] ([Id], [OrderId], [ItemId], [OrderPrice], [QtyIn], [QtyOut], [UnitCost], [Revenue], [Cost], [Profit]) VALUES (47, 15, 8, 199000, 0, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (19, N'ee', CAST(N'2022-06-20T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (20, N'sss', CAST(N'2022-06-20T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (1, N'ORD-2022001', CAST(N'2022-01-01T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (2, N'ORD-2022002', CAST(N'2022-01-05T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (3, N'ORD-2022003', CAST(N'2022-02-03T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (4, N'ORD-2022004', CAST(N'2022-02-15T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (5, N'ORD-2022005', CAST(N'2022-03-02T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (6, N'ORD-2022006', CAST(N'2022-03-11T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (7, N'ORD-2022007', CAST(N'2022-03-28T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (8, N'ORD-2022008', CAST(N'2022-04-04T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (9, N'ORD-2022009', CAST(N'2022-04-13T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (10, N'ORD-2022010', CAST(N'2022-04-25T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (11, N'ORD-2022011', CAST(N'2022-05-06T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (12, N'ORD-2022012', CAST(N'2022-05-10T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (13, N'ORD-2022013', CAST(N'2022-05-16T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (14, N'ORD-2022014', CAST(N'2022-05-20T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (15, N'ORD-2022015', CAST(N'2022-05-30T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (16, N'dfd', CAST(N'2022-01-01T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (17, N'sds', CAST(N'2022-01-01T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (18, N'ORD', CAST(N'2022-01-01T00:00:00.000' AS DateTime), N'                                                  ')
INSERT [dbo].[Order] ([Id], [Name], [OrderDate], [CustomerId]) VALUES (21, N'hkk', CAST(N'2022-06-20T00:00:00.000' AS DateTime), N'                                                  ')
GO
USE [master]
GO
ALTER DATABASE [Book] SET  READ_WRITE 
GO
