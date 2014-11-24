using System.ComponentModel.DataAnnotations;


namespace LotteryDraw.Core.Models.Account
{
    /// <summary>
    /// 用户地址信息
    /// </summary>
    public class MemberAddress
    {
        //[StringLength(10)]
        public string Province { get; set; }

        //[StringLength(20)]
        public string City { get; set; }

        //[StringLength(20)]
        public string Town { get; set; }

        //[StringLength(60)]
        public string Suffix { get; set; }
        
        public override string ToString()
        {
            return Province + City + Town + Suffix;
        }
    }
}
