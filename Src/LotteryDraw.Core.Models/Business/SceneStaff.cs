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
    ///     ʵ���ࡪ���ֳ��齱��Ա����
    /// </summary>
    [Description("�ֳ��齱��Ա")]
    public class SceneStaff : EntityBase<Guid>
    {
        public SceneStaff()
        {
            Id = CombHelper.NewComb();
        }

        [Required]
        public string Value { get; set; }

        [DefaultValue(false)]
        public bool IsLucky { get; set; }

        /// <summary>
        /// �ֳ��齱�н���Ա����״̬
        /// </summary>
        public LuckySceneStaffState LuckySceneStaffState
        {
            get { return (LuckySceneStaffState)LuckySceneStaffStateNum; }
            set { LuckySceneStaffStateNum = (int)value; }
        }

        /// <summary>
        /// ��ȡ������ ����״̬����ֵ��ʾ���������ݿ�洢
        /// </summary>
        public int LuckySceneStaffStateNum { get; set; }

        /// <summary>
        /// ��ȡ������ ������Ϣ
        /// </summary>
        public virtual PrizeOrder PrizeOrder { get; set; }
    }
}