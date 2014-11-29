USE [LotteryDraw]
GO
/****** Object:  StoredProcedure [dbo].[sp_viewPage]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [dbo].[sp_viewPage]         
    /*          
    nzperfect [no_mIss] 高效通用分页存储过程(双向检索)        
    敬告：适用于单一主键或存在唯一值列的表或视图          
    ps:Sql语句为8000字节,调用时请注意传入参数及sql总长度不要超过指定范围          
    */          
    @TableName VARCHAR(200),     --表名          
    @FieldList VARCHAR(2000),    --显示列名，如果是全部字段则为*          
    @PrimaryKey VARCHAR(100),    --单一主键或唯一值键          
    @Where VARCHAR(2000),        --查询条件 不含'where'字符，如id>10 and len(userid)>9          
    @Order VARCHAR(1000),        --排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
    --注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷          
    @SortType INT,               --排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
    @RecorderCount INT,          --记录总数 0:会返回总记录          
    @PageSize INT,               --每页输出的记录数          
    @PageIndex INT,              --当前页数          
    @TotalCount INT OUTPUT ,      --记返回总记录          
    @TotalPageCount INT OUTPUT   --返回总页数          
AS          
SET NOCOUNT ON          
    IF ISNULL(@TotalCount,'') = '' SET @TotalCount = 0          
    SET @Order = RTRIM(LTRIM(@Order))          
    SET @PrimaryKey = RTRIM(LTRIM(@PrimaryKey))          
    SET @FieldList = REPLACE(RTRIM(LTRIM(@FieldList)),' ','')          
    WHILE CHARINDEX(', ',@Order) > 0 or CHARINDEX(' ,',@Order) > 0          
        BEGIN          
            SET @Order = REPLACE(@Order,', ',',')          
            SET @Order = REPLACE(@Order,' ,',',')          
        END          
    IF ISNULL(@TableName,'') = '' or ISNULL(@FieldList,'') = ''          
            or ISNULL(@PrimaryKey,'') = ''          
            or @SortType < 1 or @SortType >3          
            or @RecorderCount  < 0 or @PageSize < 0 or @PageIndex < 0          
        BEGIN          
            PRINT('ERR_00')          
            RETURN          
        END          
    IF @SortType = 3          
        BEGIN
        print @order          
            IF (UPPER(RIGHT(@Order,4))!=' ASC' AND UPPER(RIGHT(@Order,5))!=' DESC')          
                BEGIN print(UPPER(RIGHT(@Order,4))) PRINT('ERR_02') RETURN END          
        END          
    DECLARE @new_where1 VARCHAR(1000)          
    DECLARE @new_where2 VARCHAR(1000)          
    DECLARE @new_order1 VARCHAR(1000)          
    DECLARE @new_order2 VARCHAR(1000)          
    DECLARE @new_order3 VARCHAR(1000)          
    DECLARE @Sql VARCHAR(8000)          
    DECLARE @SqlCount NVARCHAR(4000)          
    IF ISNULL(@where,'') = ''          
        BEGIN          
            SET @new_where1 = ' '          
            SET @new_where2 = ' Where  '          
        END          
    ELSE          
        BEGIN          
            SET @new_where1 = ' Where ' + @where          
            SET @new_where2 = ' Where ' + @where + ' AND '          
        END          
    IF ISNULL(@order,'') = '' or @SortType = 1  or @SortType = 2          
        BEGIN          
            IF @SortType = 1          
                BEGIN          
                    SET @new_order1 = ' orDER BY ' + @PrimaryKey + ' ASC'          
                    SET @new_order2 = ' orDER BY ' + @PrimaryKey + ' DESC'          
                END          
            IF @SortType = 2          
                BEGIN          
                    SET @new_order1 = ' orDER BY ' + @PrimaryKey + ' DESC'          
                    SET @new_order2 = ' orDER BY ' + @PrimaryKey + ' ASC'          
                END          
        END          
    ELSE          
        BEGIN          
            SET @new_order1 = ' orDER BY ' + @Order          
        END          
    
    IF @SortType = 3 AND  CHARINDEX(','+@PrimaryKey+' ',','+@Order)>0          
    BEGIN          
        SET @new_order1 = ' orDER BY ' + @Order          
        SET @new_order2 = @Order + ','          
        SET @new_order2 = REPLACE(REPLACE(@new_order2,'ASC,','{ASC},'),'DESC,','{DESC},')          
        SET @new_order2 = REPLACE(REPLACE(@new_order2,'{ASC},','DESC,'),'{DESC},','ASC,')          
        SET @new_order2 = ' orDER BY ' + SUBSTRING(@new_order2,1,LEN(@new_order2)-1)          
        IF @FieldList <> '*'          
            BEGIN          
                SET @new_order3 = REPLACE(REPLACE(@Order + ',','ASC,',','),'DESC,',',')          
                SET @FieldList = ',' + @FieldList          
                WHILE CHARINDEX(',',@new_order3)>0          
                    BEGIN          
                        IF CHARINDEX(SUBSTRING(','+@new_order3,1,CHARINDEX(',',@new_order3)),','+@FieldList+',')>0          
                            BEGIN          
                                SET @FieldList =          
                                @FieldList + ',' + SUBSTRING(@new_order3,1,CHARINDEX(',',@new_order3))          
                            END          
                        SET @new_order3 =          
                        SUBSTRING(@new_order3,CHARINDEX(',',@new_order3)+1,LEN(@new_order3))          
                    END          
                SET @FieldList = SUBSTRING(@FieldList,2,LEN(@FieldList))          
            END          
        END     
         
    SET @SqlCount = 'Select @TotalCount=COUNT(*),@TotalPageCount=CEILING((COUNT(*)+0.0)/'          
    + CAST(@PageSize AS VARCHAR)+') FROM (Select * FROM ' + @TableName + @new_where1+') AS T'          
    IF @RecorderCount  = 0          
        BEGIN          
            EXEC SP_EXECUTESQL @SqlCount,N'@TotalCount INT OUTPUT,@TotalPageCount INT OUTPUT',          
            @TotalCount OUTPUT,@TotalPageCount OUTPUT          
        END          
    ELSE          
        BEGIN          
            Select @TotalCount = @RecorderCount        
        END          
    IF @PageIndex > CEILING((@TotalCount+0.0)/@PageSize)          
        BEGIN          
            SET @PageIndex =  CEILING((@TotalCount+0.0)/@PageSize)          
        END          
    IF @PageIndex = 1 or @PageIndex >= CEILING((@TotalCount+0.0)/@PageSize)          
        BEGIN          
            IF @PageIndex = 1 --返回第一页数据          
                BEGIN          
                    SET @Sql = 'Select * FROM (Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM '          
                    + @TableName + @new_where1 + @new_order1 +') AS TMP ' + @new_order1    
                END          
            IF @PageIndex >= CEILING((@TotalCount+0.0)/@PageSize)  --返回最后一页数据          
                BEGIN          
                    SET @Sql = 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM ('          
                    + 'Select TOP ' + STR(ABS(@PageSize*@PageIndex-@TotalCount-@PageSize))          
                    + ' ' + @FieldList + ' FROM '          
                    + @TableName + @new_where1 + @new_order2 + ' ) AS TMP '          
                    + @new_order1          
                END          
        END          
    ELSE      
            
        BEGIN          
        IF @SortType = 1  --仅主键正序排序          
            BEGIN          
                IF @PageIndex <= CEILING((@TotalCount+0.0)/@PageSize)/2  --正向检索          
                    BEGIN          
                        SET @Sql = 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM '          
                        + @TableName + @new_where2 + @PrimaryKey + ' > '          
                        + '(Select MAX(' + @PrimaryKey + ') FROM (Select TOP '          
                        + STR(@PageSize*(@PageIndex-1)) + ' ' + @PrimaryKey          
                        + ' FROM ' + @TableName          
                        + @new_where1 + @new_order1 +' ) AS TMP) '+ @new_order1          
                    END          
                ELSE  --反向检索          
                    BEGIN          
                        SET @Sql = 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM ('          
                        + 'Select TOP ' + STR(@PageSize) + ' '          
                        + @FieldList + ' FROM '          
                        + @TableName + @new_where2 + @PrimaryKey + ' < '          
                        + '(Select MIN(' + @PrimaryKey + ') FROM (Select TOP '         
                        + STR(@TotalCount-@PageSize*@PageIndex) + ' ' + @PrimaryKey          
                        + ' FROM ' + @TableName          
                        + @new_where1 + @new_order2 +' ) AS TMP) '+ @new_order2          
                        + ' ) AS TMP ' + @new_order1          
                    END          
            END          
        IF @SortType = 2  --仅主键反序排序          
            BEGIN          
                IF @PageIndex <= CEILING((@TotalCount+0.0)/@PageSize)/2  --正向检索          
                    BEGIN          
                        SET @Sql = 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM '          
                        + @TableName + @new_where2 + @PrimaryKey + ' < '          
                        + '(Select MIN(' + @PrimaryKey + ') FROM (Select TOP '          
                        + STR(@PageSize*(@PageIndex-1)) + ' ' + @PrimaryKey          
                        +' FROM '+ @TableName          
                        + @new_where1 + @new_order1 + ') AS TMP) '+ @new_order1          
                    END          
                ELSE  --反向检索          
                    BEGIN          
                        SET @Sql = 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM ('          
                        + 'Select TOP ' + STR(@PageSize) + ' '          
                        + @FieldList + ' FROM '          
                        + @TableName + @new_where2 + @PrimaryKey + ' > '          
                        + '(Select MAX(' + @PrimaryKey + ') FROM (Select TOP '          
                        + STR(@TotalCount-@PageSize*@PageIndex) + ' ' + @PrimaryKey          
                        + ' FROM ' + @TableName          
                        + @new_where1 + @new_order2 +' ) AS TMP) '+ @new_order2          
                        + ' ) AS TMP ' + @new_order1          
                    END          
            END          
        IF @SortType = 3  --多列排序，必须包含主键，且放置最后，否则不处理          
            BEGIN          
                IF CHARINDEX(',' + @PrimaryKey + ' ',',' + @Order) = 0          
                    BEGIN PRINT('ERR_02') RETURN END          
                    IF @PageIndex <= CEILING((@TotalCount+0.0)/@PageSize)/2  --正向检索          
                        BEGIN          
                            SET @Sql = 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM ( '          
                            + 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM ( '          
                            + ' Select TOP ' + STR(@PageSize*@PageIndex) + ' ' + @FieldList          
                            + ' FROM ' + @TableName + @new_where1 + @new_order1 + ' ) AS TMP '          
                            + @new_order2 + ' ) AS TMP ' + @new_order1          
                        END          
                    ELSE  --反向检索          
                        BEGIN          
                            SET @Sql = 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM ( '          
                            + 'Select TOP ' + STR(@PageSize) + ' ' + @FieldList + ' FROM ( '          
                            + ' Select TOP ' + STR(@TotalCount-@PageSize *@PageIndex+@PageSize) + ' ' + @FieldList          
                            + ' FROM ' + @TableName + @new_where1 + @new_order2 + ' ) AS TMP '          
                            + @new_order1 + ' ) AS TMP ' + @new_order1          
                        END          
            END          
        END          
    PRINT(@SQL)          
    EXEC(@Sql)
GO
/****** Object:  StoredProcedure [dbo].[sp_set2Top]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_set2Top]
@PrizeOrderId   VARCHAR(100),--奖单ID
@ErrorCode      VARCHAR(10) output --错误代码
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ErrorCode=''
END
GO
/****** Object:  StoredProcedure [dbo].[sp_rechangfieldtype]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_rechangfieldtype](@typename varchar(50), @newtype varchar(50))
as
begin
declare @typeid int
declare @tablename varchar(50)
declare @column varchar(50)

declare @sqlstr varchar(200)
declare @defaultid int


select @typeid = xusertype
 from systypes
  where name = @typename and xusertype > 256
  AND (is_member('db_owner') = 1 OR is_member('db_ddladmin') = 1 OR is_member(user_name(uid))=1)

declare mycursor cursor for
select o.name, c.name, c.cdefault
from syscolumns c, systypes t, sysusers u, sysobjects o
where c.xusertype = @typeid
 and t.xusertype = @typeid
 and o.uid = u.uid
 and c.id = o.id
  and o.type = 'u'

open mycursor
fetch next from mycursor into @tablename, @column, @defaultid
while @@fetch_status = 0
begin
  if @defaultid <> 0
  begin
    set @sqlstr = 'alter table ' + @tablename + ' drop ' + object_name(@defaultid)
    exec(@sqlstr)

    set @sqlstr = 'alter table ' + @tablename + ' alter column ' + @column + ' ' + @newtype 
    exec(@sqlstr)
    
--    set @sqlstr = 'alter table ' + @tablename + ' add contraint ' + @tablename + 'df'+@column + ' default 0'

  end
  else
  begin
    set @sqlstr = 'alter table ' + @tablename + ' alter column ' + @column + ' ' + @newtype

    print @sqlstr
    exec(@sqlstr)
  end
  --if @@error <> 0
  --  continue
  fetch next from mycursor into @tablename, @column, @defaultid
end
--如果没有约束，则可以直接删除。如果有约束。先处理约束。

close mycursor
deallocate mycursor

end
GO
/****** Object:  StoredProcedure [dbo].[sp_getTopPrizeOrders]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_getTopPrizeOrders] @topCount   INT = 10,
                                             @revealType INT=NULL
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      DECLARE @sqlstring   NVARCHAR(max),
              @wherestring NVARCHAR(max)=''

      IF @revealType > 0
        BEGIN
            SET @wherestring=' where po.RevealType='
                             + Cast(@revealType AS VARCHAR(10))
        END

      SET @sqlstring = 'select top '
                       + Cast(@topCount AS VARCHAR(10))
                       + ' po.*,po.AddDate as RaiseTime,p.Description as PrizeDescription,p.Name as PrizeName,m.*,me.* from dbo.PrizeOrders po
left join Prizes p on po.Prize_Id=p.Id left join Members m on p.Member_Id = m.Id left join MemberExtends me on m.Id = me.Member_Id '
                       + @wherestring + ' order by po.SortOrder'

      PRINT @sqlstring

      EXEC (@sqlstring)
  END
GO
/****** Object:  StoredProcedure [dbo].[sp_getLotteries]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_getLotteries] @RevealType     INT=0,--开奖类型
                                        @RevealState    INT=0,--奖单状态
                                        @PageSize       INT = 10,--每页输出的记录数          
                                        @PageIndex      INT = 1,--当前页数
                                        @Order          VARCHAR(1000),--排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
                                        @Where          VARCHAR(2000) = null,--查询条件 不含'where'字符，如id>10 and len(userid)>9
                                        @TotalCount     INT output,--记返回总记录          
                                        @TotalPageCount INT OUTPUT --返回总页数 
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      DECLARE @TableName     VARCHAR(200),--表名          
              @FieldList     VARCHAR(2000),--显示列名，如果是全部字段则为*          
              @PrimaryKey    VARCHAR(100),--单一主键或唯一值键          
              --@Where         VARCHAR(2000),--查询条件 不含'where'字符，如id>10 and len(userid)>9          
              --@Order VARCHAR(1000),        --排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
              --注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷          
              @SortType      INT,--排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
              @RecorderCount INT --记录总数 0:会返回总记录 
      SET @TableName='View_Lotteries' --表名          
      SET @FieldList ='*' --显示列名，如果是全部字段则为*          
      SET @PrimaryKey ='PrizeOrderId' --单一主键或唯一值键
      
      IF Isnull(@Where, '') = ''
      begin 
      SET @Where ='1=1'
      end
      
      IF Isnull(@RevealType, '') = ''
        BEGIN
            SET @RevealType=0
        END

      IF @RevealType > 0
        BEGIN
            SET @Where +=' and RevealType='
                         + Cast(@RevealType AS CHAR(1))
        END

      IF Isnull(@RevealState, '') = ''
        BEGIN
            SET @RevealState=0
        END

      IF @RevealState > 0
        BEGIN
            SET @Where +=' and RevealState='
                         + Cast(@RevealState AS CHAR(1))
        END

      PRINT @where

      IF Isnull(@Order, '') <> ''
        BEGIN
            SET @Order =@Order + ',PrizeOrderId asc' --排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
        END
      ELSE
        BEGIN
            SET @Order = 'PrizeOrderId asc'
        END

      --注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷          
      SET @SortType =3 --排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
      SET @RecorderCount = 0 --记录总数 0:会返回总记录  
      EXEC [Sp_viewpage]
        @TableName,--表名          
        @FieldList,--显示列名，如果是全部字段则为*          
        @PrimaryKey,--单一主键或唯一值键          
        @Where,--查询条件 不含'where'字符，如id>10 and len(userid)>9          
        @Order,--排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
        --注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷          
        @SortType,--排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
        @RecorderCount,--记录总数 0:会返回总记录          
        @PageSize,--每页输出的记录数          
        @PageIndex,
        @TotalCount output,
        @TotalPageCount output
  END

--declare @RevealType int
--declare @RevealState int
--declare @PageSize int
--declare @PageIndex int
--declare @OrderString varchar(1000)
--declare @WhereString varchar(2000)
--declare @TotalCount int
--declare @TotalPageCount int
--set @RevealType = 0
--set @RevealState = 1
--set @PageSize = 30
--set @PageIndex = 1
--set @OrderString = 'SortOrder asc'
----set @OrderString = ''
--exec [sp_getLotteries] @RevealType,@RevealState,@PageSize,@PageIndex,@OrderString,@WhereString,@TotalCount output,@TotalPageCount output
--print @TotalCount
GO
/****** Object:  StoredProcedure [dbo].[sp_checkRegisterMember]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_checkRegisterMember] @UserName  NVARCHAR(20),
                                               @Email     NVARCHAR(50),
                                               @ErrorCode VARCHAR(10) output
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
      SET @ErrorCode = ''

      DECLARE @usernamecount INT=0,
              @emailcoiunt   INT=0

      SELECT @usernamecount = Count(*)
      FROM   Members
      WHERE  Lower(UserName) = Lower(@UserName)

      SELECT @emailcoiunt = Count(*)
      FROM   Members
      WHERE  Lower(Email) = Lower(@Email)

      IF @usernamecount > 0
        BEGIN
            IF @emailcoiunt > 0 --用户名和邮箱都已存在
              BEGIN
                  SET @ErrorCode='Error_03'
              END
            ELSE --用户名已存在
              BEGIN
                  SET @ErrorCode='Error_01'
              END
        END
      ELSE IF @emailcoiunt > 0 --邮箱已存在
        BEGIN
            SET @ErrorCode='Error_02'
        END
  END
GO
/****** Object:  StoredProcedure [dbo].[sp_getUsers]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_getUsers]
                                        @PageSize       INT = 10,--每页输出的记录数          
                                        @PageIndex      INT = 1,--当前页数
                                        @Order          VARCHAR(1000),--排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
                                        @Where          VARCHAR(2000) = null,--查询条件 不含'where'字符，如id>10 and len(userid)>9
                                        @TotalCount     INT output,--记返回总记录          
                                        @TotalPageCount INT OUTPUT --返回总页数 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
      DECLARE @TableName     VARCHAR(200),--表名          
              @FieldList     VARCHAR(2000),--显示列名，如果是全部字段则为*          
              @PrimaryKey    VARCHAR(100),--单一主键或唯一值键          
              --@Where         VARCHAR(2000),--查询条件 不含'where'字符，如id>10 and len(userid)>9          
              --@Order VARCHAR(1000),        --排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
              --注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷          
              @SortType      INT,--排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
              @RecorderCount INT --记录总数 0:会返回总记录 
      SET @TableName='View_Users' --表名          
      SET @FieldList ='*' --显示列名，如果是全部字段则为*          
      SET @PrimaryKey ='MemberId' --单一主键或唯一值键
      
      IF Isnull(@Where, '') = ''
      begin 
      SET @Where ='1=1'
      end

      IF Isnull(@Order, '') <> ''
        BEGIN
            SET @Order =@Order + ',MemberId asc' --排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
        END
      ELSE
        BEGIN
            SET @Order = 'MemberId asc'
        END

      --注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷          
      SET @SortType =3 --排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
      SET @RecorderCount = 0 --记录总数 0:会返回总记录  
      EXEC [Sp_viewpage]
        @TableName,--表名          
        @FieldList,--显示列名，如果是全部字段则为*          
        @PrimaryKey,--单一主键或唯一值键          
        @Where,--查询条件 不含'where'字符，如id>10 and len(userid)>9          
        @Order,--排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
        --注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷          
        @SortType,--排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
        @RecorderCount,--记录总数 0:会返回总记录          
        @PageSize,--每页输出的记录数          
        @PageIndex,
        @TotalCount output,
        @TotalPageCount output
END

--declare @PageSize int
--declare @PageIndex int
--declare @OrderString varchar(1000)
--declare @WhereString varchar(2000)
--declare @TotalCount int
--declare @TotalPageCount int
--set @PageSize = 10
--set @PageIndex = 2
--set @OrderString = ''
--set @WhereString = null
----set @OrderString = ''
--exec [sp_getUsers] @PageSize,@PageIndex,@OrderString,@WhereString,@TotalCount output,@TotalPageCount output
--print @TotalCount
--print @TotalPageCount
GO
/****** Object:  View [dbo].[View_Users]    Script Date: 11/30/2014 00:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Users]
AS
SELECT     dbo.Members.Id AS MemberId, dbo.Members.UserName, dbo.Members.Password, dbo.Members.Name, dbo.Members.Email, dbo.Members.MemberType, 
                      dbo.Members.MemberTypeNum, dbo.Members.IsDeleted, dbo.Members.AddDate, dbo.MemberExtends.Tel, dbo.MemberExtends.Province, dbo.MemberExtends.City, 
                      dbo.MemberExtends.County, dbo.MemberExtends.Street
FROM         dbo.Members LEFT OUTER JOIN
                      dbo.MemberExtends ON dbo.Members.Id = dbo.MemberExtends.Member_Id
WHERE     (dbo.Members.IsDeleted = 0)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Members"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MemberExtends"
            Begin Extent = 
               Top = 6
               Left = 248
               Bottom = 125
               Right = 391
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 19
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Users'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Users'
GO
/****** Object:  StoredProcedure [dbo].[sp_getUsersForWhiteList]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_getUsersForWhiteList] @PageSize       INT = 10,--每页输出的记录数          
                                                @PageIndex      INT = 1,--当前页数
                                                @Order          VARCHAR(1000),--排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
                                                @Where          VARCHAR(2000) = NULL,--查询条件 不含'where'字符，如id>10 and len(userid)>9
                                                @PrizeOrderId   VARCHAR(100),--奖单ID 
                                                @TotalCount     INT output,--记返回总记录          
                                                @TotalPageCount INT OUTPUT --返回总页数
                                                ,
                                                @ErrorCode      VARCHAR(10) output --错误代码
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
      SET @ErrorCode=''

      DECLARE @whiteUsers VARCHAR(max)

      SELECT @whiteUsers = Stuff((SELECT ',' + Cast(Member_Id AS VARCHAR(10)) + ''
                                  FROM   dbo.WhiteLists
                                  FOR xml path('')), 1, 1, '')

      --select  @whiteUsers = stuff((select top 10 ','+cast(Id as varchar(10))+'' from dbo.Members for xml path('')),1,1,'')
      --select stuff((select ','''+Name+'''' from dbo.Members for xml path('')),1,1,'')
      IF Isnull(@Where, '') = ''
        BEGIN
            SET @Where ='1=1'
        END

      PRINT @whiteUsers

      IF Isnull(@whiteUsers, '') != ''
        BEGIN
            SET @Where =@Where + ' and MemberId not in(' + @whiteUsers
                        + ')'
        END

      EXEC [Sp_getusers]
        @PageSize,
        @PageIndex,
        @Order,
        @Where,
        @TotalCount output,
        @TotalPageCount output
  END
--declare @PageSize int
--declare @PageIndex int
--declare @PrizeOrderId varchar(100)
--declare @OrderString varchar(1000)
--declare @WhereString varchar(2000)
--declare @TotalCount int
--declare @TotalPageCount int
--declare @ErrorCode varchar(10)
--set @PageSize = 10
--set @PageIndex = 2
--set @OrderString = ''
--set @WhereString = null
----set @OrderString = ''
--exec [sp_getUsersForWhiteList] @PageSize,@PageIndex,@OrderString,@WhereString,@PrizeOrderId,@TotalCount output,@TotalPageCount output,@ErrorCode output
--print @TotalCount
--print @TotalPageCount
--print @ErrorCode
GO
/****** Object:  StoredProcedure [dbo].[sp_addToWhiteList]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_addToWhiteList] @MemberId     INT,
                                          @PrizeOrderId VARCHAR(100),
                                          @ErrorCode    VARCHAR(10) output
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
      SET @ErrorCode=''

      DECLARE @WhilteListCount INT
      DECLARE @LuckyCount INT

      SELECT @LuckyCount = LuckyCount
      FROM   PrizeOrderExtends
      WHERE  PrizeOrder_Id = @PrizeOrderId

      SELECT @WhilteListCount = Count(*)
      FROM   WhiteLists
      WHERE  PrizeOrder_Id = @PrizeOrderId

      IF @WhilteListCount = @LuckyCount
        BEGIN
            SET @ErrorCode = 'Error_01'

            PRINT '白名单数目不能超过中奖人数'

            RETURN
        END

      DECLARE @rcount INT=0

      SELECT @rcount = Count(*)
      FROM   WhiteLists
      WHERE  Member_Id = @MemberId
             AND PrizeOrder_Id = @PrizeOrderId

      IF @rcount > 0
        BEGIN
            SET @ErrorCode = 'Error_02'

            PRINT '该用户已在白名单中，不能重复添加。'

            RETURN
        END

      INSERT INTO WhiteLists
                  (Id,
                   Member_Id,
                   PrizeOrder_Id,
                   IsDeleted,
                   AddDate)
      VALUES     (Newid(),
                  @MemberId,
                  @PrizeOrderId,
                  0,
                  Getdate())
  END
GO
/****** Object:  UserDefinedFunction [dbo].[GetBettingCount]    Script Date: 11/30/2014 00:39:06 ******/
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
/****** Object:  StoredProcedure [dbo].[sp_revealTimingLottery]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
              @PoolCount      INT,
              @RevealType     CHAR(1)

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
                              (Id,
                               [State],
                               IsDeleted,
                               AddDate,
                               UpdateDate,
                               Member_Id,
                               PrizeOrder_Id)
            SELECT Newid(),
                   0,
                   0,
                   Getdate(),
                   null,
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
                  DECLARE @sqlstr   VARCHAR(max)='',
                          @wherestr VARCHAR(max)='',
                          @answer   VARCHAR(10)=''

                  SET @sqlstr = 'select top '
                                + Cast(@LuckCountTrue AS VARCHAR(20))
                                + ' Member_Id from PrizeBettings WHERE 1=1 '

                  IF @RevealType = 3
                    BEGIN
                        SELECT @answer = Answer
                        FROM   dbo.PrizeOrderExtends
                        WHERE  PrizeOrder_Id = @PrizeOrderId

                        SET @wherestr=' and AnswerOption=''' + @answer + ''' '
                    END

                  SET @wherestr += ' and PrizeOrder_Id =''' + @PrizeOrderId
                                   + ''' order by newid()'
                  SET @sqlstr += @wherestr

                  --创建临时表，存储中奖者
                  CREATE TABLE #tb
                    (
                       MemberId INT
                    )

                  --将中奖者存入临时表
                  INSERT INTO #tb
                  EXEC(@sqlstr)

                  INSERT INTO dbo.LotteryResults
                              (Id,
                               [State],
                               IsDeleted,
                               AddDate,
                               UpdateDate,
                               Member_Id,
                               PrizeOrder_Id)
                  SELECT Newid(),
                         0,
                         0,
                         Getdate(),
                         null,
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
/****** Object:  StoredProcedure [dbo].[sp_revealQuotaLottery]    Script Date: 11/30/2014 00:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_revealQuotaLottery] @RevealOrders    [REVEALORDERSTABLETYPE] readonly,
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
              @PoolCount      INT,
              @RevealType     CHAR(1)

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
            SELECT @PrizeOrderId = PrizeOrderId,
                   @RevealType = RevealType
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
                        (Id,
                         [State],
                         IsDeleted,
                         AddDate,
                         UpdateDate,
                         Member_Id,
                         PrizeOrder_Id)
            SELECT Newid(),
                   0,
                   0,
                   Getdate(),
                   NULL,
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
                  DECLARE @sqlstr   VARCHAR(max)='',
                          @wherestr VARCHAR(max)='',
                          @answer   VARCHAR(10)=''

                  SET @sqlstr = 'select top '
                                + Cast(@LuckCountTrue AS VARCHAR(20))
                                + ' Member_Id from PrizeBettings WHERE 1=1 '

                  IF @RevealType = 3
                    BEGIN
                        SELECT @answer = Answer
                        FROM   dbo.PrizeOrderExtends
                        WHERE  PrizeOrder_Id = @PrizeOrderId

                        SET @wherestr=' and AnswerOption=''' + @answer + ''' '
                    END

                  SET @wherestr += ' and PrizeOrder_Id =''' + @PrizeOrderId
                                   + ''' order by newid()'
                  SET @sqlstr += @wherestr

                  --创建临时表，存储中奖者
                  CREATE TABLE #tb
                    (
                       MemberId INT
                    )

                  --将中奖者存入临时表
                  INSERT INTO #tb
                  EXEC(@sqlstr)

                  INSERT INTO dbo.LotteryResults
                              (Id,
                               [State],
                               IsDeleted,
                               AddDate,
                               UpdateDate,
                               Member_Id,
                               PrizeOrder_Id)
                  SELECT Newid(),
                         0,
                         0,
                         Getdate(),
                         NULL,
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
/****** Object:  View [dbo].[View_Lotteries]    Script Date: 11/30/2014 00:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Lotteries]
AS
SELECT     po.Id AS PrizeOrderId, po.RevealType, po.RevealState, po.SortOrder, po.AddDate, p.Id AS PrizeId, p.Name AS PrizeName, p.Description AS PrizeDescription, 
                      m.Name AS UserNickName, m.UserName, po.AddDate AS RaiseTime
FROM         dbo.PrizeOrders AS po LEFT OUTER JOIN
                      dbo.PrizeOrderExtends AS poe ON po.Id = poe.PrizeOrder_Id LEFT OUTER JOIN
                      dbo.Prizes AS p ON p.Id = po.Prize_Id LEFT OUTER JOIN
                      dbo.Members AS m ON p.Member_Id = m.Id LEFT OUTER JOIN
                      dbo.MemberExtends AS me ON m.Id = me.Member_Id
WHERE     (po.IsDeleted = 0)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "po"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 207
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "poe"
            Begin Extent = 
               Top = 6
               Left = 245
               Bottom = 125
               Right = 493
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 6
               Left = 531
               Bottom = 125
               Right = 678
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "me"
            Begin Extent = 
               Top = 126
               Left = 248
               Bottom = 245
               Right = 391
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Ou' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Lotteries'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'tput = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Lotteries'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Lotteries'
GO
/****** Object:  StoredProcedure [dbo].[sp_revealAnswerLottery]    Script Date: 11/30/2014 00:39:04 ******/
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
             po.Id                           AS PrizeOrderId,
             3 --答案开奖
      FROM   dbo.PrizeOrders po
             LEFT JOIN dbo.PrizeOrderExtends poe
                    ON po.Id = poe.PrizeOrder_Id
      WHERE  RevealState = 1 --未开奖
             AND po.IsDeleted = 0
             AND po.RevealTypeNum = 3 --答案开奖
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
             po.Id                           AS PrizeOrderId,
             3 --答案开奖
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
/****** Object:  StoredProcedure [dbo].[sp_revealLottery]    Script Date: 11/30/2014 00:39:04 ******/
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
             po.Id                           AS PrizeOrderId,
             1 --定时开奖
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
             po.Id                           AS PrizeOrderId,
             2 --定员开奖
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
