// Դ�ļ�ͷ��Ϣ��
// <copyright file="MemberExtend.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core.Models
// ����޸ģ�������
// ����޸ģ�2013/05/20 13:43
// </copyright>

using System;
using System.ComponentModel;

using LotteryDraw.Component.Tools;


namespace LotteryDraw.Core.Models.Account
{
    /// <summary>
    ///     ʵ���ࡪ���û���չ��Ϣ
    /// </summary>
    [Description("�û���չ��Ϣ")]
    public class MemberExtend : EntityBase<Guid>
    {
        /// <summary>
        /// ��ʼ��һ�� �û���չʵ���� ����ʵ��
        /// </summary>
        public MemberExtend()
        {
            Id = CombHelper.NewComb();
            Address = new MemberAddress();
        }

        public string Tel { get; set; }

        /// <summary>
        ///  ͷ��
        /// </summary>
        public string HeadPortrait { get; set; }

        /// <summary>
        ///  ���Url
        /// </summary>
        public string AdvertisingUrl { get; set; }

        public MemberAddress Address { get; set; }

        public virtual Member Member { get; set; }
    }
}