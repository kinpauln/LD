USE [LotteryDraw]
GO
/****** Object:  UserDefinedTableType [dbo].[RevealOrdersTableType]    Script Date: 11/26/2014 11:35:30 ******/
CREATE TYPE [dbo].[RevealOrdersTableType] AS TABLE(
	[RowNum] [int] NULL,
	[PrizeOrderId] [varchar](100) NULL,
	[RevealType] [char](1) NULL
)
GO
