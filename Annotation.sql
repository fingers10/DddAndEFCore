CREATE DATABASE [EFCoreDDD]
go
USE [EFCoreDDD]
GO

/****** Object:  Table [dbo].[Annotation]    Script Date: 5/8/2020 12:56:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Annotation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Annotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AnnotationObject]    Script Date: 5/8/2020 2:13:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AnnotationObject](
	[Id] [uniqueidentifier] NOT NULL,
	[PageNumber] [int] NOT NULL,
	[AnnotationType] [bit] NOT NULL,
	[AnnotationId] [int] NOT NULL,
 CONSTRAINT [PK_AnnotationObject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AnnotationObject]  WITH CHECK ADD  CONSTRAINT [FK_AnnotationObject_Annotation] FOREIGN KEY([AnnotationId])
REFERENCES [dbo].[Annotation] ([Id])
GO

ALTER TABLE [dbo].[AnnotationObject] CHECK CONSTRAINT [FK_AnnotationObject_Annotation]
GO

