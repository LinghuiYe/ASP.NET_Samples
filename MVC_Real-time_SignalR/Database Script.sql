USE [master]
GO
/****** Object:  Database [CRUD_Sample]    Script Date: 1/18/2016 11:23:00 AM ******/
CREATE DATABASE [CRUD_Sample] ON  PRIMARY 
( NAME = N'CRUD_Sample', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\CRUD_Sample.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CRUD_Sample_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\CRUD_Sample_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CRUD_Sample].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CRUD_Sample] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CRUD_Sample] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CRUD_Sample] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CRUD_Sample] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CRUD_Sample] SET ARITHABORT OFF 
GO
ALTER DATABASE [CRUD_Sample] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CRUD_Sample] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CRUD_Sample] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CRUD_Sample] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CRUD_Sample] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CRUD_Sample] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CRUD_Sample] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CRUD_Sample] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CRUD_Sample] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CRUD_Sample] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CRUD_Sample] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CRUD_Sample] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CRUD_Sample] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CRUD_Sample] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CRUD_Sample] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CRUD_Sample] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CRUD_Sample] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CRUD_Sample] SET RECOVERY FULL 
GO
ALTER DATABASE [CRUD_Sample] SET  MULTI_USER 
GO
ALTER DATABASE [CRUD_Sample] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CRUD_Sample] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CRUD_Sample', N'ON'
GO
USE [CRUD_Sample]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 1/18/2016 11:23:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CustName] [varchar](100) NULL,
	[CustEmail] [varchar](150) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 1/18/2016 11:23:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CustName] [varchar](100) NULL,
	[CustEmail] [varchar](150) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[Delete_Customer]    Script Date: 1/18/2016 11:23:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Delete_Customer]
	-- Add the parameters for the stored procedure here
	 @Id Bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM[dbo].[Customers] WHERE [Id] = @Id
	SELECT 1
END

GO
/****** Object:  StoredProcedure [dbo].[Get_Customer]    Script Date: 1/18/2016 11:23:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Customer] 
	-- Add the parameters for the stored procedure here
	@Count BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT top(@Count)* FROM [dbo].[Customers]
END

GO
/****** Object:  StoredProcedure [dbo].[Get_CustomerbyID]    Script Date: 1/18/2016 11:23:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_CustomerbyID] 
	-- Add the parameters for the stored procedure here
	@Id BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [dbo].[Customers]
	WHERE Id=@Id
END

GO
/****** Object:  StoredProcedure [dbo].[Set_Customer]    Script Date: 1/18/2016 11:23:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Set_Customer]
	-- Add the parameters for the stored procedure here
	 @CustName Nvarchar(100)
	,@CustEmail Nvarchar(150)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[Customers] ([CustName],[CustEmail])
	VALUES(@CustName,@CustEmail)
	SELECT 1
END

GO
/****** Object:  StoredProcedure [dbo].[Update_Customer]    Script Date: 1/18/2016 11:23:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Update_Customer]
	-- Add the parameters for the stored procedure here
	 @Id Bigint
	,@CustName Nvarchar(100)
	,@CustEmail Nvarchar(150)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[Customers] SET[CustName] = @CustName ,[CustEmail]= @CustEmail
	WHERE [Id] = @Id
	SELECT 1
END

GO
USE [master]
GO
ALTER DATABASE [CRUD_Sample] SET  READ_WRITE 
GO
