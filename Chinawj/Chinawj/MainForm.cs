using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Threading;

namespace LeShou
{
    public delegate void FlushClient();//代理
    public partial class login_form : Form
    {
        private HtmlDocument loginform = null;
        /// <summary>
        /// 验证码地址
        /// </summary>
        private static string code_url = "";
        /// <summary>
        /// 登录地址
        /// </summary>
        private static string login_url = "http://my.chinawj.com.cn/";
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
        private static string delete_url = "http://leshou.com/my/?comp=news&task=deletenews&nid=";
        /// <summary>
        /// 每页条数
        /// </summary>
        private static int page_size = 30;
        /// <summary>
        /// 验证码图片路径
        /// </summary>
        private static string code_path = ".code";
        private string cookie = "";

        public login_form()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;
        }

        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }
        /// <summary> 载入登录页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_open_Click(object sender, EventArgs e)
        {
            web_loaction(login_url);
        }
        private void login_form_Load(object sender, EventArgs e)
        {
            init();
        }

        /// <summary> 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_login_Click(object sender, EventArgs e)
        {
            write_html();
            login();
        }
        /// <summary> 浏览器导航
        /// </summary>
        public void web_loaction(string url)
        {
            this.webser.Navigate(url);
        }

        /// <summary>初始化登录页面，并且绘制验证码
        /// </summary>
        public void init()
        {
            web_loaction(login_url);
        }


        /// <summary> 填写html
        /// </summary>
        public void write_html()
        {
            string USERID = this.tbx_userid.Text;
            string PWD = this.tbx_pwd.Text;
            string CODE = this.tbx_code.Text;
            loginform.GetElementById("username").SetAttribute("value", USERID);
            loginform.GetElementById("password").SetAttribute("value", PWD);
            loginform.GetElementById("code").SetAttribute("value", CODE);
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
                MessageBox.Show("请重新输入身份证!");
                state = false;
            }
            if (PWD.Length < 6)
            {
                MessageBox.Show("请重新输入密码!");
                state = false;
            }
            if (CODE.Length != 5)
            {
                MessageBox.Show("请重新输入验证码!");
                state = false;
            }
            return state;
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void login()
        {
            HtmlElement check_remeber = loginform.GetElementsByTagName("input")[3];
            check_remeber.InvokeMember("Click");
            HtmlElement btnAdd = loginform.GetElementsByTagName("input")[4];
            btnAdd.InvokeMember("Click");
        }
        private void webser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //timer_load.Enabled = true;
        }

        private void timer_load_Tick(object sender, EventArgs e)
        {
            if (webser.ReadyState == WebBrowserReadyState.Complete && webser.IsBusy == false)
            {
                timer_load.Enabled = false;
                if (webser.StatusText == "完成" && webser.Url.AbsoluteUri == login_url)
                {
                    load_img();
                    tbx_userid.Focus();
                    tbx_userid.SelectAll();
                }
            }
        }
        /// <summary>
        /// 加载验证码
        /// </summary>
        private void load_img()
        {
            loginform = webser.Document.Forms[0].Document;
            save_image_by_list_byte();
            this.pic_code.ImageLocation = code_path;
        }

        private void webser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            timer_load.Enabled = true;
        }

        private void pic_code_Click(object sender, EventArgs e)
        {
            load_img();
        }

        /// <summary>
        /// 将字节列表 保存为图片
        /// </summary>
        public void save_image_by_list_byte()
        {
            byte[] byte_list = RequestByCookie(code_url, webser.Document.Cookie);
            File.WriteAllBytes(code_path, byte_list.ToArray());
        }
        /// <summary>
        /// request 请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="cookie">Cookies值</param>
        /// <param name="is_get">request maehod.is get? or false:post.</param>
        public static byte[] RequestByCookie(string url, string cookie, bool is_get = true)
        {
            List<byte> list = new List<byte>();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //属性配置
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            webRequest.MaximumResponseHeadersLength = -1;
            webRequest.Headers.Add("cookie", cookie);
            if (is_get == true) webRequest.Method = "GET";
            else webRequest.Method = "POST";
            webRequest.KeepAlive = true;
            try
            {
                //获取服务器返回的资源
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream stream = webResponse.GetResponseStream())
                    {

                        while (true)
                        {
                            int data = stream.ReadByte();
                            if (data == -1)
                                break;
                            list.Add((byte)data);
                        }
                        //return BytesToImage(list.ToArray());
                        //return System.Drawing.Image.FromFile(".code", true);
                        //image = System.Drawing.Image.FromStream(stream, true, true);

                    }
                }
            }
            catch (WebException ex)
            {

            }
            catch (Exception ex)
            {

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

        private void btn_delete_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(Delete_Flush_Thread);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Delete_Flush_Thread()
        {
            while (true)
            {
                int nead_delete_num = (int)this.num_up_dowm_delete.Value;
                GetWebBrowseCookie();
                delete_data_by_num(nead_delete_num, cookie);
                break;
            }
        }

        private void GetWebBrowseCookie()
        {
            if (this.webser.InvokeRequired)
            {
                FlushClient fc = new FlushClient(GetWebBrowseCookie);
                this.Invoke(fc);
            }
            else
            {
                cookie = this.webser.Document.Cookie;
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
                if (delete_result == -1) break;
                has_delete_num += delete_result;
                if (has_delete_num >= nead_delete_num) break;
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
            MatchCollection match_nid = Regex.Matches(html, @"deletenews&nid=(\d+)", RegexOptions.Singleline);
            for (int i = 0; i < match_nid.Count; i++)
            {
                l_int.Add(Convert.ToInt32(match_nid[i].Groups[1].Value));
            }
            return l_int;
        }
        /// <summary>
        /// 根据id列表 删除帖子
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private int delete_datas_by_ids(List<int> ids, string cookie = "")
        {
            int result = 0;
            for (int i = 0; i < ids.Count; i++)
            {
                string url = delete_url + ids[i];
                RequestByCookie(url, cookie);
                Thread.Sleep(20);
                result++;
            }
            return result;
        }

    }




}
