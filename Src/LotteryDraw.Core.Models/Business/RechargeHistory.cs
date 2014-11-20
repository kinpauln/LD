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
using System;
using LotteryDraw.Core.Models.Account;


namespace LotteryDraw.Core.Models.Business
{
    /// <summary>
    ///     ʵ���ࡪ����Ա��ֵ��ʷ
    /// </summary>
    [Description("��Ա��ֵ��ʷ")]
    public class RechargeHistory : EntityBase<Guid>
    {
        public RechargeHistory()
        {
            Id = CombHelper.NewComb();
        }

        public virtual Member Member { get; set; }

        /// <summary>
        ///  ��ֵ���
        /// </summary>
        public double MoneyValue { get; set; }

        /// <summary>
        ///  ��������
        /// </summary>
        [DefaultValue(0)]
        public int PubTimes { get; set; }

        /// <summary>
        ///  ��ֵ������
        /// </summary>
        [DefaultValue(0)]
        public Member Operator { get; set; }
    }
}