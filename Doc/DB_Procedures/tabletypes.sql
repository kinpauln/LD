USE [LotteryDraw]
GO
/****** Object:  UserDefinedTableType [dbo].[RevealOrdersTableType]    Script Date: 10/29/2014 12:08:38 ******/
CREATE TYPE [dbo].[RevealOrdersTableType] AS TABLE(
	[RowNum] [int] NULL,
	[PrizeOrderId] [varchar](100) NULL
)
GO
