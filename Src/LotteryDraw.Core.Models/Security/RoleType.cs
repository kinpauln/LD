using System.ComponentModel;


namespace LotteryDraw.Core.Models.Security
{
    /// <summary>
    /// ��ʾ��ɫ���͵�ö��
    /// </summary>
    [Description("��ɫ����")]
    public enum RoleType
    {
        /// <summary>
        /// ��������
        /// </summary>
        [Description("����")]
        Personal = 0,

        /// <summary>
        /// ��ҵ����
        /// </summary>
        [Description("��ҵ")]
        Enterprise = 1,

        /// <summary>
        /// ����Ա����
        /// </summary>
        [Description("����Ա")]
        Admin = 2
    }
}
