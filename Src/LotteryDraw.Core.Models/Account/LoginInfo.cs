// Դ�ļ�ͷ��Ϣ��
// <copyright file="LoginInfo.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core.Models
// ����޸ģ�������
// ����޸ģ�2014/08/06 23:47
// </copyright>

namespace LotteryDraw.Core.Models.Account
{
    /// <summary>
    ///     ��¼��Ϣ��
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        ///     ��ȡ������ ��¼�˺�
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///     ��ȡ������ ��¼����
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     ��ȡ������ IP��ַ
        /// </summary>
        public string IpAddress { get; set; }
    }
}