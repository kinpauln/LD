using System.ComponentModel;


namespace LotteryDraw.Core.Models.Security
{
    /// <summary>
    /// 表示角色类型的枚举
    /// </summary>
    [Description("角色类型")]
    public enum RoleType
    {
        /// <summary>
        /// 个人类型
        /// </summary>
        [Description("个人")]
        Personal = 0,

        /// <summary>
        /// 企业类型
        /// </summary>
        [Description("企业")]
        Enterprise = 1,

        /// <summary>
        /// 管理员类型
        /// </summary>
        [Description("管理员")]
        Admin = 2
    }
}
