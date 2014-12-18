// Դ�ļ�ͷ��Ϣ��
// <copyright file="PrizeOrderService.cs">
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
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System;
using LotteryDraw.Component.Data;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     �ֳ��齱��Ա����ҵ��ʵ��
    /// </summary>
    [Export(typeof(ISceneStaffContract))]
    public class SceneStaffService : CoreServiceBase, ISceneStaffContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ ��Ʒ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected ISceneStaffRepository SceneStaffRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ �����ֳ��齱��Ա�Ĳ�ѯ���ݼ�
        /// </summary>
        public IQueryable<SceneStaff> SceneStaffs
        {
            get { return SceneStaffRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     ��Ӳ���齱����Ա
        /// </summary>
        /// <param name="staffs">�齱��Ա��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Add(SceneStaff staff)
        {
            int rcount = SceneStaffRepository.Insert(staff);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "�ֳ��齱��Ա��ӳɹ���", staff);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "�ֳ��齱��Ա���ʧ�ܡ�");
            }
        }

        /// <summary>
        ///     ��Ӳ���齱����Ա
        /// </summary>
        /// <param name="staffs">�齱��Ա������Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Add(IEnumerable<SceneStaff> staffs)
        {
            int rcount = SceneStaffRepository.Insert(staffs);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "��������ֳ��齱��Ա�ɹ���");
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "�����������ֳ��齱��Աʧ�ܡ�");
            }
        }
    }
}