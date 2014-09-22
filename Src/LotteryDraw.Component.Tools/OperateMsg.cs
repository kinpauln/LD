using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryDraw.Component.Tools
{
    /// <summary>
    /// Desc : 以单例模式提供系统公共的提示信息
    /// </summary>
    public class OperateMsg
    {
        /// <summary>
        /// OperateMsg静态实例对象
        /// </summary>
        private static OperateMsg Msg = null;

        /// <summary>
        /// 存放提示信息的msgMap对象
        /// </summary>
        private Dictionary<string, string> msgMap = new Dictionary<string, string>();


        /// <summary>
        /// 存放id的Map对象
        /// </summary>
        private Dictionary<string, string> htmlIdsMap = new Dictionary<string, string>();

        /// <summary>
        /// 密码修改操作成功Key
        /// </summary>
        public static readonly string PWD_EDIT = "PwdEdit";

        /// <summary>
        /// 私有构造函数,初始化提示信息对象
        /// </summary>
        private OperateMsg()
        {
            //默认提示信息
            msgMap.Add("default", "Info:信息提示");

            //添加事例：
            msgMap.Add("xxxAdd", "xx信息添加成功，返回<a href='url'>继续添加</a> or <a href='url'>xx一览</a>");
            msgMap.Add("xxxEdit", "xx信息修改成功，返回<a href='url'>xx一览</a>");
            msgMap.Add("xxxDelete", "xx信息删除成功，返回<a href='url'>xx一览</a>");


            //办事内容管理
            msgMap.Add(FIRM_ADD, "公司信息添加成功，返回<a href='/SysFirm/FirmList'>公司信息管理查询</a> or <a href='/SysFirm/AddSysFirm'>继续添加</a>");
            msgMap.Add(FIRM_DEL, "公司信息删除成功，返回<a href='/SysFirm/FirmList'>公司内容管理查询</a>");
            msgMap.Add(FIRM_EDIT, "公司信息修改成功，返回<a href='/SysFirm/FirmList'>公司内容管理查询</a>");

            htmlIdsMap.Add(FIRM_ADD, "addFirmMsg");
            htmlIdsMap.Add(FIRM_DEL, "FirmListMsg");
            htmlIdsMap.Add(FIRM_EDIT, "FirmListMsg");
        }


        /// <summary>
        /// 获取单例提示信息对象
        /// </summary>
        /// <returns>OperateMsg</returns>
        public static OperateMsg getInstance()
        {
            //如果初始信息对象为空的场合进行构造
            if (Msg == null)
            {

                Msg = new OperateMsg();
            }
            return Msg;
        }


        /// <summary>
        /// 根据key获取操作提示信息
        /// </summary>
        /// <param name="key">操作提示信息对应的key</param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return msgMap.ContainsKey(key) ? msgMap[key] : msgMap["default"];
        }

        /// <summary>
        /// 根据key获取id
        /// </summary>
        /// <param name="key">操作key</param>
        /// <returns></returns>
        public string GetIds(string key)
        {
            return htmlIdsMap.ContainsKey(key) ? htmlIdsMap[key] : "";
        }

        /// <summary>
        /// 根据key获取操作提示信息
        /// </summary>
        /// <param name="key">操作提示信息对应的key</param>
        /// <param name="param">提示信息中对应的参数</param>
        /// <returns></returns>
        public string GetValue(string key, string[] param)
        {
            string msg = msgMap.ContainsKey(key) ? msgMap[key] : msgMap["default"];
            return String.Format(msg, param);
        }


        #region 公司管理Key

        public static readonly string FIRM_ADD = "AddFirm";

        public static readonly string FIRM_DEL = "DelFirm";

        public static readonly string FIRM_EDIT = "EditFirm";       
        
        #endregion
    }
}