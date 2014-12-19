// Դ�ļ�ͷ��Ϣ��
// <copyright file="Member.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core.Models
// ����޸ģ�������
// ����޸ģ�2014/08/06 23:15
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Models.Business;
using System;


namespace LotteryDraw.Core.Models.Account
{
    /// <summary>
    ///     ʵ���ࡪ���û���Ϣ
    /// </summary>
    [Description("�û���Ϣ")]
    public class Member : EntityBase<Int64>
    {
        public Member()
        {
            Roles = new List<Role>();
            LoginLogs = new List<LoginLog>();
        }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        //[Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// ��ȡ������ �û�����
        /// </summary>
        public MemberType MemberType
        {
            get { return (MemberType)MemberTypeNum; }
            set { MemberTypeNum = (int)value; }
        }

        /// <summary>
        /// ��ȡ������ ��ɫ���͵���ֵ��ʾ���������ݿ�洢
        /// </summary>
        public int MemberTypeNum { get; set; }

        /// <summary>
        /// ��ȡ������ �û���չ��Ϣ
        /// </summary>
        public virtual MemberExtend Extend { get; set; }

        /// <summary>
        ///  �ɷ�����Ʒ�Ĵ���
        /// </summary>
        //[DefaultValue(0)]
        public int PubishingEnableTimes { get; set; }

        /// <summary>
        /// ��ȡ������ ��ֵ��ʷ
        /// </summary>
        public virtual ICollection<RechargeHistory> RechargeHistories { get; set; }

        /// <summary>
        /// ��ȡ������ �û��µĽ�Ʒ
        /// </summary>
        public virtual ICollection<Prize> Prizes { get; set; }

        /// <summary>
        /// ��ȡ������ ���û�����ĳ齱
        /// </summary>
        public virtual ICollection<PrizeBetting> PrizeBettings { get; set; }

        /// <summary>
        /// ��ȡ������ ���û��н��Ľ������ֳ��齱��
        /// </summary>
        public virtual ICollection<LotteryResult> LotteryResults { get; set; }

        /// <summary>
        /// ��ȡ������ �û�ӵ�еĽ�ɫ��Ϣ����
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// ��ȡ������ �û���¼��¼����
        /// </summary>
        public virtual ICollection<LoginLog> LoginLogs { get; set; }
    }
}