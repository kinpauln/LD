USE [LotteryDraw]
GO
/****** Object:  UserDefinedTableType [dbo].[RevealOrdersTableType]    Script Date: 10/29/2014 11:27:33 ******/
CREATE TYPE [dbo].[RevealOrdersTableType] AS TABLE(
	[RowNum] [int] NULL,
	[PrizeOrderId] [varchar](100) NULL
)
GO
