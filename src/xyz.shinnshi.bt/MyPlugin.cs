using Flexlive.CQP.Framework;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;


public class Master
{
    private static long masterQQ;
    public long QQ
    {
        get { return masterQQ; }
        set { masterQQ = value; }
    }
}

namespace Flexlive.CQP.CSharpPlugins.Demo
{
    /// <summary>
    /// 酷Q C#版插件Demo
    /// </summary>
    /// 
        public class MyPlugin : CQAppAbstract
        {
        /// <summary>
        /// 应用初始化，用来初始化应用的基本信息。
        /// </summary>
        /// 
        int num = 5; //搜索条数
        bool OutOfNumber = false;
        long Group = 0;
        public override void Initialize()
        {
            // 此方法用来初始化插件名称、版本、作者、描述等信息，
            // 不要在此添加其它初始化代码，插件初始化请写在Startup方法中。

            this.Name = "磁力搜索";
            this.Version = new Version("1.0.0.0");
            this.Author = "shinnshi";
            this.Description = "基于Flexlive版酷Q的磁力搜索插件。\n\n指令：\n%搜索888888\n%更改条数5\n\n管理员指令：\n%群搜索开\n%群搜索关";
        }

        /// <summary>
        /// 应用启动，完成插件线程、全局变量等自身运行所必须的初始化工作。
        /// </summary>
        public override void Startup()
        {
            //完成插件线程、全局变量等自身运行所必须的初始化工作。

            Master master = new Master();
            master.QQ = 734962820;

        }
        /// <summary>
        /// 打开设置窗口。
        /// </summary>
        public override void OpenSettingForm()
        {
            // 打开设置窗口的相关代码。
            FormSettings frm = new FormSettings();
            frm.ShowDialog();
        }

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        System.Windows.Forms.WebBrowser wb = new System.Windows.Forms.WebBrowser();
        /// <summary>
        /// Type=21 私聊消息。
        /// </summary>
        /// <param name="subType">子类型，11/来自好友 1/来自在线状态 2/来自群 3/来自讨论组。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        /// 

        List<long> grouplist = new List<long>();

        public override void PrivateMessage(int subType, int sendTime, long fromQQ, string msg, int font)
        {
            if (msg.Contains("%更改条数")) {
                bool IsNume = int.TryParse(msg.Replace("%更改条数", ""), out num);
                if (!IsNume)
                {
                    num = 5;
                    CQ.SendPrivateMessage(fromQQ, "更改失败，请确认指令“%更改条数5”。\n搜索条数已还原回5条。");
                }
                else if(num>15 || num <1){
                    num = 5;
                    CQ.SendPrivateMessage(fromQQ, "更改失败，请输入大于1小于15的数字。\n搜索条数已还原回5条。");
                }
                else {
                    CQ.SendPrivateMessage(fromQQ, "搜索条数以更改为" + num + "条");
                }   
            }
            if (msg.Contains("%搜索"))
            {
                CQ.SendPrivateMessage(fromQQ, "少女祈祷中……");
                string URL = "https://www.bturl.cc/search/" + msg.Replace("%搜索", "") + "_click_1.html";

                try
                {
                    System.Net.WebClient myWebClient = new System.Net.WebClient();
                    myWebClient.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)");
                    byte[] myDataBuffer = myWebClient.DownloadData(URL);
                    string SourceCode = Encoding.GetEncoding("utf-8").GetString(myDataBuffer);
                    string temp = SourceCode;
                    for (int i = 0; i < num; i++) {
                        string magnet = GetPathPoints(temp, "/" + @"(\w{40})" + ".html", i);
                        string title = GetPathPoints(temp, "(?<=("+ "item-list" + "))[.\\s\\S]*?(?=(" + "</div>" + "))", i);
                        string times = GetPathPoints(temp, "(?<=(" + "创建日期：" + "))[.\\s\\S]*?(?=(" + "</span>" + "))", i);
                        string size = GetPathPoints(temp, "(?<=(" + "文件大小：" + "))[.\\s\\S]*?(?=(" + "</span>" + "))", i);
                        string hot = GetPathPoints(temp, "(?<=(" + "访问热度：" + "))[.\\s\\S]*?(?=(" + "</span>" + "))", i);
                        if (magnet != null) {
                            CQ.SendPrivateMessage(fromQQ, "【标题】"+title.Replace(">", "").Replace("\n","").Replace("\r","")+@"""" + "\n创建日期：" + times.Replace(@"""", "").Replace("<span>", "")+
                                "\n文件大小：" +size.Replace(@"""","").Replace("<span>","")+ "\n访问热度：" + hot.Replace(@"""", "").Replace("<span>", "") +
                                "\nmagnet:?xt=urn:btih:" + magnet.Replace("/", "").Replace(".html", ""));
                            OutOfNumber = true;
                        }
                    }
                    
                    //string title = GetPathPoints(temp, "<div class=\"item - list\">"+ @"(\w{100})"+"</div>");
                    //CQ.SendPrivateMessage(fromQQ,title.Replace("<div class=\"item - list\">","").Replace("</div>","") + "magnet:?xt=urn:btih:" + temp2.Replace("/", "").Replace(".html", ""));
                    
                }
                catch (Exception e)
                {   if(!OutOfNumber)
                        CQ.SendPrivateMessage(fromQQ, "[查询失败]\r\n" + e.ToString());
                }
                if (!OutOfNumber)
                    CQ.SendPrivateMessage(fromQQ, "无结果");
                OutOfNumber = false;
            }
        }

        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="fromAnonymous">来源匿名者。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void GroupMessage(int subType, int sendTime, long fromGroup, long fromQQ, string fromAnonymous, string msg, int font)
        {
            //string Name = "";
            //decimal Money = 0;
            //string UPUser = "";
            //long QQ = 0, QQ2 = 0;
            //string Date;
            Master master = new Master();
            if (msg.Contains("%群搜索开") && fromQQ == master.QQ) {
                grouplist.Add(fromGroup);
                CQ.SendGroupMessage(fromGroup, "该群群搜索已开。");
            }
            if (msg.Contains("%群搜索关") && fromQQ == master.QQ)
            {
                for (int i = grouplist.Count - 1; i >= 0; i--)
                {
                    if (grouplist[i].Equals(fromGroup))
                    {
                        grouplist.Remove(grouplist[i]);
                    }
                }
                CQ.SendGroupMessage(fromGroup, "该群群搜索已关。");
            }


            var groupMember = CQ.GetGroupMemberInfo(fromGroup, fromQQ);
            if (msg.Contains("%更改条数"))
            {
                bool IsNume = int.TryParse(msg.Replace("%更改条数", ""), out num);
                if (!IsNume)
                {
                    num = 5;
                    CQ.SendGroupMessage(fromGroup, "更改失败，请确认指令“%更改条数5”。\n搜索条数已还原回5条。");
                }
                else if (num > 15 || num < 1)
                {
                    num = 5;
                    CQ.SendGroupMessage(fromGroup, "更改失败，请输入大于1小于15的数字。\n搜索条数已还原回5条。");
                }
                else
                {
                    CQ.SendGroupMessage(fromGroup, "搜索条数以更改为" + num + "条");
                }
            }
            if (msg.Contains("%搜索"))
            {
                bool on = false;
                for (int i = grouplist.Count - 1; i >= 0; i--)
                {
                    if (grouplist[i].Equals(fromGroup))
                    {
                        on = true;
                        break;
                    }
                }
                if (!on)
                {
                    CQ.SendGroupMessage(fromGroup, "该群群搜索已关。");
                    return;
                }
                CQ.SendGroupMessage(fromGroup, "少女祈祷中……");
                string URL = "https://www.bturl.cc/search/" + msg.Replace("%搜索", "") + "_click_1.html";

                try
                {
                    System.Net.WebClient myWebClient = new System.Net.WebClient();
                    myWebClient.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)");
                    byte[] myDataBuffer = myWebClient.DownloadData(URL);
                    string SourceCode = Encoding.GetEncoding("utf-8").GetString(myDataBuffer);
                    string temp = SourceCode;
                    for (int i = 0; i < num; i++)
                    {
                        string magnet = GetPathPoints(temp, "/" + @"(\w{40})" + ".html", i);
                        if (magnet != null) {
                            CQ.SendGroupMessage(fromGroup, "magnet:?xt=urn:btih:" + magnet.Replace("/", "").Replace(".html", ""));
                            OutOfNumber = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!OutOfNumber)
                        CQ.SendGroupMessage(Group, "[查询失败]\r\n" + e.ToString());
                }
                if (!OutOfNumber)
                    CQ.SendPrivateMessage(fromQQ, "无结果");
                OutOfNumber = false;
            }
        }

        private string GetPathPoints(string value, string regx,int i)
        {
            if (value == "")
                return null;
            bool isMatch = System.Text.RegularExpressions.Regex.IsMatch(value, regx);
            if (!isMatch)
                return null;
            System.Text.RegularExpressions.MatchCollection matchCol = System.Text.RegularExpressions.Regex.Matches(value, regx);
            string result = matchCol[i].Value;
            return result;
        }

        

        /// <summary>
        /// Type=4 讨论组消息。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromDiscuss">来源讨论组。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void DiscussMessage(int subType, int sendTime, long fromDiscuss, long fromQQ, string msg, int font)
        {
            // 处理讨论组消息。
        }

        /// <summary>
        /// Type=11 群文件上传事件。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="file">上传文件信息。</param>
        public override void GroupUpload(int subType, int sendTime, long fromGroup, long fromQQ, string file)
        {
            // 处理群文件上传事件。
        }

        /// <summary>
        /// Type=101 群事件-管理员变动。
        /// </summary>
        /// <param name="subType">子类型，1/被取消管理员 2/被设置管理员。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupAdmin(int subType, int sendTime, long fromGroup, long beingOperateQQ)
        {
            // 处理群事件-管理员变动。
        }

        /// <summary>
        /// Type=102 群事件-群成员减少。
        /// </summary>
        /// <param name="subType">子类型，1/群员离开 2/群员被踢 3/自己(即登录号)被踢。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员减少。
        }

        /// <summary>
        /// Type=103 群事件-群成员增加。
        /// </summary>
        /// <param name="subType">子类型，1/管理员已同意 2/管理员邀请。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员增加。
        }

        /// <summary>
        /// Type=201 好友事件-好友已添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        public override void FriendAdded(int subType, int sendTime, long fromQQ)
        {
            // 处理好友事件-好友已添加。
        }

        /// <summary>
        /// Type=301 请求-好友添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddFriend(int subType, int sendTime, long fromQQ, string msg, string responseFlag)
        {
            // 处理请求-好友添加。
        }

        /// <summary>
        /// Type=302 请求-群添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddGroup(int subType, int sendTime, long fromGroup, long fromQQ, string msg, string responseFlag)
        {
            // 处理请求-群添加。
        }
    }
}
