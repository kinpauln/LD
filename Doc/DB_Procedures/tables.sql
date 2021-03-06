USE [LotteryDraw]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[RoleType] [int] NOT NULL,
	[RoleTypeNum] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogInfoes]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogInfoes](
	[Id] [uniqueidentifier] NOT NULL,
	[Thread] [nvarchar](50) NULL,
	[Level] [nvarchar](50) NULL,
	[Logger] [nvarchar](100) NULL,
	[Operator] [nvarchar](50) NULL,
	[IpAddress] [nvarchar](15) NULL,
	[EntityName] [nvarchar](300) NULL,
	[MethodName] [nvarchar](500) NULL,
	[LogType] [int] NOT NULL,
	[LogTypeNum] [int] NOT NULL,
	[ResultType] [int] NOT NULL,
	[ResultTypeNum] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Exception] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.LogInfoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](32) NOT NULL,
	[Name] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NOT NULL,
	[MemberType] [int] NOT NULL,
	[MemberTypeNum] [int] NOT NULL,
	[PubishingEnableTimes] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Members] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberExtends]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberExtends](
	[Id] [uniqueidentifier] NOT NULL,
	[Tel] [nvarchar](max) NULL,
	[AdvertisingUrl] [nvarchar](max) NULL,
	[Province] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Town] [nvarchar](max) NULL,
	[Suffix] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Member_Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.MemberExtends] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoginLogs]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginLogs](
	[Id] [uniqueidentifier] NOT NULL,
	[IpAddress] [nvarchar](15) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Member_Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.LoginLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMembers]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMembers](
	[Role_Id] [bigint] NOT NULL,
	[Member_Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.RoleMembers] PRIMARY KEY CLUSTERED 
(
	[Role_Id] ASC,
	[Member_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RechargeHistories]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RechargeHistories](
	[Id] [uniqueidentifier] NOT NULL,
	[MoneyValue] [float] NOT NULL,
	[PubTimes] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Member_Id] [bigint] NOT NULL,
	[Operator_Id] [bigint] NULL,
 CONSTRAINT [PK_dbo.RechargeHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prizes]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Prizes](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Photo] [varbinary](max) NULL,
	[Description] [nvarchar](max) NULL,
	[UpdateDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Member_Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Prizes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrizePhotoes]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrizePhotoes](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[PhotoTypeNum] [int] NOT NULL,
	[PhotoType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Prize_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.PrizePhotoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrizeOrders]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrizeOrders](
	[Id] [uniqueidentifier] NOT NULL,
	[RevealType] [int] NOT NULL,
	[RevealTypeNum] [int] NOT NULL,
	[RevealState] [int] NOT NULL,
	[RevealStateNum] [int] NOT NULL,
	[SortOrder] [int] NULL,
	[RevealDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Prize_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.PrizeOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WhiteLists]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WhiteLists](
	[Id] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Member_Id] [bigint] NULL,
	[PrizeOrder_Id] [uniqueidentifier] NULL,
 CONSTRAINT [PK_dbo.WhiteLists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopOrders]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopOrders](
	[Id] [uniqueidentifier] NOT NULL,
	[PaymentAmount] [decimal](18, 2) NOT NULL,
	[Sequence] [int] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[PrizeOrder_Id] [uniqueidentifier] NULL,
	[Operator_Id] [bigint] NULL,
 CONSTRAINT [PK_dbo.TopOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LotteryResults]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LotteryResults](
	[Id] [uniqueidentifier] NOT NULL,
	[State] [int] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Member_Id] [bigint] NULL,
	[PrizeOrder_Id] [uniqueidentifier] NULL,
 CONSTRAINT [PK_dbo.LotteryResults] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrizeOrderExtends]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrizeOrderExtends](
	[PrizeOrder_Id] [uniqueidentifier] NOT NULL,
	[ScopeType] [int] NOT NULL,
	[ScopeTypeNum] [int] NOT NULL,
	[ScopeCity] [nvarchar](max) NULL,
	[LaunchTime] [datetime] NULL,
	[MinLuckyCount] [int] NOT NULL,
	[LuckyPercent] [real] NULL,
	[PoolCount] [int] NULL,
	[LuckyCount] [int] NULL,
	[Remarks] [nvarchar](max) NULL,
	[AnswerRevealConditionTypeNum] [int] NOT NULL,
	[AnswerRevealConditionType] [int] NOT NULL,
	[Question] [nvarchar](max) NULL,
	[AnswerOptions] [nvarchar](max) NULL,
	[Answer] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.PrizeOrderExtends] PRIMARY KEY CLUSTERED 
(
	[PrizeOrder_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrizeBettings]    Script Date: 12/09/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrizeBettings](
	[Id] [uniqueidentifier] NOT NULL,
	[Phone] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[AnswerOption] [nvarchar](10) NULL,
	[IsDeleted] [bit] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[Member_Id] [bigint] NOT NULL,
	[PrizeOrder_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.PrizeBettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_dbo.LoginLogs_dbo.Members_Member_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[LoginLogs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LoginLogs_dbo.Members_Member_Id] FOREIGN KEY([Member_Id])
REFERENCES [dbo].[Members] ([Id])
GO
ALTER TABLE [dbo].[LoginLogs] CHECK CONSTRAINT [FK_dbo.LoginLogs_dbo.Members_Member_Id]
GO
/****** Object:  ForeignKey [FK_dbo.LotteryResults_dbo.Members_Member_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[LotteryResults]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LotteryResults_dbo.Members_Member_Id] FOREIGN KEY([Member_Id])
REFERENCES [dbo].[Members] ([Id])
GO
ALTER TABLE [dbo].[LotteryResults] CHECK CONSTRAINT [FK_dbo.LotteryResults_dbo.Members_Member_Id]
GO
/****** Object:  ForeignKey [FK_dbo.LotteryResults_dbo.PrizeOrders_PrizeOrder_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[LotteryResults]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LotteryResults_dbo.PrizeOrders_PrizeOrder_Id] FOREIGN KEY([PrizeOrder_Id])
REFERENCES [dbo].[PrizeOrders] ([Id])
GO
ALTER TABLE [dbo].[LotteryResults] CHECK CONSTRAINT [FK_dbo.LotteryResults_dbo.PrizeOrders_PrizeOrder_Id]
GO
/****** Object:  ForeignKey [FK_dbo.MemberExtends_dbo.Members_Member_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[MemberExtends]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MemberExtends_dbo.Members_Member_Id] FOREIGN KEY([Member_Id])
REFERENCES [dbo].[Members] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MemberExtends] CHECK CONSTRAINT [FK_dbo.MemberExtends_dbo.Members_Member_Id]
GO
/****** Object:  ForeignKey [FK_dbo.PrizeBettings_dbo.Members_Member_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[PrizeBettings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PrizeBettings_dbo.Members_Member_Id] FOREIGN KEY([Member_Id])
REFERENCES [dbo].[Members] ([Id])
GO
ALTER TABLE [dbo].[PrizeBettings] CHECK CONSTRAINT [FK_dbo.PrizeBettings_dbo.Members_Member_Id]
GO
/****** Object:  ForeignKey [FK_dbo.PrizeBettings_dbo.PrizeOrders_PrizeOrder_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[PrizeBettings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PrizeBettings_dbo.PrizeOrders_PrizeOrder_Id] FOREIGN KEY([PrizeOrder_Id])
REFERENCES [dbo].[PrizeOrders] ([Id])
GO
ALTER TABLE [dbo].[PrizeBettings] CHECK CONSTRAINT [FK_dbo.PrizeBettings_dbo.PrizeOrders_PrizeOrder_Id]
GO
/****** Object:  ForeignKey [FK_dbo.PrizeOrderExtends_dbo.PrizeOrders_PrizeOrder_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[PrizeOrderExtends]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PrizeOrderExtends_dbo.PrizeOrders_PrizeOrder_Id] FOREIGN KEY([PrizeOrder_Id])
REFERENCES [dbo].[PrizeOrders] ([Id])
GO
ALTER TABLE [dbo].[PrizeOrderExtends] CHECK CONSTRAINT [FK_dbo.PrizeOrderExtends_dbo.PrizeOrders_PrizeOrder_Id]
GO
/****** Object:  ForeignKey [FK_dbo.PrizeOrders_dbo.Prizes_Prize_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[PrizeOrders]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PrizeOrders_dbo.Prizes_Prize_Id] FOREIGN KEY([Prize_Id])
REFERENCES [dbo].[Prizes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrizeOrders] CHECK CONSTRAINT [FK_dbo.PrizeOrders_dbo.Prizes_Prize_Id]
GO
/****** Object:  ForeignKey [FK_dbo.PrizePhotoes_dbo.Prizes_Prize_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[PrizePhotoes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PrizePhotoes_dbo.Prizes_Prize_Id] FOREIGN KEY([Prize_Id])
REFERENCES [dbo].[Prizes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrizePhotoes] CHECK CONSTRAINT [FK_dbo.PrizePhotoes_dbo.Prizes_Prize_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Prizes_dbo.Members_Member_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[Prizes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Prizes_dbo.Members_Member_Id] FOREIGN KEY([Member_Id])
REFERENCES [dbo].[Members] ([Id])
GO
ALTER TABLE [dbo].[Prizes] CHECK CONSTRAINT [FK_dbo.Prizes_dbo.Members_Member_Id]
GO
/****** Object:  ForeignKey [FK_dbo.RechargeHistories_dbo.Members_Member_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[RechargeHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RechargeHistories_dbo.Members_Member_Id] FOREIGN KEY([Member_Id])
REFERENCES [dbo].[Members] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RechargeHistories] CHECK CONSTRAINT [FK_dbo.RechargeHistories_dbo.Members_Member_Id]
GO
/****** Object:  ForeignKey [FK_dbo.RechargeHistories_dbo.Members_Operator_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[RechargeHistories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RechargeHistories_dbo.Members_Operator_Id] FOREIGN KEY([Operator_Id])
REFERENCES [dbo].[Members] ([Id])
GO
ALTER TABLE [dbo].[RechargeHistories] CHECK CONSTRAINT [FK_dbo.RechargeHistories_dbo.Members_Operator_Id]
GO
/****** Object:  ForeignKey [FK_dbo.RoleMembers_dbo.Members_Member_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[RoleMembers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RoleMembers_dbo.Members_Member_Id] FOREIGN KEY([Member_Id])
REFERENCES [dbo].[Members] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMembers] CHECK CONSTRAINT [FK_dbo.RoleMembers_dbo.Members_Member_Id]
GO
/****** Object:  ForeignKey [FK_dbo.RoleMembers_dbo.Roles_Role_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[RoleMembers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RoleMembers_dbo.Roles_Role_Id] FOREIGN KEY([Role_Id])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMembers] CHECK CONSTRAINT [FK_dbo.RoleMembers_dbo.Roles_Role_Id]
GO
/****** Object:  ForeignKey [FK_dbo.TopOrders_dbo.Members_Operator_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[TopOrders]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TopOrders_dbo.Members_Operator_Id] FOREIGN KEY([Operator_Id])
REFERENCES [dbo].[Members] ([Id])
GO
ALTER TABLE [dbo].[TopOrders] CHECK CONSTRAINT [FK_dbo.TopOrders_dbo.Members_Operator_Id]
GO
/****** Object:  ForeignKey [FK_dbo.TopOrders_dbo.PrizeOrders_PrizeOrder_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[TopOrders]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TopOrders_dbo.PrizeOrders_PrizeOrder_Id] FOREIGN KEY([PrizeOrder_Id])
REFERENCES [dbo].[PrizeOrders] ([Id])
GO
ALTER TABLE [dbo].[TopOrders] CHECK CONSTRAINT [FK_dbo.TopOrders_dbo.PrizeOrders_PrizeOrder_Id]
GO
/****** Object:  ForeignKey [FK_dbo.WhiteLists_dbo.Members_Member_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[WhiteLists]  WITH CHECK ADD  CONSTRAINT [FK_dbo.WhiteLists_dbo.Members_Member_Id] FOREIGN KEY([Member_Id])
REFERENCES [dbo].[Members] ([Id])
GO
ALTER TABLE [dbo].[WhiteLists] CHECK CONSTRAINT [FK_dbo.WhiteLists_dbo.Members_Member_Id]
GO
/****** Object:  ForeignKey [FK_dbo.WhiteLists_dbo.PrizeOrders_PrizeOrder_Id]    Script Date: 12/09/2014 15:32:58 ******/
ALTER TABLE [dbo].[WhiteLists]  WITH CHECK ADD  CONSTRAINT [FK_dbo.WhiteLists_dbo.PrizeOrders_PrizeOrder_Id] FOREIGN KEY([PrizeOrder_Id])
REFERENCES [dbo].[PrizeOrders] ([Id])
GO
ALTER TABLE [dbo].[WhiteLists] CHECK CONSTRAINT [FK_dbo.WhiteLists_dbo.PrizeOrders_PrizeOrder_Id]
GO
