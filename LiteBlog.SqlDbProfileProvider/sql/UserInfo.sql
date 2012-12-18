USE [LiteBlogDB]
GO

/****** Object:  Table [dbo].[UserInfo]    Script Date: 12/18/2012 17:16:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Locked] [smallint] NOT NULL,
	[CreatedTime] [datetime] NULL,
	[LastLoginTime] [datetime] NULL,
	[LastActivityTime] [datetime] NULL,
	[LastPasswordTime] [datetime] NULL,
	[LastLockedTime] [datetime] NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_Locked]  DEFAULT ((0)) FOR [Locked]
GO

ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_CreatedTime]  DEFAULT (getdate()) FOR [CreatedTime]
GO

ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_LastLoginTime]  DEFAULT (getdate()) FOR [LastLoginTime]
GO

ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_LastActivityTime]  DEFAULT (getdate()) FOR [LastActivityTime]
GO

ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_LastPasswordTime]  DEFAULT (getdate()) FOR [LastPasswordTime]
GO

ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_LastLockedTime]  DEFAULT (getdate()) FOR [LastLockedTime]
GO