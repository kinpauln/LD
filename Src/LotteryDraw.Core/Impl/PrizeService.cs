// Դ�ļ�ͷ��Ϣ��
// <copyright file="AccountService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core
// ����޸ģ�������
// ����޸ģ�2014/08/06 23:08
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
    ///     �˻�ģ�����ҵ��ʵ��
    /// </summary>
    [Export(typeof(IPrizeContract))]
    public class PrizeService : CoreServiceBase, IPrizeContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ ��Ʒ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected IPrizeRepository PrizeRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<Prize> Prizes
        {
            get { return PrizeRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     ��ӽ�Ʒ
        /// </summary>
        /// <param name="prizeorder">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Add(Prize prize)
        {
            int rcount = PrizeRepository.Insert(prize);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "������Ʒ�ɹ���", prize);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "������Ʒʧ�ܡ�");
            }
        }

        /// <summary>
        ///     ���½�Ʒ
        /// </summary>
        /// <param name="member">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Update(Prize prize)
        {
            int rcount = PrizeRepository.Update(prize);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "���½�Ʒ�ɹ���", prize);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "���½�Ʒʧ�ܡ�");
            }
        }

        /// <summary>
        ///     ɾ����Ʒ
        /// </summary>
        /// <param name="member">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Delete(Prize prize)
        {
            int rcount = PrizeRepository.Delete(prize);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "ɾ����Ʒ�ɹ���", prize);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "ɾ����Ʒʧ�ܡ�");
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