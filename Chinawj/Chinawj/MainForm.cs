using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace LeShou
{
    public delegate void FlushClient();//代理
    public partial class login_form : Form
    {
        private Dictionary<string, string> cookies = new Dictionary<string, string>();

        public Object Cookies
        {
            get
            {
                string cookie = "";
                foreach (string key in cookies.Keys)
                {
                    cookie += (key + "=" + cookies[key] + ";");
                }
                return cookie;
            }
            set
            {
                foreach (string key in ((Dictionary<string, string>)value).Keys)
                {
                    cookies[key] = ((Dictionary<string, string>)value)[key];
                }
            }
        }
        //System.Runtime.Serialization.Formatter;
        private HtmlDocument loginform = null;
        /// <summary>
        /// 验证码地址
        /// </summary>
        private static string code_url = "";
        /// <summary>
        /// 登录页地址
        /// </summary>
        private static string login_page_url = "http://my.chinawj.com.cn/";
        /// <summary>
        /// 登录地址
        /// </summary>
        private static string login_url = "http://my.chinawj.com.cn/member/logins.php?act=logins";
        /// <summary>
        /// 删除页地址
        /// </summary>
        private static string delete_page_url = "http://my.chinawj.com.cn/member/logistics/?dc=1";
        /// <summary>
        /// 最大删除页面
        /// </summary>
        private static int max_delete_page_num = 999999999;
        /// <summary>
        /// 删除地址
        /// </summary>
        private static string delete_url = "http://my.chinawj.com.cn/member/logistics/?act=del";
        /// <summary>
        /// 每页条数
        /// </summary>
        private static int page_size = 30;
        /// <summary>
        /// 验证码图片路径
        /// </summary>
        private static string cookie_path = ".co";
        private int bar_num = 0;
        CookieContainer cc = new CookieContainer();

        public login_form()
        {
            InitializeComponent();
            LoadCookie();
            //CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 序列化cookie python --> pickle
        /// </summary>
        public void DumpCookie()
        {
            FileStream fileStream = new FileStream(cookie_path, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, cc);
            fileStream.Close();
        }
        /// <summary>
        /// 反序列化cookie DeSerialize
        /// </summary>
        public void LoadCookie()
        {
            if (File.Exists(cookie_path))
            {
                FileStream fileStream = new FileStream(cookie_path, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter b = new BinaryFormatter();
                try
                {
                    cc = b.Deserialize(fileStream) as CookieContainer;
                    fileStream.Close();
                }
                catch (System.Runtime.Serialization.SerializationException)
                {
                    check_cookie();
                }
            }
        }
        public void check_cookie()
        {
            //TODO:检查cookie是否可用
        }

        private void login_form_Load(object sender, EventArgs e)
        {
            //init();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            login();
        }
        /// <summary>
        /// 删除
        /// </summary>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Delete_Flush_Thread);
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void login()
        {
            string USERID = this.tbx_userid.Text;
            string PWD = this.tbx_pwd.Text;
            byte[] data = Encoding.UTF8.GetBytes("UsernameGet=" + USERID + "&pwd=" + PWD + "&submit=");
            byte[] response = RequestByCookie(login_url, (string)Cookies, false, data);
            string html = BytesToString(response);
            if (html == "<script>top.location=\"http://my.chinawj.com.cn/member/index.php\"</script>")
            {
                DumpCookie();
                MessageBox.Show("登录成功!");
            }
        }

        /// <summary>检查帐号 密码 验证码</summary>
        /// <returns>True/False</returns>
        public bool check()
        {
            bool state = true;
            string USERID = this.tbx_userid.Text;
            string PWD = this.tbx_pwd.Text;
            string CODE = this.tbx_code.Text;
            if (USERID.Length < 16)
            {
                MessageBox.Show("请重新输入帐号!");
                state = false;
            }
            if (PWD.Length < 6)
            {
                MessageBox.Show("请重新输入密码!");
                state = false;
            }
            //if (CODE.Length != 5)
            //{
            //    MessageBox.Show("请重新输入验证码!");
            //    state = false;
            //}
            return state;
        }
        /// <summary>
        /// 将字节列表 保存为图片
        /// </summary>
        //public void save_image_by_list_byte()
        //{
        //    byte[] byte_list = RequestByCookie(code_url, "");
        //    File.WriteAllBytes(code_path, byte_list.ToArray());
        //}

        /// <summary>
        /// request 请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="cookie">Cookies值</param>
        /// <param name="is_get">request maehod.is get? or false:post.</param>
        public byte[] RequestByCookie(string url, string cookie, bool is_get = true, byte[] data = null)
        {
            List<byte> list = new List<byte>();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //属性配置
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            webRequest.MaximumResponseHeadersLength = -1;
            webRequest.CookieContainer = cc;
            //if (cookie != "")
            //{
            //    webRequest.Headers.Add("cookie", cookie);
            //}
            if (is_get == true) webRequest.Method = "GET";
            else webRequest.Method = "POST";
            webRequest.KeepAlive = true;
            if (is_get == false && data != null)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
                //写入请求流
                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                //获取服务器返回的资源
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    //webResponse.Cookies;
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        Dictionary<string, string> cd = new Dictionary<string, string>();
                        foreach (Cookie ck in webRequest.CookieContainer.GetCookies(new Uri("http://www.chinawj.com.cn/")))
                        {
                            cd.Add(ck.Name, ck.Value);
                        }
                        Cookies = cd;
                        while (true)
                        {
                            int data_b = stream.ReadByte();
                            if (data_b == -1)
                                break;
                            list.Add((byte)data_b);
                        }
                        //return BytesToImage(list.ToArray());
                        //return System.Drawing.Image.FromFile(".code", true);
                        //image = System.Drawing.Image.FromStream(stream, true, true);

                    }
                }
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return list.ToArray();
        }
        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms, true);
            ms.Close();
            return image;
        }
        public static byte[] DictionaryToBytes(Dictionary<string, string> parameters)
        {
            byte[] data = null;
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            foreach (string key in parameters.Keys)
            {
                if (i > 0)
                {
                    buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", key, parameters[key]);
                }
                i++;
            }
            return Encoding.UTF8.GetBytes(buffer.ToString());
        }
        /// <summary>
        /// Convert Byte[] to string
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] buffer)
        {
            string result = Encoding.UTF8.GetString(buffer);
            return result;
        }



        private void Delete_Flush_Thread()
        {
            while (true)
            {
                int nead_delete_num = (int)this.num_up_dowm_delete.Value;
                //GetWebBrowseCookie();
                delete_data_by_num(nead_delete_num, (String)Cookies);
                break;
            }
        }

        private void SetDeleteBarStatus()
        {
            if (this.bar_delete.InvokeRequired)
            {
                FlushClient fc = new FlushClient(SetDeleteBarStatus);
                this.Invoke(fc);
            }
            else
            {
                this.bar_delete.Value = bar_num;
            }
        }

        /// <summary>
        /// 删除x条帖子
        /// </summary>
        /// <param name="nead_delete_num"></param>
        /// <returns></returns>
        private void delete_data_by_num(int nead_delete_num, string cookie)
        {
            int has_delete_num = 0;
            for (int i = 0; i < nead_delete_num; i++)
            {
                int delete_result = delete_data_by_page(nead_delete_num - has_delete_num, cookie);
                if (delete_result == -1)
                {
                    bar_num = 100;
                    SetDeleteBarStatus();
                    break;
                }
                has_delete_num += delete_result;
                if (has_delete_num >= nead_delete_num)
                {
                    bar_num = 100;
                    SetDeleteBarStatus();
                    break;
                }
                bar_num = has_delete_num / nead_delete_num * 100;
                SetDeleteBarStatus();
            }
        }

        /// <summary>
        /// 删除某页的帖子
        /// </summary>
        /// <param name="page_num"></param>
        private int delete_data_by_page(int num = 30, string cookie = "")
        {
            string url = delete_page_url;
            byte[] byte_result = RequestByCookie(url, cookie);
            string str_html = BytesToString(byte_result);
            //MatchCollection match = Regex.Matches(str_html, @"page=(\d+)", RegexOptions.Singleline);
            //if (match.Count > 0) max_page = Convert.ToInt32(match[match.Count - 1].Groups[1].Value);
            List<int> ids = re_page_data_count(str_html);
            if (ids.Count == 0) return -1;
            if (ids.Count > num) ids.RemoveRange(Math.Min(ids.Count, num), ids.Count);
            return delete_datas_by_ids(ids, cookie);
        }

        /// <summary>
        /// 正则 -- 匹配每页的帖子id
        /// </summary>
        /// <param name="html">html string</param>
        /// <returns>list int</returns>
        private List<int> re_page_data_count(string html)
        {
            List<int> l_int = new List<int>();
            List<int> result = new List<int>();
            MatchCollection match_nid = Regex.Matches(html, @"add.php\?id=(\d+)", RegexOptions.Singleline);
            for (int i = 0; i < match_nid.Count; i++)
            {
                l_int.Add(Convert.ToInt32(match_nid[i].Groups[1].Value));
            }
            foreach (int eachString in l_int)
            {
                if (!result.Contains(eachString))
                    result.Add(eachString);
            }
            return result;
        }
        /// <summary>
        /// 根据id列表 删除帖子id[]:22678338
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private int delete_datas_by_ids(List<int> ids, string cookie = "")
        {
            int result = 0;
            string url = delete_url;
            //http://my.chinawj.com.cn/member/logistics/?act=del&id[]=22619003&id[]=22619012
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < ids.Count; i++)
            {
                url += ("&id[]=" + ids[i]);
            }

            byte[] response = RequestByCookie(url, cookie);
            //byte[] response = RequestByCookie(delete_url, cookie, false, Encoding.UTF8.GetBytes(id_str));
            string html = BytesToString(response);
            MatchCollection match_nid = Regex.Matches(html, @"操作(\d)条成功", RegexOptions.Singleline);
            for (int i = 0; i < match_nid.Count; i++)
            {
                result += Convert.ToInt32(match_nid[i].Groups[1].Value);
            }
            return result;
        }


    }




}
