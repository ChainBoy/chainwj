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


namespace ChinaWJ
{
    public delegate void FlushClient();//代理
    public partial class login_form : Form
    {

        /// <summary>
        /// 登录页地址
        /// </summary>
        private static string LOGIN_PAGE_URL = "http://my.chinawj.com.cn/";
        /// <summary>
        /// 登录地址
        /// </summary>
        private static string LOGIN_URL = "http://my.chinawj.com.cn/member/logins.php?act=logins";
        /// <summary>
        /// 删除页地址
        /// </summary>
        private static string DELETE_PAGE_URL = "http://my.chinawj.com.cn/member/logistics/?dc=1";

        /// <summary>
        /// 删除地址
        /// </summary>
        private static string DELETE_URL = "http://my.chinawj.com.cn/member/logistics/?act=del";

        /// <summary>
        /// 发布地址
        /// </summary>
        private static string PUT_URL = "http://my.chinawj.com.cn/member/product/add.php";
        private static string PUT_PAGE_URL = "http://my.chinawj.com.cn/member/product/add.php";

        private static string BOUNDARY = "----WebKitFormBoundaryYeixS6TNKlpzDBJA";
        Thread thread_delete = null;
        Thread thread_put = null;

        /// <summary>
        /// 验证码图片路径
        /// </summary>
        private static string COOKIE_PATH = ".c";
        private static string USER_PATH = ".u";
        private static string DATA_PATH = ".d";

        /// <summary>
        /// 是否登录
        /// </summary>
        private static bool IS_LOGIN = false;

        private static string STATUS_DO = "";

        /// <summary> 发布休眠时间
        /// 
        /// </summary>
        private int PUT_SLEEP_TIME = 1000 * 30;

        /// <summary>
        /// 用户帐号
        /// </summary>
        private static Dictionary<String, String> USER = new Dictionary<string, string>() { };
        List<list_item> TYPES = new List<list_item>();
        List<list_item> UTYPES = new List<list_item>();
        private static Citys CITY = new Citys();
        private static Comm COMM = new Comm();
        private static List<List<string>> CITYS = CITY.citys();
        Dictionary<string, string> WULIU_INFO = new Dictionary<string, string>();

        public login_form()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>删除的数量
        /// </summary>
        private int DELETE_BAR_NUM = 0;
        /// <summary> 发布的数量
        /// 
        /// </summary>
        private int PUT_BAR_NUM = 0;
        private CookieContainer COOKIE = new CookieContainer();


        private void login_form_Load(object sender, EventArgs e)
        {
            form_Init();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            login();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            delete();
        }

        /// <summary> 窗体初始化，检查Cookie，调整页面
        /// </summary>
        private void form_Init()
        {
            band();
            load_user();
            bool has_cookie = load_cookie();
            bool cookie_can_use = false;
            if (has_cookie)
            {
                cookie_can_use = check_cookie();
            }
            if (cookie_can_use && has_cookie)
            {
                IS_LOGIN = true;
                this.status.Text = "已登录";
                //todo:操作页
            }
            else
            {
                IS_LOGIN = false;
                this.status.Text = "Cookie已失效，请重新登录！";
                //todu:登录
            }
            show_main();
        }
        private void form_Exit()
        {
            dump_cookie();
        }

        /// <summary>
        /// 序列化，保存
        /// </summary>
        /// <param name="path">路径（相对)</param>
        /// <param name="param"></param>
        private void dump_pick(string path, object param)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, param);
            fileStream.Close();
        }

        /// <summary> 反序列化，加载
        /// </summary>
        /// <param name="type">类型，type=1: cookie. type=2: user</param>
        /// <returns></returns>
        private bool load_pick(string path, int type)
        {
            bool result = false;
            if (File.Exists(path))
            {
                FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    if (type == 1)
                    {
                        COOKIE = bf.Deserialize(fileStream) as CookieContainer;
                    }
                    else if (type == 2)
                    {
                        USER = bf.Deserialize(fileStream) as Dictionary<string, string>;
                    }
                    else if (type == 3)
                    {
                        WULIU_INFO = bf.Deserialize(fileStream) as Dictionary<string, string>;
                    }
                    fileStream.Close();
                    result = true;
                }
                catch (System.Runtime.Serialization.SerializationException)
                {

                }
            }
            return result;
        }

        /// <summary> 保存用户信息
        /// </summary>
        private void dump_user()
        {
            string user_name = this.tbx_userid.Text.Trim();
            string user_pwd = this.tbx_pwd.Text.Trim();
            USER["user"] = user_name;
            USER["pwd"] = user_pwd;
            dump_pick(USER_PATH, USER);
        }

        /// <summary> 加载用户信息
        /// </summary>
        private void load_user()
        {
            bool result = load_pick(USER_PATH, 2);
            if (result)
            {
                this.tbx_userid.Text = USER["user"];
                this.tbx_pwd.Text = USER["pwd"];
            }
        }

        /// <summary> 保存发布内容
        /// </summary>
        private void dump_data()
        {
            string user_name = this.tbx_userid.Text.Trim();
            string user_pwd = this.tbx_pwd.Text.Trim();
            if (check_put_data())
            {
                init_data();
                dump_pick(DATA_PATH, WULIU_INFO);
                STATUS_DO = "保存成功";
                set_status_bar_status();
            }
        }

        /// <summary> 加载发布内容
        /// </summary>
        private void load_data()
        {
            bool result = load_pick(DATA_PATH, 3);
            if (result)
            {
                show_setting_data();
            }
        }

        /// <summary> 保存cookie
        /// </summary>
        private void dump_cookie()
        {
            dump_pick(COOKIE_PATH, COOKIE);
        }

        /// <summary> 加载cookie
        /// </summary>
        private bool load_cookie()
        {
            return load_pick(COOKIE_PATH, 1);
        }

        /// <summary> 删除
        /// </summary>
        public void delete()
        {
            STATUS_DO = "删除任务开始";
            this.DELETE_BAR_NUM = 0;
            set_delete_bar_status();
            set_status_bar_status();
            thread_delete = new Thread(flush_delete_thread);
            thread_delete.IsBackground = true;
            thread_delete.Start();
        }
        /// <summary> 发布
        /// </summary>
        public void put()
        {
            if (check_put_data())
            {
                init_data();
                check_type();
                STATUS_DO = "发布任务开始";
                this.PUT_BAR_NUM = 0;
                set_put_bar_status();
                set_status_bar_status();

                thread_put = new Thread(flush_put_thread);
                thread_put.IsBackground = true;
                thread_put.Start();
            }
            else
            {
                show_setting();
            }
        }

        /// <summary> 登录
        /// </summary>
        public void login()
        {
            if (!check_login()) return;
            string USERID = this.tbx_userid.Text;
            string PWD = this.tbx_pwd.Text;
            byte[] data = Encoding.UTF8.GetBytes("UsernameGet=" + USERID + "&pwd=" + PWD + "&submit=");
            byte[] response = RequestByCookie(LOGIN_URL, false, data);
            string html = bytes_to_string(response);
            if (html == "<script>top.location=\"http://my.chinawj.com.cn/member/index.php\"</script>")
            {
                dump_cookie();
                dump_user();
                IS_LOGIN = true;
                this.status.Text = "登录成功！";
            }
            else
            {
                this.status.Text = "登录失败！";
                IS_LOGIN = false;
            }
            show_main();
        }

        /// <summary> 跳转登录或者操作
        /// 
        /// </summary>
        public void show_main()
        {
            if (IS_LOGIN)
            {
                this.panel_login.Visible = false;
                this.tab_main.Visible = true;
                this.Size = new Size(480, 360);
                load_data();
            }
            else
            {
                this.panel_login.Visible = true;
                this.tab_main.Visible = false;
                this.Size = new Size(332, 287);
            }
            this.panel_sttting.Visible = false;
        }

        /// <summary> 跳转设置界面
        /// </summary>
        private void show_setting()
        {
            this.tab_main.Visible = false;
            this.panel_login.Visible = false;
            this.panel_sttting.Visible = true;
            this.panel_sttting.Location = new Point(15, 27);
            this.Size = new Size(530, 566);
            load_data();
        }

        /// <summary> 现实设置页内容
        /// 
        /// </summary>
        private void show_setting_data()
        {
            this.tbx_title.Text = WULIU_INFO["title"];
            this.tbx_body_simple.Text = WULIU_INFO["body_simple"];
            this.rtbx_content.Text = WULIU_INFO["content"];
            string keywords = WULIU_INFO["keywords_main[]"];
            string[] keys = keywords.Split(new string[] { "&keywords_main[]=" }, StringSplitOptions.None);
            if (keys.Length == 3)
            {
                this.tbx_keywords_main1.Text = keys[0];
                this.tbx_keywords_main2.Text = keys[1];
                this.tbx_keywords_main3.Text = keys[2];
            }
        }

        /// <summary>检查帐号 密码 验证码</summary>
        /// <returns>True/False</returns>
        public bool check_login()
        {
            bool state = true;
            string user = this.tbx_userid.Text;
            string pwd = this.tbx_pwd.Text;
            string code = this.tbx_code.Text;
            if (user.Length < 5)
            {
                MessageBox.Show("请重新输入帐号!");
                this.status.Text = "帐号长度不能小于5";
                state = false;
            }
            if (pwd.Length < 5)
            {
                MessageBox.Show("请重新输入密码!");
                this.status.Text = "密码长度不能小于5";
                state = false;
            }
            //if (code.Length != 5)
            //{
            //    MessageBox.Show("请重新输入验证码!");
            //    state = false;
            //}
            return state;
        }

        /// <summary> 检查发布内容是否符合网站要求
        /// </summary>
        /// <returns></returns>
        private bool check_put_data()
        {
            bool state = true;
            string title = this.tbx_title.Text.Trim();
            if (title.Length < 10)
            {
                state = false;
                MessageBox.Show("标题不能小于10个字");
                STATUS_DO = "标题不能小于10个字";
                set_status_bar_status();
                return state;
            }
            string sample = this.tbx_body_simple.Text.Trim();
            if (sample.Length < 22)
            {
                state = false;
                MessageBox.Show("描述不能小于20个字");
                STATUS_DO = "描述不能小于20个字";
                set_status_bar_status();
                return state;
            }
            string keywork = this.tbx_keywords_main1.Text.Trim() + this.tbx_keywords_main2.Text.Trim() + this.tbx_keywords_main3.Text.Trim();
            if (keywork.Length < 6)
            {
                state = false;
                MessageBox.Show("关键词不能少于6个字");
                STATUS_DO = "关键词不能少于6个字";
                set_status_bar_status();
                return state;
            }
            string content = this.rtbx_content.Text.Trim();
            if (content.Length < 30)
            {
                state = false;
                MessageBox.Show("文章内容不能少于30个字");
                STATUS_DO = "文章内容不能少于30个字";
                set_status_bar_status();
                return state;
            }
            return state;
        }

        /// <summary> 将字节列表 保存为图片
        /// </summary>
        public void save_image_by_list_byte(byte[] byte_list, string path)
        {
            File.WriteAllBytes(path, byte_list.ToArray());
        }

        /// <summary> 模拟请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="cookie">Cookies值</param>
        /// <param name="is_get">request maehod.is get? or false:post.</param>
        public byte[] RequestByCookie(string url, bool is_get = true, byte[] data = null, string content_type = "application/x-www-form-urlencoded")
        {
            List<byte> list = new List<byte>();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //属性配置
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            webRequest.MaximumResponseHeadersLength = -1;
            webRequest.CookieContainer = COOKIE;
            if (is_get == true) webRequest.Method = "GET";
            else webRequest.Method = "POST";
            webRequest.KeepAlive = true;
            if (is_get == false && data != null)
            {
                //webRequest.
                webRequest.ContentType = content_type;
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
                        while (true)
                        {
                            int data_b = stream.ReadByte();
                            if (data_b == -1)
                                break;
                            list.Add((byte)data_b);
                        }
                    }
                }
            }
            catch (WebException)
            {
            }
            catch (Exception)
            {
            }
            return list.ToArray();
        }

        /// <summary> Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image bytes_to_image(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms, true);
            ms.Close();
            return image;
        }

        /// <summary> Convert Byte[] to string
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string bytes_to_string(byte[] buffer)
        {
            string result = Encoding.UTF8.GetString(buffer);
            return result;
        }

        /// <summary> 刷新删除线程
        /// </summary>
        private void flush_delete_thread()
        {
            while (true)
            {
                int num = (int)this.num_up_down_delete.Value;
                delete_data_by_num(num);
                break;
            }
            STATUS_DO = "删除任务结束";
            this.DELETE_BAR_NUM = 100;
            set_delete_bar_status();
            set_status_bar_status();
        }

        /// <summary> 刷新发布线程
        /// </summary>
        private void flush_put_thread()
        {
            while (true)
            {
                int num = (int)this.num_up_down_put.Value;
                put_data_by_num(num);
                break;
            }
            STATUS_DO = "发布任务结束";
            this.PUT_BAR_NUM = 100;
            set_put_bar_status();
            set_status_bar_status();
        }

        /// <summary> 发布x条帖子
        /// </summary>
        /// <param name="nead_put_num"></param>
        private void put_data_by_num(int nead_put_num)
        {
            int has_put_num = 0;
            for (int i = 0; i < nead_put_num; i++)
            {
                int put_result = put_data();
                if (put_result == -1)
                {
                    PUT_BAR_NUM = 100;
                    set_put_bar_status();
                    break;
                }
                has_put_num += put_result;
                if (has_put_num >= nead_put_num)
                {
                    PUT_BAR_NUM = 100;
                    set_put_bar_status();

                    break;
                }
                PUT_BAR_NUM = has_put_num * 100 / nead_put_num;

                STATUS_DO = "已发布：" + has_put_num + "/" + nead_put_num;
                set_status_bar_status();
                set_put_bar_status();
                Thread.Sleep(PUT_SLEEP_TIME);
            }
        }

        /// <summary> 随即获取省份、城市
        /// </summary>
        /// <returns></returns>
        private List<string> random_city()
        {
            int count = CITYS.Count();
            Random random = new Random();
            int index = random.Next(count);
            return CITYS[index];
        }

        /// <summary> 随机获取分类
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private list_item random_type(List<list_item> list)
        {
            int count = list.Count();
            Random random = new Random();
            int index = random.Next(count);
            return list[index];
        }

        /// <summary> 初始化发布内容
        /// </summary>
        private void init_data()
        {
            WULIU_INFO["mypretime"] = COMM.pretime(); //62187
            WULIU_INFO["id"] = "0"; //0
            WULIU_INFO["title"] = this.tbx_title.Text.Trim(); //再次测试
            WULIU_INFO["typeid"] = random_type(TYPES).Id; //559
            WULIU_INFO["utype"] = random_type(UTYPES).Id; //1
            WULIU_INFO["body_simple"] = this.tbx_body_simple.Text.Trim(); //又是嘛。禁重复，包括每条重复，
            WULIU_INFO["keywords_main[]"] = this.tbx_keywords_main1.Text.Trim() + "&keywords_main[]=" +
                this.tbx_keywords_main2.Text.Trim() + "&keywords_main[]=" + this.tbx_keywords_main3.Text.Trim();

            //关键词1
            WULIU_INFO["pic1"] = ""; //
            WULIU_INFO["file_pic1"] = ""; //; filename="" Content-Type: application/octet-stream
            WULIU_INFO["content"] = this.rtbx_content.Text.Trim(); //实测以下内容
            WULIU_INFO["period"] = "90"; //90
            List<string> from = random_city();
            List<string> to = random_city();
            WULIU_INFO["from_province"] = from[2]; //3
            WULIU_INFO["from_city"] = from[0]; //118
            WULIU_INFO["to_province"] = to[2]; //1
            WULIU_INFO["to_city"] = to[0]; //41
            WULIU_INFO["price_unit"] = ""; //
            WULIU_INFO["price"] = ""; //
            WULIU_INFO["isread"] = "1"; //1
            WULIU_INFO["submit"] = "同意服务条款，立即发布"; //同意服务条款，立即发布 
        }

        /// <summary> 检查帖子所属分类
        /// </summary>
        private void check_type()
        {
            string url = "http://my.chinawj.com.cn/member/product/add.php";
            byte[] result_b = RequestByCookie(url);
            string result = bytes_to_string(result_b);
            MatchCollection match_nid = Regex.Matches(result, "<script>alert(.+?);top.location=\"(.+?)\"</script>", RegexOptions.Singleline);
            if (match_nid.Count > 0)
            {
                PUT_PAGE_URL = "http://my.chinawj.com.cn" + match_nid[0].Groups[2].Value;
                PUT_URL = "http://my.chinawj.com.cn" + match_nid[0].Groups[2].Value + "?act=save01";
                STATUS_DO = match_nid[0].Groups[1].Value;
            }
        }

        /// <summary> 发布帖子
        /// </summary>
        /// <returns></returns>
        private int put_data()
        {
            Dictionary<string, string> data_tmp = new Dictionary<string, string>();
            foreach (string key in WULIU_INFO.Keys)
            {
                data_tmp[key] = WULIU_INFO[key];
            }
            data_tmp["mypretime"] = COMM.pretime(); //62187
            data_tmp["body_simple"] = data_tmp["body_simple"] + "\n1" + DateTime.Now.Ticks.ToString();
            data_tmp["content"] = data_tmp["content"] + "\n\n2" + DateTime.Now.Ticks.ToString();
            data_tmp["title"] = data_tmp["title"] + "\n\n3" + DateTime.Now.Ticks.ToString();
            string data_str = COMM.diction_to_boundary(data_tmp, BOUNDARY);

            data_str += "--" + BOUNDARY + "\nContent-Disposition: form-data; name=\"file_pic1\"; filename=\"\"\nContent-Type: application/octet-stream\n\n";
            byte[] data_b = Encoding.UTF8.GetBytes(data_str);
            RequestByCookie(LOGIN_PAGE_URL, false, data_b);
            byte[] result_b = RequestByCookie(PUT_URL, false, data_b, "multipart/form-data; boundary=" + BOUNDARY);
            string result = bytes_to_string(result_b);
            MatchCollection match_nid = Regex.Matches(result, "<script>alert(.+?);top.location=\"(.+?)\"</script>", RegexOptions.Singleline);
            if (match_nid.Count > 0)
            {
                PUT_URL = "http://my.chinawj.com.cn" + match_nid[0].Groups[2].Value + "?act=save01";
                STATUS_DO = match_nid[0].Groups[1].Value;
                return 0;
            }
            match_nid = Regex.Matches(result, "操作成功", RegexOptions.Singleline);
            if (match_nid.Count > 0)
            {
                return 1;
            }
            if (Regex.Matches(result, "请填写标题！标题不能少于5个字|不支持的文件格式，只支持，jpg，gif格式的图片|http://my.chinawj.com.cn/member/index.php|优势描述不能少于20字", RegexOptions.Singleline).Count > 0)
            {
                return -1;
            }
            return 0;

        }

        /// <summary> 设置状态栏目文字
        /// </summary>
        private void set_status_bar_status()
        {
            if (this.bar_delete.InvokeRequired)
            {
                FlushClient fc = new FlushClient(set_status_bar_status);
                this.Invoke(fc);
            }
            else
            {
                this.status_do.Text = STATUS_DO;
            }
        }

        /// <summary> 设置删除进度条状态
        /// </summary>
        private void set_delete_bar_status()
        {
            if (this.bar_delete.InvokeRequired)
            {
                FlushClient fc = new FlushClient(set_delete_bar_status);
                this.Invoke(fc);
            }
            else
            {
                this.bar_delete.Value = DELETE_BAR_NUM;
            }
        }

        /// <summary> 设置发布进度条状态
        /// </summary>
        private void set_put_bar_status()
        {
            if (this.bar_put.InvokeRequired)
            {
                FlushClient fc = new FlushClient(set_put_bar_status);
                this.Invoke(fc);
            }
            else
            {
                this.bar_put.Value = PUT_BAR_NUM;
            }
        }

        /// <summary> 删除x条帖子
        /// </summary>
        /// <param name="nead_delete_num"></param>
        /// <returns></returns>
        private void delete_data_by_num(int nead_delete_num)
        {
            int has_delete_num = 0;
            for (int i = 0; i < nead_delete_num; i++)
            {
                //int delete_result = 10;
                int delete_result = delete_data_by_page(nead_delete_num - has_delete_num);
                if (delete_result == -1)
                {
                    DELETE_BAR_NUM = 100;
                    set_delete_bar_status();
                    break;
                }
                has_delete_num += delete_result;
                if (has_delete_num >= nead_delete_num)
                {
                    DELETE_BAR_NUM = 100;
                    set_delete_bar_status();
                    break;
                }
                DELETE_BAR_NUM = has_delete_num / nead_delete_num * 100;
                STATUS_DO = "已删除：" + has_delete_num + "/" + nead_delete_num;
                set_status_bar_status();
                set_delete_bar_status();
            }
        }

        /// <summary> 删除某页的帖子
        /// </summary>
        /// <param name="page_num"></param>
        private int delete_data_by_page(int num = 30)
        {
            string url = DELETE_PAGE_URL;
            byte[] byte_result = RequestByCookie(url);
            string str_html = bytes_to_string(byte_result);
            //MatchCollection match = Regex.Matches(str_html, @"page=(\d+)", RegexOptions.Singleline);
            //if (match.Count > 0) max_page = Convert.ToInt32(match[match.Count - 1].Groups[1].Value);
            List<int> ids = re_page_data_count(str_html);
            if (ids.Count == 0) return -1;
            if (ids.Count > num) ids.RemoveRange(Math.Min(ids.Count, num), ids.Count - num);
            return delete_datas_by_ids(ids);
        }

        /// <summary> 正则 -- 获取每页的帖子id
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

        /// <summary> 根据id列表 删除帖子id[]:22678338
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private int delete_datas_by_ids(List<int> ids)
        {
            int result = 0;
            string url = DELETE_URL;
            //http://my.chinawj.com.cn/member/logistics/?act=del&id[]=22619003&id[]=22619012
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < ids.Count; i++)
            {
                url += ("&id[]=" + ids[i]);
            }

            byte[] response = RequestByCookie(url);
            //byte[] response = RequestByCookie(delete_url, cookie, false, Encoding.UTF8.GetBytes(id_str));
            string html = bytes_to_string(response);
            MatchCollection match_nid = Regex.Matches(html, @"操作(\d)条成功", RegexOptions.Singleline);
            for (int i = 0; i < match_nid.Count; i++)
            {
                result += Convert.ToInt32(match_nid[i].Groups[1].Value);
            }
            return result;
        }

        /// <summary> 检查cookie 是否可用
        /// </summary>
        /// <returns></returns>
        private bool check_cookie()
        {
            bool is_login = false;
            byte[] html = RequestByCookie(LOGIN_PAGE_URL);
            string result = bytes_to_string(html);
            if (result == "<script>window.location='http://my.chinawj.com.cn/member/index.php';</script>")
            {
                is_login = true;
            }
            return is_login;
        }

        /// <summary> 退出
        /// 
        /// </summary>
        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
        }

        /// <summary> 发布按钮
        /// 
        /// </summary>
        private void btn_put_Click(object sender, EventArgs e)
        {
            put();
        }

        /// <summary>菜单栏 - 发布
        /// 
        /// </summary>
        private void tool_put_Click(object sender, EventArgs e)
        {
            this.tab_main.SelectedIndex = 0;
            show_main();
        }

        /// <summary>菜单栏 - 删除
        /// 
        /// </summary>
        private void tool_delete_Click(object sender, EventArgs e)
        {
            this.tab_main.SelectedIndex = 1;
            show_main();
        }

        /// <summary>菜单栏 - 设置发布内容
        /// 
        /// </summary>
        private void tool_setting_Click(object sender, EventArgs e)
        {
            show_setting();
        }


        /// <summary> 菜单栏 - 登录
        /// 
        /// </summary> 
        private void tool_login_Click(object sender, EventArgs e)
        {
            show_main();
        }

        /// <summary> 绑定主分类
        /// </summary>
        /// <returns></returns>
        private List<list_item> band_type()
        {
            List<list_item> data_source = new List<list_item>();
            data_source.Add(new list_item() { Id = "559", Name = "汽车运输" });
            data_source.Add(new list_item() { Id = "560", Name = "火车运输" });
            data_source.Add(new list_item() { Id = "561", Name = "航空运输" });
            data_source.Add(new list_item() { Id = "562", Name = "海运航运" });
            data_source.Add(new list_item() { Id = "569", Name = "托运/搬家" });
            data_source.Add(new list_item() { Id = "570", Name = "回程车服务" });
            this.list_type.DataSource = data_source;
            return data_source;
        }

        /// <summary> 绑定细分类
        /// 
        /// </summary>
        /// <returns></returns>
        private List<list_item> band_utype()
        {
            List<list_item> data_source = new List<list_item>();
            data_source.Add(new list_item() { Id = "1", Name = "集装箱汽车运输" });
            data_source.Add(new list_item() { Id = "2", Name = "危险品运输" });
            data_source.Add(new list_item() { Id = "3", Name = "海关监管运输" });
            data_source.Add(new list_item() { Id = "4", Name = "任何形式" });
            data_source.Add(new list_item() { Id = "5", Name = "罐车运输" });
            data_source.Add(new list_item() { Id = "6", Name = "普货运输" });
            data_source.Add(new list_item() { Id = "7", Name = "出租货运" });
            data_source.Add(new list_item() { Id = "8", Name = "大件运输" });
            data_source.Add(new list_item() { Id = "9", Name = "其他公路运输" });
            this.list_utype.DataSource = data_source;
            return data_source;
        }

        /// <summary> 绑定省份
        /// 
        /// </summary>
        private void band_province()
        {

            this.list_city_from.DataSource = CITY.provinces();
            this.list_city_to.DataSource = CITY.provinces();
        }

        /// <summary> 获取城市
        /// 
        /// </summary>
        /// <param name="city"></param>
        private void get_province(int city)
        {

        }

        /// <summary> 绑定主次分类、省份
        /// 
        /// </summary>
        private void band()
        {
            TYPES = band_type();
            UTYPES = band_utype();
            band_province();
        }

        private void btn_setting_save_Click(object sender, EventArgs e)
        {
            dump_data();
        }

        private void btn_setting_cancel_Click(object sender, EventArgs e)
        {
            show_main();
        }

        private void btn_delete_end_Click(object sender, EventArgs e)
        {
            thread_delete.Abort();

            STATUS_DO = "删除已终止";
            this.DELETE_BAR_NUM = 100;
            set_delete_bar_status();
            set_status_bar_status();
        }

        private void btn_put_end_Click(object sender, EventArgs e)
        {
            thread_put.Abort();
            STATUS_DO = "发布已终止";
            this.PUT_BAR_NUM = 100;
            set_put_bar_status();
            set_status_bar_status();
        }

    }
    public class list_item
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

}
