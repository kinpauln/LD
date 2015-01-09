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
    [Description("�������")]
    public class LotteryResult : EntityBase<Guid>
    {
        public LotteryResult()
        {
            Id = CombHelper.NewComb();
        }

        public virtual Member Member { get; set; }

        /// <summary>
        /// ��ȡ������ ������Ϣ
        /// </summary>
        public virtual PrizeOrder PrizeOrder { get; set; }

        public LotteryResultState LotteryResultState
        {
            get { return (LotteryResultState)LotteryResultStateNum; }
            set { LotteryResultStateNum = (int)value; }
        }

        /// <summary>
        /// ��ȡ������ �н�״̬
        /// </summary>
        public int LotteryResultStateNum { get; set; }

        public int State { get; set; }
        
        /// <summary>
        /// ��ȡ������ �н�����
        /// </summary>
        public string SpeechAfterWinning { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}