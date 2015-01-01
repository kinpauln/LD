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
    ///     ʵ���ࡪ����Ʒ������չ��Ϣ
    /// </summary>
    [Description("��Ʒ������չ��Ϣ")]
    public class PrizeOrderExtend : EntityBase<Guid>
    {
        public PrizeOrderExtend()
        {
            //Id = CombHelper.NewComb();
            PrizeAsking = new PrizeAsking();
        }

        /// <summary>
        /// �齱��Χ���
        /// </summary>
        public ScopeType ScopeType
        {
            get { return (ScopeType)ScopeTypeNum; }
            set { ScopeTypeNum = (int)value; }
        }

        /// <summary>
        /// �齱��Χ���
        /// </summary>
        public int ScopeTypeNum { get; set; }

        /// <summary>
        /// �齱��Χ(������)
        /// </summary>
        public string ScopeCity { get; set; }

        /// <summary>
        ///  ����ʱ��
        /// </summary>
        public DateTime? LaunchTime { get; set; }

        /// <summary>
        ///  �н��������
        /// </summary>
        [DefaultValue(1)]
        public int MinLuckyCount { get; set; }

        /// <summary>
        ///  ������
        /// </summary>
        public float? LuckyPercent { get; set; }

        /// <summary>
        ///  ���ش�С
        /// </summary>
        public int? PoolCount { get; set; }

        /// <summary>
        ///  �н�����
        /// </summary>
        public int? LuckyCount { get; set; }
        
        /// <summary>
        /// ��ȡ������ �˷�
        /// </summary>
        public virtual decimal Freight { get; set; }

        /// <summary>
        ///  ��ע
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// ��ȡ������ ���¿��������������͵���ֵ��ʾ���������ݿ�洢
        /// </summary>
        public int AnswerRevealConditionTypeNum { get; set; }

        /// <summary>
        /// ���¿�����������
        /// </summary>
        public AnswerRevealConditionType AnswerRevealConditionType
        {
            get { return (AnswerRevealConditionType)AnswerRevealConditionTypeNum; }
            set { AnswerRevealConditionTypeNum = (int)value; }
        }

        /// <summary>
        ///  �ʴ�
        /// </summary>
        public PrizeAsking PrizeAsking { get; set; }
        
        public virtual PrizeOrder PrizeOrder { get; set; }
    }
}