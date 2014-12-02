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
    [Description("��ƷͼƬ")]
    public class PrizePhoto : EntityBase<Guid>
    {
        public PrizePhoto()
        {
            Id = CombHelper.NewComb();
        }

        public string Name { get; set; }

        public int PhotoTypeNum { get; set; }

        public PhotoType PhotoType
        {
            get { return (PhotoType)PhotoTypeNum; }
            set { PhotoTypeNum = (int)value; }
        }

        public virtual Prize Prize { get; set; }
    }
}