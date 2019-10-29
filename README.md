Create table with sql command. MSSQL
-------------------------------------------------------------------------------------------

USE [PropertyDb]
GO

/****** Object:  Table [dbo].[Properties]    Script Date: 10/29/2019 12:45:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Properties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[PropertyDescriptionTitle] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Pictures] [nvarchar](max) NULL,
	[Location] [nvarchar](100) NULL,
	[Area] [nvarchar](50) NULL,
	[PropertyVector] [nvarchar](max) NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


