USE [master]
GO
/****** Object:  Database [sss]    Script Date: 07/22/2021 08:59:21 ******/
CREATE DATABASE [sss] ON  PRIMARY 
( NAME = N'sss', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\sss.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'sss_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\sss_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [sss] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [sss].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [sss] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [sss] SET ANSI_NULLS OFF
GO
ALTER DATABASE [sss] SET ANSI_PADDING OFF
GO
ALTER DATABASE [sss] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [sss] SET ARITHABORT OFF
GO
ALTER DATABASE [sss] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [sss] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [sss] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [sss] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [sss] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [sss] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [sss] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [sss] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [sss] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [sss] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [sss] SET  DISABLE_BROKER
GO
ALTER DATABASE [sss] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [sss] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [sss] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [sss] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [sss] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [sss] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [sss] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [sss] SET  READ_WRITE
GO
ALTER DATABASE [sss] SET RECOVERY SIMPLE
GO
ALTER DATABASE [sss] SET  MULTI_USER
GO
ALTER DATABASE [sss] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [sss] SET DB_CHAINING OFF
GO
USE [sss]
GO
/****** Object:  Table [dbo].[reward]    Script Date: 07/22/2021 08:59:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reward](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[reward_money] [float] NOT NULL,
	[type_of_suggest] [nchar](10) NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_reward] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[systemuser]    Script Date: 07/22/2021 08:59:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[systemuser](
	[userid] [nchar](10) NOT NULL,
	[username] [nchar](10) NOT NULL,
	[password] [nchar](128) NOT NULL,
	[role] [nchar](10) NULL,
	[department] [nchar](10) NULL,
	[email] [nchar](50) NULL,
	[description] [nvarchar](max) NULL,
	[created_date] [date] NULL,
 CONSTRAINT [PK_systemuser] PRIMARY KEY CLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[suggestion]    Script Date: 07/22/2021 08:59:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[suggestion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
	[implement_date] [date] NULL,
	[status_type] [nchar](10) NULL,
	[remark_from_approver] [nvarchar](max) NULL,
	[reward_money] [float] NULL,
	[userid] [nchar](10) NULL,
	[creator] [nchar](10) NULL,
	[created_date] [date] NULL,
	[updated_date] [date] NULL,
 CONSTRAINT [PK_suggestion] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_suggestion_systemuser]    Script Date: 07/22/2021 08:59:22 ******/
ALTER TABLE [dbo].[suggestion]  WITH CHECK ADD  CONSTRAINT [FK_suggestion_systemuser] FOREIGN KEY([userid])
REFERENCES [dbo].[systemuser] ([userid])
GO
ALTER TABLE [dbo].[suggestion] CHECK CONSTRAINT [FK_suggestion_systemuser]
GO

INSERT INTO [sss].[dbo].[systemuser]
           ([userid],[username],[password],[role],[department],[email],[created_date])
     VALUES
           ('suggestor','suggestor','25d55ad283aa400af464c76d713c07ad' ,'Suggestor','DEV_BAS' ,'a@x.y','2021-07-12'),
           ('admin' ,'admin'  ,'25d55ad283aa400af464c76d713c07ad' ,'Admin' ,'DEV_BAS' ,'b@x.y','2021-07-12'),
           ('router' ,'router'  ,'25d55ad283aa400af464c76d713c07ad' ,'Router' ,'DEV_BAS' ,'c@x.y','2021-07-12'),
           ('manager' ,'manager'  ,'25d55ad283aa400af464c76d713c07ad' ,'Manager' ,'DEV_BAS' ,'d@x.y','2021-07-12'),
           ('cqtrong' ,'cqtrong'  ,'25d55ad283aa400af464c76d713c07ad' ,'Suggestor' ,'DEV_BAS' ,'a@x.y','2021-07-12'),
           ('pnkhanh' ,'pnkhanh'  ,'25d55ad283aa400af464c76d713c07ad' ,'Suggestor' ,'DEV_BAS' ,'a@x.y','2021-07-12'),
           ('vtcong' ,'vtcong'  ,'25d55ad283aa400af464c76d713c07ad' ,'Suggestor' ,'DEV_BAS' ,'a@x.y','2021-07-12');
GO

INSERT INTO [sss].[dbo].[suggestion]
           ([title],[description],[implement_date],[userid] ,[creator],[created_date],[updated_date])
     VALUES
           (N'Đóng góp ý kiến về vấn đề điện',N'Vấn đề điện hiện nay như thế ...','2021-07-23','router' ,'suggestor','2021-07-22','2021-07-22'),
           (N'Đóng góp ý kiến về vấn đề nước',N'Vấn đề nước hiện nay như thế ...','2021-07-22','router' ,'suggestor','2021-07-20','2021-07-20'),
           (N'Đóng góp ý kiến về vấn đề lương',N'Vấn đề lương hiện nay như thế ...','2021-07-23','router' ,'suggestor','2021-07-23','2021-07-23'),
           (N'Đóng góp ý kiến về vấn đề thưởng',N'Vấn đề thưởng hiện nay như thế ...','2021-07-20','router' ,'suggestor','2021-07-23','2021-07-23'),
           (N'Đóng góp ý kiến về vấn đề nghỉ phép',N'Vấn đề nghỉ phép hiện nay như thế ...','2021-07-23','router' ,'pnkhanh','2021-07-20','2021-07-20'),
           (N'Đóng góp ý kiến về vấn đề công việc',N'Vấn đề công việc hiện nay như thế ...','2021-07-19','router' ,'pnkhanh','2021-07-20','2021-07-20'),
           (N'Đóng góp ý kiến về vấn đề quy trình',N'Vấn đề quy trình hiện nay như thế ...','2021-07-23','router' ,'pnkhanh','2021-07-22','2021-07-22'),
           (N'Đóng góp ý kiến về vấn đề nhân sự',N'Vấn đề nhân sự hiện nay như thế ...','2021-07-23','router' ,'pnkhanh','2021-07-22','2021-07-22'),
           (N'Đóng góp ý kiến về vấn đề kỹ thuật',N'Vấn đề kỹ thuật hiện nay như thế ...','2021-07-23','router' ,'pnkhanh','2021-07-22','2021-07-22'),
           (N'Đóng góp ý kiến về vấn đề điện nước',N'Vấn đề điện nước hiện nay như thế ...','2021-07-23','router' ,'pnkhanh','2021-07-22','2021-07-22');
GO