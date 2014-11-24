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
            //У���Ƿ��ظ�ע�ᣨ�û����������Ƿ��ѱ�ע�����
            OperationResult fresult = CheckRegisteringMember(member.UserName, member.Email);

            if (fresult.ResultType != OperationResultType.Success)
                return fresult;

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

        /// <summary>
        ///     У���û����������Ƿ��Ѵ���
        /// </summary>
        /// <param name="prizebetting">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult CheckRegisteringMember(string username, string email)
        {
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //�û���
                SqlParameter paramUserName = new SqlParameter("@UserName", SqlDbType.NVarChar,20);
                paramUserName.Value = username;
                paramList.Add(paramUserName);
                //����
                SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
                paramEmail.Value = email;
                paramList.Add(paramEmail);

                SqlParameter paramerrorcode = new SqlParameter("@ErrorCode", SqlDbType.VarChar, 10);
                paramerrorcode.Direction = ParameterDirection.Output;
                paramList.Add(paramerrorcode);

                SqlCommand command = new SqlCommand();
                DataSet ds = MemberRepository.ExecProcdureReturnDataSet("Sp_checkregistermember", out command, paramList.ToArray());

                string errorCode = command.Parameters["@ErrorCode"].Value.ToString();
                if (string.IsNullOrEmpty(errorCode))
                {
                    return new OperationResult(OperationResultType.Success, "У��ɹ���", null);
                }
                else
                {
                    switch (errorCode)
                    {
                        case "Error_01":
                            return new OperationResult(OperationResultType.Warning, "�û����Ѵ���", null);
                        case "Error_02":
                            return new OperationResult(OperationResultType.Warning, "�����Ѵ���", null);
                        case "Error_03":
                            return new OperationResult(OperationResultType.Warning, "�û��������䶼�Ѵ���", null);
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

        public OperationResult GetMember(long userid)
        {
            Member member = MemberRepository.Entities.SingleOrDefault(m => m.Id == userid);
            if (member == null)
            {
                return new OperationResult(OperationResultType.QueryNull, string.Format("IdΪ{0}�û������ڡ�", userid.ToString()));
            }
            return new OperationResult(OperationResultType.Success, "��ȡ�û���Ϣ�ɹ���", member);
        }


        /// <summary>
        ///  ȡ�û�
        /// </summary>
        /// <param name="pageSize">ÿҳ����ļ�¼��</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="whereString">�����ַ���</param>
        /// <param name="orderbyString">�����ַ���</param>
        /// <param name="totalCount">�����ܼ�¼</param>
        /// <param name="totalPageCount">������ҳ��</param>
        /// <param name="revealtype">��������</param>
        /// <param name="revealstate">����״̬</param>
        /// <returns></returns>
        public OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount)
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
                //�����ַ���
                SqlParameter paramWhere = new SqlParameter("@Where", SqlDbType.VarChar, 2000);
                paramWhere.Value = whereString;
                paramList.Add(paramWhere);
                //�����ַ���
                SqlParameter paramOrder = new SqlParameter("@Order", SqlDbType.VarChar, 1000);
                paramOrder.Value = orderbyString;
                paramList.Add(paramOrder);

                SqlParameter paramtc = new SqlParameter("@TotalCount", SqlDbType.Int);
                paramtc.Direction = ParameterDirection.Output;
                paramList.Add(paramtc);
                SqlParameter paramtpc = new SqlParameter("@TotalPageCount", SqlDbType.Int);
                paramtpc.Direction = ParameterDirection.Output;
                paramList.Add(paramtpc);


                SqlCommand command = new SqlCommand();
                DataSet ds = MemberRepository.ExecProcdureReturnDataSet("sp_getUsers", out command, paramList.ToArray());
                totalCount = Convert.ToInt32(command.Parameters["@TotalCount"].Value);
                totalPageCount = Convert.ToInt32(command.Parameters["@TotalPageCount"].Value);
                return new OperationResult(OperationResultType.Success, "ģ����ѯ�û�����˳����", ds);
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  �����
        /// </summary>
        /// <param name="memberid">�û�ID</param>
        public bool NoAudit(long memberid)
        {
            try
            {
                Member member = MemberRepository.Entities.SingleOrDefault(m => m.Id == memberid);
                if (member == null)
                {
                    throw new BusinessException(string.Format("IdΪ{0}�û������ڡ�", memberid.ToString()));
                }
                member.PubishingEnableTimes = int.MaxValue;
                int rcount = MemberRepository.Update(member);
                if (rcount > 0)
                {
                    return true;
                }
                else
                {
                    throw new BusinessException("���²���Ӱ�������Ϊ0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  �����
        /// </summary>
        /// <param name="memberid">�û�ID</param>
        public bool Delete(long memberid)
        {
            try
            {
                Member member = MemberRepository.Entities.SingleOrDefault(m => m.Id == memberid);
                if (member == null)
                {
                    throw new BusinessException(string.Format("IdΪ{0}�û������ڡ�", memberid.ToString()));
                }
                member.IsDeleted = true;
                int rcount = MemberRepository.Update(member);
                if (rcount > 0)
                {
                    return true;
                }
                else
                {
                    throw new BusinessException("���²���Ӱ�������Ϊ0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  ��������
        /// </summary>
        /// <param name="memberid">�û�Id</param>
        public bool ResetPassword(long memberid)
        {
            try
            {
                Member member = MemberRepository.Entities.SingleOrDefault(m => m.Id == memberid);
                if (member == null)
                {
                    throw new BusinessException(string.Format("IdΪ{0}�û������ڡ�", memberid.ToString()));
                }
                member.Password = "123456";
                int rcount = MemberRepository.Update(member);
                if (rcount > 0)
                {
                    return true;
                }
                else
                {
                    throw new BusinessException("���²���Ӱ�������Ϊ0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}