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
using System.Data.SqlClient;
using System.Data;
using System;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     账户模块核心业务实现
    /// </summary>
    [Export(typeof(IAccountContract))]
    public class AccountService : CoreServiceBase, IAccountContract
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 用户信息数据访问对象
        /// </summary>
        [Import]
        protected IMemberRepository MemberRepository { get; set; }

        /// <summary>
        /// 获取或设置 用户扩展信息数据访问对象
        /// </summary>
        [Import]
        protected IMemberExtendRepository MemberExtendRepository { get; set; }

        /// <summary>
        /// 获取或设置 登录记录信息数据访问对象
        /// </summary>
        [Import]
        protected ILoginLogRepository LoginLogRepository { get; set; }

        /// <summary>
        /// 获取或设置 角色信息业务访问对象
        /// </summary>
        [Import]
        protected IRoleRepository RoleRepository { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<Member> Members
        {
            get { return MemberRepository.Entities; }
        }

        /// <summary>
        /// 获取 用户扩展信息查询数据集
        /// </summary>
        public IQueryable<MemberExtend> MemberExtends
        {
            get { return MemberExtendRepository.Entities; }
        }

        /// <summary>
        /// 获取 登录记录信息查询数据集
        /// </summary>
        public IQueryable<LoginLog> LoginLogs
        {
            get { return LoginLogRepository.Entities; }
        }

        /// <summary>
        /// 获取 角色信息查询数据集
        /// </summary>
        public IQueryable<Role> Roles
        {
            get { return RoleRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult Login(LoginInfo loginInfo)
        {
            PublicHelper.CheckArgument(loginInfo, "loginInfo");
            Member member = MemberRepository.Entities.SingleOrDefault(m => m.UserName == loginInfo.Account || m.Email == loginInfo.Account);
            if (member == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定账号的用户不存在。");
            }
            if (member.Password != loginInfo.Password)
            {
                return new OperationResult(OperationResultType.Warning, "登录密码不正确。");
            }
            LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            LoginLogRepository.Insert(loginLog);
            return new OperationResult(OperationResultType.Success, "登录成功。", member);
        }

        /// <summary>
        ///     用户注册
        /// </summary>
        /// <param name="member">用户信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Register(Member member)
        {
            //PublicHelper.CheckArgument(member, "member");
            //校验是否重复注册（用户名和邮箱是否已被注册过）
            OperationResult fresult = CheckRegisteringMember(member.UserName, member.Email);

            if (fresult.ResultType != OperationResultType.Success)
                return fresult;

            int rcount = MemberRepository.Insert(member);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "注册成功。", member);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "注册失败。");
            }
        }

        /// <summary>
        ///     校验用户名和邮箱是否已存在
        /// </summary>
        /// <param name="prizebetting">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult CheckRegisteringMember(string username, string email)
        {
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //用户名
                SqlParameter paramUserName = new SqlParameter("@UserName", SqlDbType.NVarChar,20);
                paramUserName.Value = username;
                paramList.Add(paramUserName);
                //邮箱
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
                    return new OperationResult(OperationResultType.Success, "校验成功。", null);
                }
                else
                {
                    switch (errorCode)
                    {
                        case "Error_01":
                            return new OperationResult(OperationResultType.Warning, "用户名已存在", null);
                        case "Error_02":
                            return new OperationResult(OperationResultType.Warning, "邮箱已存在", null);
                        case "Error_03":
                            return new OperationResult(OperationResultType.Warning, "用户名和邮箱都已存在", null);
                        default:
                            return new OperationResult(OperationResultType.Warning, "出错了。", null);
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
                return new OperationResult(OperationResultType.QueryNull, string.Format("Id为{0}用户不存在。", userid.ToString()));
            }
            return new OperationResult(OperationResultType.Success, "获取用户信息成功。", member);
        }


        /// <summary>
        ///  取用户
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <param name="revealtype">开奖类型</param>
        /// <param name="revealstate">奖单状态</param>
        /// <returns></returns>
        public OperationResult GetUsers(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount)
        {
            totalCount = 0;
            totalPageCount = 0;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();

                //每页输出的记录数
                SqlParameter paramPS = new SqlParameter("@PageSize", SqlDbType.Int);
                paramPS.Value = pageSize;
                paramList.Add(paramPS);
                //当前页数
                SqlParameter paramPI = new SqlParameter("@PageIndex", SqlDbType.Int);
                paramPI.Value = pageIndex;
                paramList.Add(paramPI);
                //排序字符串
                SqlParameter paramWhere = new SqlParameter("@Where", SqlDbType.VarChar, 2000);
                paramWhere.Value = whereString;
                paramList.Add(paramWhere);
                //排序字符串
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
                return new OperationResult(OperationResultType.Success, "模糊查询用户操作顺利。", ds);
            }
            catch (System.Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///  免审核
        /// </summary>
        /// <param name="memberid">用户ID</param>
        public bool NoAudit(long memberid)
        {
            try
            {
                Member member = MemberRepository.Entities.SingleOrDefault(m => m.Id == memberid);
                if (member == null)
                {
                    throw new BusinessException(string.Format("Id为{0}用户不存在。", memberid.ToString()));
                }
                member.PubishingEnableTimes = int.MaxValue;
                int rcount = MemberRepository.Update(member);
                if (rcount > 0)
                {
                    return true;
                }
                else
                {
                    throw new BusinessException("更新操作影响的行数为0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  免审核
        /// </summary>
        /// <param name="memberid">用户ID</param>
        public bool Delete(long memberid)
        {
            try
            {
                Member member = MemberRepository.Entities.SingleOrDefault(m => m.Id == memberid);
                if (member == null)
                {
                    throw new BusinessException(string.Format("Id为{0}用户不存在。", memberid.ToString()));
                }
                member.IsDeleted = true;
                int rcount = MemberRepository.Update(member);
                if (rcount > 0)
                {
                    return true;
                }
                else
                {
                    throw new BusinessException("更新操作影响的行数为0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  重置密码
        /// </summary>
        /// <param name="memberid">用户Id</param>
        public bool ResetPassword(long memberid)
        {
            try
            {
                Member member = MemberRepository.Entities.SingleOrDefault(m => m.Id == memberid);
                if (member == null)
                {
                    throw new BusinessException(string.Format("Id为{0}用户不存在。", memberid.ToString()));
                }
                member.Password = "123456";
                int rcount = MemberRepository.Update(member);
                if (rcount > 0)
                {
                    return true;
                }
                else
                {
                    throw new BusinessException("更新操作影响的行数为0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}