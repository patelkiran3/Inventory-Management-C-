USE [master]
GO
/****** Object:  Database [BillingSystem]    Script Date: 03-09-2020 11:49:44 ******/
CREATE DATABASE [BillingSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BillingSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.YOURFATHER\MSSQL\DATA\BillingSystem.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BillingSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.YOURFATHER\MSSQL\DATA\BillingSystem_log.ldf' , SIZE = 3456KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BillingSystem] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BillingSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BillingSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BillingSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BillingSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BillingSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BillingSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [BillingSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BillingSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BillingSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BillingSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BillingSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BillingSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BillingSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BillingSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BillingSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BillingSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BillingSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BillingSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BillingSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BillingSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BillingSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BillingSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BillingSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BillingSystem] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BillingSystem] SET  MULTI_USER 
GO
ALTER DATABASE [BillingSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BillingSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BillingSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BillingSystem] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [BillingSystem] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'BillingSystem', N'ON'
GO
USE [BillingSystem]
GO
/****** Object:  User [devesh]    Script Date: 03-09-2020 11:49:44 ******/
CREATE USER [devesh] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[tbl_categories]    Script Date: 03-09-2020 11:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](50) NULL,
	[description] [text] NULL,
	[added_date] [datetime] NULL,
	[added_by] [int] NULL,
 CONSTRAINT [PK_tbl_categories] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_dea_cust]    Script Date: 03-09-2020 11:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_dea_cust](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](50) NULL,
	[name] [varchar](150) NULL,
	[email] [varchar](150) NULL,
	[contact] [varchar](15) NULL,
	[address] [text] NULL,
	[added_date] [datetime] NULL,
	[added_by] [int] NULL,
 CONSTRAINT [PK_tbl_dea_cust] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_products]    Script Date: 03-09-2020 11:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[category] [varchar](50) NULL,
	[description] [text] NULL,
	[rate] [decimal](18, 2) NULL,
	[qty] [decimal](18, 2) NULL,
	[added_date] [datetime] NULL,
	[added_by] [int] NULL,
 CONSTRAINT [PK_tbl_products] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_transaction_detail]    Script Date: 03-09-2020 11:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_transaction_detail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NULL,
	[rate] [decimal](18, 2) NULL,
	[qty] [decimal](18, 2) NULL,
	[total] [decimal](18, 2) NULL,
	[dea_cust_id] [int] NULL,
	[added_date] [datetime] NULL,
	[added_by] [int] NULL,
 CONSTRAINT [PK_tbl_transaction_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tbl_transactions]    Script Date: 03-09-2020 11:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_transactions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](50) NULL,
	[dea_cust_id] [int] NULL,
	[grandTotal] [decimal](18, 2) NULL,
	[transaction_date] [datetime] NULL,
	[ig] [decimal](18, 2) NULL,
	[cg] [decimal](18, 2) NULL,
	[sg] [decimal](18, 2) NULL,
	[igamount] [decimal](18, 2) NULL,
	[cgamount] [decimal](18, 2) NULL,
	[sgamount] [decimal](18, 2) NULL,
	[discount] [decimal](18, 2) NULL,
	[discountamount] [decimal](18, 2) NULL,
	[added_by] [int] NULL,
 CONSTRAINT [PK_tbl_transactions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_users]    Script Date: 03-09-2020 11:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [varchar](50) NULL,
	[last_name] [varchar](50) NULL,
	[email] [varchar](150) NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[contact] [varchar](15) NULL,
	[address] [text] NULL,
	[gender] [varchar](10) NULL,
	[user_type] [varchar](15) NULL,
	[added_date] [datetime] NULL,
	[added_by] [int] NULL,
 CONSTRAINT [PK_tbl_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [BillingSystem] SET  READ_WRITE 
GO
