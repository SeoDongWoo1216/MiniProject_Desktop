USE [OpenApiLab]
GO
/****** Object:  Table [dbo].[NaverFavoriteMovies]    Script Date: 21-04-05-월 오후 4:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NaverFavoriteMovies](
	[Idx] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1000) NOT NULL,
	[Link] [varchar](255) NULL,
	[Image] [varchar](500) NULL,
	[SubTitle] [varchar](1000) NULL,
	[PubDate] [varchar](20) NULL,
	[Director] [nvarchar](1000) NULL,
	[Actor] [nvarchar](1000) NULL,
	[UserRating] [varchar](10) NULL,
	[RegDate] [datetime] NULL,
 CONSTRAINT [PK_NaverFavoriteMovies] PRIMARY KEY CLUSTERED 
(
	[Idx] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
