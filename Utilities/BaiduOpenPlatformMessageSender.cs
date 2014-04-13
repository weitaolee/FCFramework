namespace FC.Framework.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Net;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq; 
    using System.Web;
    using Newtonsoft.Json;

    public static class BaiduOpenPlatformMessageSender
    {
        public static void SendMessage(string title, string msg, string userID, string channelID)
        {
            Check.Argument.IsNotEmpty(userID, "userID");
            Check.Argument.IsNotEmpty(channelID, "channelID");

            string sk = "aagB9W7SNUrs5MBWhhSIHu7ca5UobFqq";
            string ak = "izmfeOZnPI27jTRSNFoPPfI7";


            BaiduPush Bpush = new BaiduPush("POST", sk);
            String apiKey = ak;
            String messages = string.Empty;
            String method = "push_msg";
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            uint unixTime = (uint)ts.TotalSeconds;

            uint message_type;
            string messageksy = title;

            message_type = 1;
            BaiduPushNotification notification = new BaiduPushNotification();
            notification.title = title;
            notification.description = msg;
            messages = notification.getJsonString().Replace("\"", "'");



            PushOptions pOpts;
            //单播

            pOpts = new PushOptions(method, apiKey, userID, channelID, messages, messageksy, unixTime);


            pOpts.message_type = message_type;

            string response = Bpush.PushMessage(pOpts);
        }

        #region 百度Push
        public class BaiduPush
        {
            public PushOptions opts { get; set; }

            public string httpMehtod { get; set; }
            public string url { get; set; }
            public string secret_key { get; set; }

            public BaiduPush(string httpMehtod, string secret_key)
            {
                this.httpMehtod = httpMehtod;
                this.url = "http://channel.api.duapp.com/rest/2.0/channel/channel";
                this.secret_key = secret_key;
            }


            public string PushMessage(PushOptions opts)
            {

                this.opts = opts;

                Dictionary<string, string> dic = new Dictionary<string, string>();

                //将键值对按照key的升级排列
                var props = typeof(PushOptions).GetProperties().OrderBy(p => p.Name);
                foreach (var p in props)
                {
                    if (p.GetValue(this.opts, null) != null)
                    {
                        dic.Add(p.Name, p.GetValue(this.opts, null).ToString());
                    }
                }
                //生成sign时，不能包含sign标签，所有移除
                dic.Remove("sign");

                var preData = new StringBuilder();
                foreach (var l in dic)
                {
                    preData.Append(l.Key);
                    preData.Append("=");
                    preData.Append(l.Value);

                }

                //按要求拼接字符串，并urlencode编码
                var str = HttpUtility.UrlEncode(this.httpMehtod.ToUpper() + this.url + preData.ToString() + this.secret_key);

                var strSignUpper = new StringBuilder();
                int perIndex = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    var c = str[i].ToString();
                    if (str[i] == '%')
                    {
                        perIndex = i;
                    }
                    if (i - perIndex == 1 || i - perIndex == 2)
                    {
                        c = c.ToUpper();
                    }
                    strSignUpper.Append(c);
                }

                var sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strSignUpper.ToString(), "MD5").ToLower();

                //加入生成好的sign键值对
                dic.Add("sign", sign);
                var strb = new StringBuilder();
                //int tagIndex = 0;
                foreach (var l in dic)
                {

                    strb.Append(l.Key);
                    strb.Append("=");
                    strb.Append(l.Value);
                    strb.Append("&");
                }

                var postStr = strb.ToString().EndsWith("&") ? strb.ToString().Remove(strb.ToString().LastIndexOf('&')) : strb.ToString();


                byte[] data = Encoding.UTF8.GetBytes(postStr);//编码，尤其是汉字，事先要看下抓取网页的编码方式  
                WebClient webClient = new WebClient();
                try
                {
                    webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                    byte[] responseData = webClient.UploadData(this.url, "POST", data);//得到返回字符流  
                    string srcString = Encoding.UTF8.GetString(responseData);//解码  
                    return "Post:" + postStr + "\r\n\r\n" + "Response:" + srcString;
                }
                catch (WebException ex)
                {
                    Stream stream = ex.Response.GetResponseStream();
                    string m = ex.Response.Headers.ToString();
                    byte[] buf = new byte[256];
                    stream.Read(buf, 0, 256);
                    stream.Close();
                    int count = 0;
                    foreach (var b in buf)
                    {
                        if (b > 0)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return "Post:" + postStr + ex.Message + "\r\n\r\n" + Encoding.UTF8.GetString(buf, 0, count);
                }
            }
        }
        #endregion

        #region 推送参数
        public class PushOptions
        {
            public PushOptions(string method, string apikey, string user_id, uint push_type,
               string channel_id, string tag, uint device_type, uint message_type, string messages,
                string msg_keys, uint message_expires, uint timestamp, uint expires, uint v)
            {
                this.method = method;
                this.apikey = apikey;
                this.user_id = user_id;
                this.push_type = push_type;
                this.channel_id = channel_id;
                this.tag = tag;
                this.device_type = device_type;
                this.message_type = message_type;
                this.messages = messages;
                this.msg_keys = msg_keys;
                this.message_expires = message_expires;
                this.timestamp = timestamp;

                this.expires = expires;
                this.v = v;

            }

            //单播
            public PushOptions(string method, string apikey, string user_id, string channel_id,
                               string messages, string msg_keys, uint timestamp)
            {
                this.method = method;
                this.apikey = apikey;
                this.user_id = user_id;
                this.channel_id = channel_id;
                this.push_type = 1;
                this.messages = messages;
                this.msg_keys = msg_keys;
                this.timestamp = timestamp;

                device_type = 3;
            }
            //组播
            public PushOptions(string method, string apikey, string tag, string messages, string msg_keys, uint timestamp)
            {
                this.method = method;
                this.apikey = apikey;
                this.tag = tag;
                this.push_type = 2;
                this.messages = messages;
                this.msg_keys = msg_keys;
                this.timestamp = timestamp;

                device_type = 3;
            }

            //广播
            public PushOptions(string method, string apikey, string messages, string msg_keys, uint timestamp)
            {
                this.method = method;
                this.apikey = apikey;
                this.push_type = 3;
                this.messages = messages;
                this.msg_keys = msg_keys;
                this.timestamp = timestamp;

                device_type = 3;
            }

            public string method { get; set; } 	//string 	是 	方法名，必须存在：push_msg。
            public string apikey { get; set; }	//string 	是 	访问令牌，明文AK，可从此值获得App的信息，配合sign中的sk做合法性身份认证。
            public string user_id { get; set; }	//string 	否 	用户标识，在Android里，channel_id + userid指定某一个特定client。不超过256字节，如果存在此字段，则只推送给此用户。
            public uint push_type { get; set; } /*	uint 	是 	推送类型，取值范围为：1～3

                                                                1：单个人，必须指定user_id 和 channel_id （指定用户的指定设备）或者user_id（指定用户的所有设备）

                                                                2：一群人，必须指定 tag

                                                                3：所有人，无需指定tag、user_id、channel_id*/
            public string channel_id { get; set; }	//string 	否 	通道标识
            public string tag { get; set; }	//string 	否 	标签名称，不超过128字节
            public uint? device_type { get; set; }	/*uint 	否 	设备类型，取值范围为：1～5

                                        百度Channel支持多种设备，各种设备的类型编号如下：（支持某种组合，如：1,2,4:）

                                        1：浏览器设备；

                                        2：PC设备；

                                        3：Andriod设备；

                                        4：iOS设备；

                                        5：Windows Phone设备；

                                        如果存在此字段，则向指定的设备类型推送消息。 默认不区分设备类型。*/
            public uint? message_type { get; set; }	/*uint 	否 	消息类型

                                                    0：消息（透传）

                                                    1：通知

                                                    默认值为0。*/
            public string messages { get; set; } 	/*string 	是 	指定消息内容，单个消息为单独字符串，多个消息用json数组表示。

                                                    如果有二进制的消息内容，请先做BASE64的编码。

                                                    一次推送最多10个消息。

                                                    注：当message_type=1且为Android端接收消息时，需按照以下格式：

                                                    "{ 
                                                       \"title\" : \"hello\" ,
                                                       \"description\": \"hello\"
                                                     }"

                                                    说明：

                                                        title : 通知标题，可以为空；如果为空则设为appid对应的Android应用名称。
                                                        description：通知文本内容，不能为空，否则Android端上不展示。 */

            public string msg_keys { get; set; } 	//string 	是 	指定消息标识，必须和messages一一对应。相同消息标识的消息会自动覆盖。单个消息为单独字符串，多个msg_key也使用json数组表示。特别提醒：该功能只支持android、browser、pc三种设备类型。。
            public uint? message_expires { get; set; }	//uint 	否 	指定消息的过期时间，默认为86400秒。必须和messages一一对应。
            public uint timestamp { get; set; }	//uint 	是 	用户发起请求时的unix时间戳。本次请求签名的有效时间为该时间戳+10分钟。
            public string sign { get; set; } 	//string 	是 	调用参数签名值，与apikey成对出现。
            public uint? expires { get; set; }//	uint 	否 	用户指定本次请求签名的失效时间。格式为unix时间戳形式。
            public uint? v { get; set; }	//uint 	否 	API版本号，默认使用最高版本。
        }
        #endregion

        #region 通知样式定义

        public class BaiduPushNotification
        {
            public string title { get; set; } //通知标题，可以为空；如果为空则设为appid对应的应用名;
            public string description { get; set; } //通知文本内容，不能为空;
            public int notification_builder_id { get; set; } //android客户端自定义通知样式，如果没有设置默认为0;
            public int notification_basic_style { get; set; } //只有notification_builder_id为0时才有效，才需要设置，如果notification_builder_id为0则可以设置通知的基本样式包括(响铃：0x04;振动：0x02;可清除：0x01;),这是一个flag整形，每一位代表一种样式;
            public int open_type { get; set; }//点击通知后的行为(打开Url：1; 自定义行为：2：其它值则默认打开应用;);
            public string url { get; set; } //只有open_type为1时才有效，才需要设置，如果open_type为1则可以设置需要打开的Url地址;
            public int user_confirm { get; set; } //只有open_type为1时才有效，才需要设置,(需要请求用户授权：1；默认直接打开：0), 如果open_type为1则可以设置打开的Url地址时是否请求用户授权;
            public string pkg_content { get; set; }//只有open_type为2时才有效，才需要设置, 如果open_type为2则可以设置自定义打开行为(具体参考管理控制台文档);
            public string custom_content { get; set; }// 自定义内容，键值对，Json对象形式(可选)；在android客户端，这些键值对将以Intent中的extra进行传递。

            public BaiduPushNotification()
            {
                notification_builder_id = 0;
                notification_basic_style = 0x04 | 0x02;

                url = "";
                user_confirm = 0;
                pkg_content = "";
                custom_content = "";
                open_type = 2;
                pkg_content = "intent:#Intent;launchFlags=0x10000000;component=com.fundsupervision.push.CustomActivity";

            }

            public string getJsonString()
            {
                var settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                string jsonData = JsonConvert.SerializeObject(this, Formatting.Indented, settings);

                return jsonData;
            }
        }
        #endregion
    }
}
