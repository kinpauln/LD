// 源文件头信息：
// <copyright file="AccountService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core
// 最后修改：王金鹏
// 最后修改：2014/08/06 23:08
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Data.Repositories.Account;
using LotteryDraw.Core.Data.Repositories.Security;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Data.Repositories.Business;
using LotteryDraw.Core.Models.Business;
using System.Data.SqlClient;
using System.Data;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     账户模块核心业务实现
    /// </summary>
    [Export(typeof(IPrizeContract))]
    public class PrizeService : CoreServiceBase, IPrizeContract
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected IPrizeRepository PrizeRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<Prize> Prizes
        {
            get { return PrizeRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prizeorder">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(Prize prize)
        {
            int rcount = PrizeRepository.Insert(prize);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "发布奖品成功。", prize);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "发布奖品失败。");
            }
        }

        /// <summary>
        ///     更新奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Update(Prize prize)
        {
            int rcount = PrizeRepository.Update(prize);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "更新奖品成功。", prize);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "更新奖品失败。");
            }
        }

        /// <summary>
        ///     删除奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(Prize prize)
        {
            int rcount = PrizeRepository.Delete(prize);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "删除奖品成功。", prize);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "删除奖品失败。");
            }
        }

        public void CallProcedureDemo(out string outInvalidPartCodes, out string outInvalidPropertyCodes, out string outErrorMessage)
        {
            int searchType = 1; 
            string keywords = string.Empty; 
            int topcount = 0;
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@searchType", searchType));
            paramList.Add(new SqlParameter("@searchKeywords", keywords));
            paramList.Add(new SqlParameter("@topCount", topcount));

            DataSet ds = PrizeRepository.ExecProcdureReturnDataSet("sp_gloabalsearch", paramList.ToArray<SqlParameter>());


            DataTable dt = new DataTable();
            IList<SqlParameter> paramList2 = new List<SqlParameter>();

            SqlParameter paramdt = new SqlParameter("@Datas", SqlDbType.Structured);
            paramdt.Value = dt;
            paramList2.Add(paramdt);

            SqlParameter partParam = new SqlParameter("@InvalidPartCodes", SqlDbType.VarChar, 1000);
            partParam.Direction = ParameterDirection.Output;
            paramList2.Add(partParam);

            SqlParameter propertyParam = new SqlParameter("@InvalidPropertyCodes", SqlDbType.VarChar, 1000);
            propertyParam.Direction = ParameterDirection.Output;
            paramList2.Add(propertyParam);

            SqlParameter errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 1000);
            errorMessageParam.Direction = ParameterDirection.Output;
            paramList2.Add(errorMessageParam);

            SqlCommand command = new SqlCommand();
            PrizeRepository.ExecProcdure("sp_importPartPropertyDatas", out command, paramList.ToArray<SqlParameter>());

            outInvalidPartCodes = command.Parameters["@InvalidPartCodes"].Value.ToString();
            outInvalidPropertyCodes = command.Parameters["@InvalidPropertyCodes"].Value.ToString();
            outErrorMessage = command.Parameters["@ErrorMessage"].Value.ToString();

            int LanType = 0;
            int state = 0;
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("LanType", LanType);
            sqlparams[1] = new SqlParameter("state", state);
            DataTable DataTable = PrizeRepository.SqlQueryForDataTatable("select LeaveName,LeaveEmail from LeaveInfo where LanType=@LanType and State=@State", sqlparams);
        }
    }
}