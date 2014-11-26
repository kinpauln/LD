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
    ///     ʵ���ࡪ����Ʒ��Ϣ
    /// </summary>
    [Description("��ƷͶע")]
    public class PrizeBetting : EntityBase<Guid>
    {
        public PrizeBetting()
        {
            Id = CombHelper.NewComb();
        }

        public virtual Member Member { get; set; }

        /// <summary>
        /// ��ȡ������ ������Ϣ
        /// </summary>
        public PrizeOrder PrizeOrder { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// ��ȡ������ ��ѡ��
        /// </summary>
        [StringLength(10)]
        public string AnswerOption { get; set; }
    }
}