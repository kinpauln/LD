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
    ///     ʵ���ࡪ����Ʒ������Ϣ
    /// </summary>
    [Description("��Ʒ������Ϣ")]
    public class PrizeOrder : EntityBase<Guid>
    {
        public PrizeOrder()
        {
            //Id = CombHelper.NewComb();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public RevealType RevealType
        {
            get { return (RevealType)RevealTypeNum; }
            set { RevealTypeNum = (int)value; }
        }

        /// <summary>
        /// ��ȡ������ �齱���͵���ֵ��ʾ���������ݿ�洢
        /// </summary>
        public int RevealTypeNum { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public RealState RealState
        {
            get { return (RealState)RevealStateNum; }
            set { RevealStateNum = (int)value; }
        }

        /// <summary>
        /// ��ȡ������ ����״̬����ֵ��ʾ���������ݿ�洢
        /// </summary>
        public int RevealStateNum { get; set; }

        public virtual Prize Prize { get; set; }

        public virtual PrizeOrderExtend Extend { get; set; }
    }
}