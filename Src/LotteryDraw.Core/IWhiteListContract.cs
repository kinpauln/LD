// Դ�ļ�ͷ��Ϣ��
// <copyright file="IAccountService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core
// ����޸ģ�������
// ����޸ģ�2013/05/27 23:06
// </copyright>

using System.Linq;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Data.Repositories;
using LotteryDraw.Core.Models;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Models.Business;
using System;


namespace LotteryDraw.Core
{
    /// <summary>
    ///     ����������ҵ����Լ
    /// </summary>
    public interface IWhiteListContract
    {
        #region ����

        /// <summary>
        /// ��ȡ��������ѯ���ݼ�
        /// </summary>
        IQueryable<WhiteList> WhiteLists { get; }

        #endregion

        #region ��������
        /// <summary>
        ///     ��Ӱ�����
        /// </summary>
        /// <param name="prizebetting">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Add(int memberid, Guid poid);

        /// <summary>
        ///     ɾ��������
        /// </summary>
        /// <param name="wl">������ʵ��</param>
        /// <returns>ҵ��������</returns>
        OperationResult Delete(WhiteList wl);
        
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
        OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, Guid poid);
        #endregion
    }
}