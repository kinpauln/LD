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


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     �˻�ģ�����ҵ��ʵ��
    /// </summary>
    [Export(typeof(IAccountContract))]
    public class AccountService : CoreServiceBase, IAccountContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ �û���Ϣ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected IMemberRepository MemberRepository { get; set; }

        /// <summary>
        /// ��ȡ������ �û���չ��Ϣ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected IMemberExtendRepository MemberExtendRepository { get; set; }

        /// <summary>
        /// ��ȡ������ ��¼��¼��Ϣ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected ILoginLogRepository LoginLogRepository { get; set; }

        /// <summary>
        /// ��ȡ������ ��ɫ��Ϣҵ����ʶ���
        /// </summary>
        [Import]
        protected IRoleRepository RoleRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<Member> Members
        {
            get { return MemberRepository.Entities; }
        }

        /// <summary>
        /// ��ȡ �û���չ��Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<MemberExtend> MemberExtends
        {
            get { return MemberExtendRepository.Entities; }
        }

        /// <summary>
        /// ��ȡ ��¼��¼��Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<LoginLog> LoginLogs
        {
            get { return LoginLogRepository.Entities; }
        }

        /// <summary>
        /// ��ȡ ��ɫ��Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<Role> Roles
        {
            get { return RoleRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        /// �û���¼
        /// </summary>
        /// <param name="loginInfo">��¼��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public virtual OperationResult Login(LoginInfo loginInfo)
        {
            PublicHelper.CheckArgument(loginInfo, "loginInfo");
            Member member = MemberRepository.Entities.SingleOrDefault(m => m.UserName == loginInfo.Account || m.Email == loginInfo.Account);
            if (member == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "ָ���˺ŵ��û������ڡ�");
            }
            if (member.Password != loginInfo.Password)
            {
                return new OperationResult(OperationResultType.Warning, "��¼���벻��ȷ��");
            }
            LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            LoginLogRepository.Insert(loginLog);
            return new OperationResult(OperationResultType.Success, "��¼�ɹ���", member);
        }

        /// <summary>
        ///     �û�ע��
        /// </summary>
        /// <param name="member">�û���Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Register(Member member)
        {
            //PublicHelper.CheckArgument(member, "member");
            int rcount = MemberRepository.Insert(member);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "ע��ɹ���", member);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "ע��ʧ�ܡ�");
            }
        }
    }
}