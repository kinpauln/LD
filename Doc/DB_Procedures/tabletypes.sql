USE [LotteryDraw]
GO
/****** Object:  UserDefinedTableType [dbo].[RevealOrdersTableType]    Script Date: 12/04/2014 10:18:07 ******/
CREATE TYPE [dbo].[RevealOrdersTableType] AS TABLE(
	[RowNum] [int] NULL,
	[PrizeOrderId] [varchar](100) NULL,
	[RevealType] [char](1) NULL
)
GO
