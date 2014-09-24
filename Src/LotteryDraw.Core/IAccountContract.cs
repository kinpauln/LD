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


namespace LotteryDraw.Core
{
    /// <summary>
    ///     �˻�ģ�����ҵ����Լ
    /// </summary>
    public interface IAccountContract
    {
        #region ����

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        IQueryable<Member> Members { get; }

        /// <summary>
        /// ��ȡ �û���չ��Ϣ��ѯ���ݼ�
        /// </summary>
        IQueryable<MemberExtend> MemberExtends { get; }

        /// <summary>
        /// ��ȡ ��¼��¼��Ϣ��ѯ���ݼ�
        /// </summary>
        IQueryable<LoginLog> LoginLogs { get; }

        /// <summary>
        /// ��ȡ ��ɫ��Ϣ��ѯ���ݼ�
        /// </summary>
        IQueryable<Role> Roles { get; }

        #endregion

        #region ��������

        /// <summary>
        ///     �û���¼
        /// </summary>
        /// <param name="loginInfo">��¼��Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Login(LoginInfo loginInfo);

        /// <summary>
        ///     �û�ע��
        /// </summary>
        /// <param name="member">�û���Ϣ</param>
        /// <returns>ҵ��������</returns>
        OperationResult Register(Member member);

        #endregion
    }
}