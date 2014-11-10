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
using System;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     ����������ҵ����Լ
    /// </summary>
    [Export(typeof(IWhiteListContract))]
    public class WhiteListService : CoreServiceBase, IWhiteListContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ ��Ʒ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected IWhiteListRepository WhiteListRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ��������ѯ���ݼ�
        /// </summary>
        public IQueryable<WhiteList> WhiteLists
        {
            get { return WhiteListRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     ��Ӱ�����
        /// </summary>
        /// <param name="prizebetting">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Add(int memberid, Guid poid)
        {
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //�û�ID
                SqlParameter paramMemberId = new SqlParameter("@MemberId", SqlDbType.Int);
                paramMemberId.Value = memberid;
                paramList.Add(paramMemberId);
                //PrizeOrder Id
                SqlParameter paramPI = new SqlParameter("@PrizeOrderId", SqlDbType.VarChar, 100);
                paramPI.Value = poid.ToString();
                paramList.Add(paramPI);

                SqlParameter paramerrorcode = new SqlParameter("@ErrorCode", SqlDbType.VarChar, 10);
                paramerrorcode.Direction = ParameterDirection.Output;
                paramList.Add(paramerrorcode);

                SqlCommand command = new SqlCommand();
                DataSet ds = WhiteListRepository.ExecProcdureReturnDataSet("sp_addToWhiteList", out command, paramList.ToArray());

                string errorCode = command.Parameters["@ErrorCode"].Value.ToString();
                if (string.IsNullOrEmpty(errorCode))
                {
                    return new OperationResult(OperationResultType.Success, "��Ӱ������ɹ���", null);
                }
                else
                {
                    switch (errorCode)
                    {
                        case "Error_01":
                            return new OperationResult(OperationResultType.Warning, "��������Ŀ���ܳ����н�������", null);
                        case "Error_02":
                            return new OperationResult(OperationResultType.Warning, "���û����ڰ������У������ظ���ӡ�", null);
                        default:
                            return new OperationResult(OperationResultType.Warning, "�����ˡ�", null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///     ɾ��������
        /// </summary>
        /// <param name="wl">������ʵ��</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Delete(WhiteList wl)
        {
            int rcount = WhiteListRepository.Delete(wl);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "ɾ���������ɹ���", wl);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "ɾ��������ʧ�ܡ�");
            }
        }

        /// <summary>
        ///  ȡ����������������û�
        /// </summary>
        /// <param name="pageSize">ÿҳ����ļ�¼��</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="whereString">�����ַ���</param>
        /// <param name="orderbyString">�����ַ���</param>
        /// <param name="totalCount">�����ܼ�¼</param>
        /// <param name="totalPageCount">������ҳ��</param>
        /// <param name="revealstate">����ID</param>
        /// <returns></returns>
        public OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, Guid poid)
        {
            totalCount = 0;
            totalPageCount = 0;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //ÿҳ����ļ�¼��
                SqlParameter paramPS = new SqlParameter("@PageSize", SqlDbType.Int);
                paramPS.Value = pageSize;
                paramList.Add(paramPS);
                //��ǰҳ��
                SqlParameter paramPI = new SqlParameter("@PageIndex", SqlDbType.Int);
                paramPI.Value = pageIndex;
                paramList.Add(paramPI);
                //where�����ַ���
                SqlParameter paramWhere = new SqlParameter("@Where", SqlDbType.VarChar, 2000);
                paramWhere.Value = whereString;
                paramList.Add(paramWhere);
                //�����ַ���
                SqlParameter paramOrder = new SqlParameter("@Order", SqlDbType.VarChar, 1000);
                paramOrder.Value = orderbyString;
                paramList.Add(paramOrder);
                //����ID
                SqlParameter paramPOId = new SqlParameter("@PrizeOrderId", SqlDbType.VarChar, 100);
                paramPOId.Value = poid.ToString();
                paramList.Add(paramPOId);

                SqlParameter paramtc = new SqlParameter("@TotalCount", SqlDbType.Int);
                paramtc.Direction = ParameterDirection.Output;
                paramList.Add(paramtc);
                SqlParameter paramtpc = new SqlParameter("@TotalPageCount", SqlDbType.Int);
                paramtpc.Direction = ParameterDirection.Output;
                paramList.Add(paramtpc);
                SqlParameter paramerrorcode = new SqlParameter("@ErrorCode", SqlDbType.VarChar, 10);
                paramerrorcode.Direction = ParameterDirection.Output;
                paramList.Add(paramerrorcode);

                SqlCommand command = new SqlCommand();
                DataSet ds = WhiteListRepository.ExecProcdureReturnDataSet("sp_getUsersForWhiteList", out command, paramList.ToArray());

                string errorCode = command.Parameters["@ErrorCode"].Value.ToString();
                if (string.IsNullOrEmpty(errorCode))
                {
                    totalCount = Convert.ToInt32(command.Parameters["@TotalCount"].Value);
                    totalPageCount = Convert.ToInt32(command.Parameters["@TotalPageCount"].Value);
                    return new OperationResult(OperationResultType.Success, "ģ����ѯ�û�����˳����", ds);
                }
                else
                {
                    switch (errorCode)
                    {
                        case "Error_01":
                            return new OperationResult(OperationResultType.Warning, "��������Ŀ���ܳ����н�������", null);
                        default:
                            return new OperationResult(OperationResultType.Warning, "�����ˡ�", null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }
    }
}