// Դ�ļ�ͷ��Ϣ��
// <copyright file="Role.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core.Models
// ����޸ģ�������
// ����޸ģ�2014/08/07 15:26
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Models.Account;


namespace LotteryDraw.Core.Models.Security
{
    /// <summary>
    ///     ʵ���ࡪ����ɫ��Ϣ
    /// </summary>
    [Description("��ɫ��Ϣ")]
    public class Role : EntityBase<Int64>
    {
        public Role()
        {
        }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// ��ȡ������ ��ɫ����
        /// </summary>
        public RoleType RoleType
        {
            get { return (RoleType)RoleTypeNum; }
            set { RoleTypeNum = (int)value; }
        }

        /// <summary>
        /// ��ȡ������ ��ɫ���͵���ֵ��ʾ���������ݿ�洢
        /// </summary>
        public int RoleTypeNum { get; set; }

        /// <summary>
        ///     ��ȡ������ ӵ�д˽�ɫ���û���Ϣ����
        /// </summary>
        public virtual ICollection<Member> Members { get; set; }
    }
}
