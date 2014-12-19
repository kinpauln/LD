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
using System.Data.SqlClient;
using System.Data;
using System;
using LotteryDraw.Core.Data.Repositories.Business;
using LotteryDraw.Core.Models.Business;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     �˻�ģ�����ҵ��ʵ��
    /// </summary>
    [Export(typeof(ILotteryResultContract))]
    public class LotteryResultService : CoreServiceBase, ILotteryResultContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ �н�������ݷ��ʶ���
        /// </summary>
        [Import]
        protected ILotteryResultRepository LotteryResultRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<LotteryResult> LotteryResults
        {
            get { return LotteryResultRepository.Entities; }
        }

        #endregion

        public OperationResult UpdateLotteryResult(Guid id, int state)
        {
            LotteryResult entity = LotteryResultRepository.Entities.Where(lr => lr.Id == id).FirstOrDefault();

            if (entity ==null)
            {
                return new OperationResult(OperationResultType.Warning, string.Format("û��IdΪ{0}���н���Ϣ��",id.ToString()), id.ToString());
            }

            if (state == entity.LotteryResultStateNum)
            {
                return new OperationResult(OperationResultType.Warning, "Ҫ���µ�״̬�����ݿ��һ�£�������ġ�", entity);
            }
            entity.LotteryResultStateNum = state;
            int rcount = LotteryResultRepository.Update(entity);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "����״̬�ɹ���", entity);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "����״̬ʧ�ܡ�");
            }
        }

        #endregion
    }
}