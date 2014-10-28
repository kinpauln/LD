USE [LotteryDraw]
GO
/****** Object:  UserDefinedTableType [dbo].[RevealOrdersTableType]    Script Date: 10/28/2014 17:38:11 ******/
CREATE TYPE [dbo].[RevealOrdersTableType] AS TABLE(
	[RowNum] [int] NULL,
	[PrizeOrderId] [varchar](100) NULL
)
GO

USE [LotteryDraw]
GO
/****** Object:  StoredProcedure [dbo].[sp_getTopPrizeOrders]    Script Date: 10/28/2014 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_getTopPrizeOrders]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select po.*,p.Description as PrizeDescription,p.Name as PrizeName from dbo.PrizeOrders po
left join Prizes p on po.Prize_Id=p.Id
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetBettingCount]    Script Date: 10/28/2014 17:38:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetBettingCount]
	(@prizeOrderId VARCHAR(100))
RETURNS int
AS
BEGIN
declare @bettingCount int

set @bettingCount=0
	select @bettingCount = COUNT(*) from PrizeBettings pb where pb.PrizeOrder_Id=@prizeOrderId and pb.IsDeleted=0
	
	RETURN @bettingCount

END
GO
/****** Object:  StoredProcedure [dbo].[sp_revealTimingLottery]    Script Date: 10/28/2014 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_revealTimingLottery] @RevealOrders    [REVEALORDERSTABLETYPE] readonly,
                                               @revealedCount   INT output,
                                               @succeededOrders VARCHAR(max) output,
                                               @failedOrders    VARCHAR(max) output,
                                               @errorString     VARCHAR(max) output
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      DECLARE @Row            INT,
              @Rows           INT,
              @LuckyCount     INT,
              @WhiteListCount INT,
              @MinLuckyCount  INT,
              @BettingCount   INT,
              @PrizeOrderId   VARCHAR(100),
              @LuckyPercent   INT,
              @PoolCount      INT

      SET @succeededOrders = ''
      SET @failedOrders =''
      SET @errorString = ''

      --SET @Rows = @@ROWCOUNT
      SELECT @Rows = Count(*)
      FROM   @RevealOrders

      SET @revealedCount = @Rows

      IF( @Rows = 0 )
        BEGIN
            RETURN
        END

      SET @Row = 1

      --遍历要开奖的奖单（ID）
      WHILE ( @Row <= @Rows )
        BEGIN
            SELECT @PrizeOrderId = PrizeOrderId
            FROM   @RevealOrders
            WHERE  RowNum = @Row

            --取得奖单相关配置信息
            SELECT @LuckyCount = LuckyCount,
                   --@WhiteListCount=,
                   @MinLuckyCount = MinLuckyCount,
                   @LuckyPercent = LuckyPercent
            FROM   PrizeOrderExtends
            WHERE  PrizeOrder_Id = @PrizeOrderId

            --设置当前奖单投注者的数目 
            SELECT @BettingCount = Count(*)
            FROM   PrizeBettings
            WHERE  PrizeOrder_Id = @PrizeOrderId

            IF @BettingCount < @LuckyCount
              BEGIN
                  SET @errorString = '奖单投注者总数小于所设置的中奖人数,中止此奖单的开奖（奖单ID:'
                                     + @PrizeOrderId + ')'
                  SET @failedOrders += @PrizeOrderId + ','

				--更新奖单状态为开奖失败
				UPDATE PrizeOrders
				SET    RevealStateNum = 5,
					   RevealState = 5
				WHERE  [id] = @PrizeOrderId
				  SET @Row = @Row + 1
                  CONTINUE
              END

            --白名单中的数目
            SELECT @WhiteListCount = Count(*)
            FROM   dbo.WhiteLists
            WHERE  PrizeOrder_Id = @PrizeOrderId

            BEGIN TRANSACTION--开始事务
            DECLARE @errorSun INT --定义错误计数器
            SET @errorSun=0 --没错为0

            --白名单中的都中奖
            INSERT INTO dbo.LotteryResults
            SELECT Newid(),
                   0,
                   Getdate(),
                   Member_Id,
                   PrizeOrder_Id
            FROM   WhiteLists
            WHERE  PrizeOrder_Id = @PrizeOrderId

            SET @errorSun=@errorSun + @@ERROR --累计是否有错

            --更新奖单状态为已开奖
            UPDATE PrizeOrders
            SET    RevealStateNum = 3,
                   RevealState = 3
            WHERE  [id] = @PrizeOrderId

            SET @errorSun=@errorSun + @@ERROR --累计是否有错

            --应中奖人数
            DECLARE @LuckCountTrue INT

            SET @LuckCountTrue = @LuckyCount - @WhiteListCount

            IF @LuckCountTrue > 0
              BEGIN
                  --动态sql文，随机抽取@LuckCountTrue位中奖者
                  DECLARE @sqlstr VARCHAR(max)

                  SET @sqlstr = 'select top '
                                + Cast(@LuckCountTrue AS VARCHAR(20))
                                + ' Member_Id from PrizeBettings WHERE  PrizeOrder_Id ='''
                                + @PrizeOrderId + ''' order by newid()'

                  --创建临时表，存储中奖者
                  CREATE TABLE #tb
                    (
                       MemberId INT
                    )

                  --将中奖者存入临时表
                  INSERT INTO #tb
                  EXEC(@sqlstr)

                  INSERT INTO dbo.LotteryResults
                  SELECT Newid(),
                         0,
                         Getdate(),
                         MemberId,
                         @PrizeOrderId
                  FROM   #tb

                  SET @errorSun=@errorSun + @@ERROR --累计是否有错
                  IF @errorSun <> 0
                    BEGIN
                        SET @failedOrders += @PrizeOrderId + ','

                        --PRINT '本次定员开奖有错误，事务回滚'
                        --      + ',奖单ID:' + @PrizeOrderId
                        ROLLBACK TRANSACTION--事务回滚语句
                    END
                  ELSE
                    BEGIN
                        --PRINT '本次定员开奖成功，事务提交'
                        --      + ',奖单ID:' + @PrizeOrderId
                        SET @succeededOrders += @PrizeOrderId + ','

                        COMMIT TRANSACTION--事务提交语句
                    END

                  DROP TABLE #tb
              END

            SET @Row = @Row + 1
        END
  END
GO
/****** Object:  StoredProcedure [dbo].[sp_revealQuotaLottery]    Script Date: 10/28/2014 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,王金鹏>
-- Create date: <Create Date,2014-10-21>
-- Description:	<Description,定员开奖>
-- =============================================
CREATE PROCEDURE [dbo].[sp_revealQuotaLottery] 
@RevealOrders [RevealOrdersTableType] readonly,
@revealedCount   INT output,
                                              @succeededOrders VARCHAR(max) output,
                                              @failedOrders    VARCHAR(max) output,
                                              @errorString     VARCHAR(max) output
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      DECLARE @Row            INT,
              @Rows           INT,
              @LuckyCount     INT,
              @WhiteListCount INT,
              @MinLuckyCount  INT,
              @BettingCount   INT,
              @PrizeOrderId   VARCHAR(100),
              @LuckyPercent   INT,
              @PoolCount      INT

      SET @succeededOrders = ''
      SET @failedOrders =''
      SET @errorString = ''
      

      --SET @Rows = @@ROWCOUNT
      select @Rows = COUNT(*) from @RevealOrders
      SET @revealedCount = @Rows

      IF( @Rows = 0 )
        BEGIN
            RETURN
        END

      SET @Row = 1

      --遍历要开奖的奖单（ID）
      WHILE ( @Row <= @Rows )
        BEGIN
            SELECT @PrizeOrderId = PrizeOrderId
            FROM   @RevealOrders
            WHERE  RowNum = @Row

            --取得奖单相关配置信息
            SELECT @LuckyCount = LuckyCount,
                   --@WhiteListCount=,
                   @MinLuckyCount = MinLuckyCount,
                   @LuckyPercent = LuckyPercent
            FROM   PrizeOrderExtends
            WHERE  PrizeOrder_Id = @PrizeOrderId

            --设置当前奖单投注者的数目 
            SELECT @BettingCount = Count(*)
            FROM   PrizeBettings
            WHERE  PrizeOrder_Id = @PrizeOrderId

            --白名单中的数目
            SELECT @WhiteListCount = Count(*)
            FROM   dbo.WhiteLists
            WHERE  PrizeOrder_Id = @PrizeOrderId

            BEGIN TRANSACTION--开始事务

            DECLARE @errorSun INT --定义错误计数器

            SET @errorSun=0 --没错为0
            --白名单中的都中奖
            INSERT INTO dbo.LotteryResults
            SELECT Newid(),
                   0,
                   Getdate(),
                   Member_Id,
                   PrizeOrder_Id
            FROM   WhiteLists
            WHERE  PrizeOrder_Id = @PrizeOrderId

            SET @errorSun=@errorSun + @@ERROR --累计是否有错
            --更新奖单状态为已开奖
            UPDATE PrizeOrders
            SET    RevealStateNum = 3,
                   RevealState = 3
            WHERE  [id] = @PrizeOrderId

            SET @errorSun=@errorSun + @@ERROR --累计是否有错
            --应中奖人数
            DECLARE @LuckCountTrue INT

            SET @LuckCountTrue = @LuckyCount - @WhiteListCount

            IF @LuckCountTrue > 0
              BEGIN
                  --动态sql文，随机抽取@LuckCountTrue位中奖者
                  DECLARE @sqlstr VARCHAR(max)

                  SET @sqlstr = 'select top '
                                + Cast(@LuckCountTrue AS VARCHAR(20))
                                + ' Member_Id from PrizeBettings WHERE  PrizeOrder_Id ='''
                                + @PrizeOrderId + ''' order by newid()'

                  --创建临时表，存储中奖者
                  CREATE TABLE #tb
                    (
                       MemberId INT
                    )

                  --将中奖者存入临时表
                  INSERT INTO #tb
                  EXEC(@sqlstr)

                  INSERT INTO dbo.LotteryResults
                  SELECT Newid(),
                         0,
                         Getdate(),
                         MemberId,
                         @PrizeOrderId
                  FROM   #tb

                  SET @errorSun=@errorSun + @@ERROR --累计是否有错

                  IF @errorSun <> 0
                    BEGIN
                        SET @failedOrders += @PrizeOrderId + ','

                        --PRINT '本次定员开奖有错误，事务回滚'
                        --      + ',奖单ID:' + @PrizeOrderId

                        ROLLBACK TRANSACTION--事务回滚语句
                    END
                  ELSE
                    BEGIN
                        --PRINT '本次定员开奖成功，事务提交'
                        --      + ',奖单ID:' + @PrizeOrderId

                        SET @succeededOrders += @PrizeOrderId + ','

                        COMMIT TRANSACTION--事务提交语句
                    END

                  DROP TABLE #tb
              END

            SET @Row = @Row + 1
        END
  END
  
  
--update PrizeOrders set RevealState=1,RevealStateNum=1 

--declare @errorString varchar(max)
--exec dbo.sp_revealLottery @errorString output
--print @errorString
GO
/****** Object:  StoredProcedure [dbo].[sp_revealAnswerLottery]    Script Date: 10/28/2014 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_revealAnswerLottery] @interval    INT,
                                               @errorString VARCHAR(max) output
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      CREATE TABLE #result_answer
        (
           RevealCount     INT,
           SucceededOrders VARCHAR(max),
           FailedOrders    VARCHAR(max)
        )

      -- =======================================================
      -- 定员开奖方式
      -- =======================================================
      DECLARE @quotaOrders REVEALORDERSTABLETYPE

      INSERT INTO @quotaOrders
      SELECT Row_number()
               OVER(
                 ORDER BY po.SortOrder DESC) AS rownum,
             po.Id                           AS PrizeOrderId
      FROM   dbo.PrizeOrders po
             LEFT JOIN dbo.PrizeOrderExtends poe
                    ON po.Id = poe.PrizeOrder_Id
      WHERE  RevealState = 1 --未开奖
             AND po.IsDeleted = 0
             AND po.RevealTypeNum = 3 --答案开奖开奖
             AND poe.AnswerRevealConditionTypeNum = 2 --定员开奖方式
             AND dbo.Getbettingcount(po.Id) >= poe.PoolCount

      --调用定员开奖存储过程
      DECLARE @revealedCount_quota   INT,
              @succeededOrders_quota VARCHAR(max),--开奖成功的
              @failedOrders_quota    VARCHAR(max),--开奖未成功的
              @errorString_quota     VARCHAR(max)

      SET @errorString = ''
      SET @errorString_quota = ''

      EXEC dbo.Sp_revealquotalottery
        @quotaOrders,
        @revealedCount_quota output,
        @succeededOrders_quota output,
        @failedOrders_quota output,
        @errorString_quota output

      SET @errorString += @errorString_quota

      -- =======================================================
      -- 定时开奖方式
      -- =======================================================
      DECLARE @timingOrders REVEALORDERSTABLETYPE

      INSERT INTO @timingOrders
      SELECT Row_number()
               OVER(
                 ORDER BY po.SortOrder DESC) AS rownum,
             po.Id                           AS PrizeOrderId
      FROM   dbo.PrizeOrders po
             LEFT JOIN dbo.PrizeOrderExtends poe
                    ON po.Id = poe.PrizeOrder_Id
      WHERE  RevealState = 1 --未开奖
             AND po.IsDeleted = 0
             AND po.RevealTypeNum = 3 --答案开奖开奖
             AND poe.AnswerRevealConditionTypeNum = 1 --定时开奖方式
             AND poe.LaunchTime > Dateadd(ss, -@interval, Getdate())
             AND poe.LaunchTime <= Getdate()

      --AND dbo.Getbettingcount(po.Id) >= poe.PoolCount
      --调用定员开奖存储过程
      DECLARE @revealedCount_timing   INT,
              @succeededOrders_timing VARCHAR(max),--开奖成功的
              @failedOrders_timing    VARCHAR(max),--开奖未成功的
              @errorString_timing     VARCHAR(max)

      SET @errorString_timing = ''

      EXEC dbo.Sp_revealtiminglottery
        @timingOrders,
        @revealedCount_timing output,
        @succeededOrders_timing output,
        @failedOrders_timing output,
        @errorString_timing output

      INSERT INTO #result_answer
      VALUES      (@revealedCount_quota
                   + @revealedCount_timing,
                   @succeededOrders_quota
                   + @succeededOrders_timing,
                   @failedOrders_quota + @failedOrders_timing)

      SET @errorString += @errorString_timing

      SELECT *
      FROM   #result_answer

      DROP TABLE #result_answer
  END
GO
/****** Object:  StoredProcedure [dbo].[sp_revealLottery]    Script Date: 10/28/2014 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,wangjp>
-- Create date: <Create Date,2014-10-21>
-- Description:	<Description,开奖>
-- =============================================
CREATE PROCEDURE [dbo].[sp_revealLottery] @interval    INT,
                                         @errorString VARCHAR(max) output
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
      SET @errorString = ''

      CREATE TABLE #result
        (
           RevealCount     INT,
           SucceededOrders VARCHAR(max),
           FailedOrders    VARCHAR(max)
        )

      -- =======================================================
      -- 定时开奖
      -- =======================================================
      DECLARE @timingOrders REVEALORDERSTABLETYPE

      INSERT INTO @timingOrders
      SELECT Row_number()
               OVER(
                 ORDER BY po.SortOrder DESC) AS RowNum,
             po.Id                           AS PrizeOrderId
      FROM   dbo.PrizeOrders po
             LEFT JOIN dbo.PrizeOrderExtends poe
                    ON po.Id = poe.PrizeOrder_Id
      WHERE  RevealState = 1 --未开奖
             AND po.IsDeleted = 0
             AND po.RevealTypeNum = 1 --定时开奖
             AND poe.LaunchTime > Dateadd(ss, -@interval, Getdate())
             AND poe.LaunchTime <= Getdate()

      DECLARE @revealedCount_timing   INT,
              @succeededOrders_timing VARCHAR(max),--开奖成功的
              @failedOrders_timing    VARCHAR(max),--开奖未成功的
              @errorString_timing     VARCHAR(max)

      EXEC dbo.Sp_revealtiminglottery
        @timingOrders,
        @revealedCount_timing output,
        @succeededOrders_timing output,
        @failedOrders_timing output,
        @errorString_timing output

      INSERT INTO #result
      VALUES      (@revealedCount_timing,
                   @succeededOrders_timing,
                   @failedOrders_timing)

      SET @errorString += @errorString_timing
      SELECT *
      FROM   #result

      -- =======================================================
      -- 定员开奖
      -- =======================================================
      DECLARE @quotaOrders REVEALORDERSTABLETYPE

      INSERT INTO @quotaOrders
      SELECT Row_number()
               OVER(
                 ORDER BY po.SortOrder DESC) AS RowNum,
             po.Id                           AS PrizeOrderId
      FROM   dbo.PrizeOrders po
             LEFT JOIN dbo.PrizeOrderExtends poe
                    ON po.Id = poe.PrizeOrder_Id
      WHERE  RevealState = 1 --未开奖
             AND po.IsDeleted = 0
             AND po.RevealTypeNum = 2 --定员开奖
             AND dbo.Getbettingcount(po.Id) >= poe.PoolCount

      --调用存储过程
      DECLARE @revealedCount_quota   INT,
              @succeededOrders_quota VARCHAR(max),--开奖成功的
              @failedOrders_quota    VARCHAR(max),--开奖未成功的
              @errorString_quota     VARCHAR(max)

      EXEC dbo.Sp_revealquotalottery
        @quotaOrders,
        @revealedCount_quota output,
        @succeededOrders_quota output,
        @failedOrders_quota output,
        @errorString_quota output

      TRUNCATE TABLE #result

      INSERT INTO #result
      VALUES      (@revealedCount_quota,
                   @succeededOrders_quota,
                   @failedOrders_quota)

      SET @errorString += @errorString_quota
      SELECT *
      FROM   #result

      DROP TABLE #result


      -- =======================================================
      -- 答案开奖
      -- =======================================================
      DECLARE @errorString_answer VARCHAR(max)

      EXEC Sp_revealanswerlottery
        @interval,
        @errorString_answer output

      SET @errorString += @errorString_answer
  END
--update PrizeOrders set RevealState=1,RevealStateNum=1 
--declare @interval int
--set @interval=60000
--declare @errorString varchar(max)
--exec dbo.sp_revealLottery @interval,@errorString output
--print @errorString
GO