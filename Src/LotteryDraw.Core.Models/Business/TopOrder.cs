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
    [Description("�ö�")]
    public class TopOrder : EntityBase<Guid>
    {
        public TopOrder()
        {
            Id = CombHelper.NewComb();
        }

        public virtual PrizeOrder PrizeOrder { get; set; }

        /// <summary>
        /// ��ȡ������ �ͻ��ɷ�
        /// </summary>
        public virtual decimal PaymentAmout { get; set; }

        /// <summary>
        /// ��ȡ������ ��������
        /// </summary>
        public virtual int Sequence { get; set; }

        /// <summary>
        /// ��ȡ������ ��ֹ����
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// ��ȡ������ ��������
        /// </summary>
        public virtual DateTime? UpdateDate { get; set; }
        
        /// <summary>
        /// ��ȡ������ ������
        /// </summary>
        public virtual Member Operator { get; set; }
    }
}