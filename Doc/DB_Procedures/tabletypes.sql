USE [LotteryDraw]
GO
/****** Object:  UserDefinedTableType [dbo].[RevealOrdersTableType]    Script Date: 12/08/2014 14:20:36 ******/
CREATE TYPE [dbo].[RevealOrdersTableType] AS TABLE(
	[RowNum] [int] NULL,
	[PrizeOrderId] [varchar](100) NULL,
	[RevealType] [char](1) NULL
)
GO
