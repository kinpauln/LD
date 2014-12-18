// Դ�ļ�ͷ��Ϣ��
// <copyright file="IPrizeOrderContract.cs">
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
using System.Collections.Generic;
using System;


namespace LotteryDraw.Core
{
    /// <summary>
    ///     �˻�ģ�����ҵ����Լ
    /// </summary>
    public interface ISceneStaffContract
    {
        #region ����

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        IQueryable<SceneStaff> SceneStaffs { get; }

        #endregion

        #region ��������

        /// <summary>
        ///     ��Ӳ���齱����Ա
        /// </summary>
        /// <param name="staffs">�齱��Ա��Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Add(SceneStaff staffs);

        /// <summary>
        ///     ��Ӳ���齱����Ա
        /// </summary>
        /// <param name="staffs">�齱��Ա������Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Add(IEnumerable<SceneStaff> staffs);
        #endregion
    }
}