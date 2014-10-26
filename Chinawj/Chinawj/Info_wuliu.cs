using System;

namespace ChinaWJ
{
    public class Info_wuliu
    {

        private string mypretime;// 62187
        private string id;// 0
        private string title;// 再次测试
        private string typeid;// 559
        private string utype;// 1
        private string body_simple;// 又是嘛。禁重复，包括每条重复，
        private string keywords_main;// 关键词1
        private string keywords_more;// 
        private string pic1;// 
        private string file_pic1;// ; filename="" Content-Type: application/octet-stream
        private string content;// 实测以下内容
        private string period;// 90
        private string from_province;// 3
        private string from_city;// 118
        private string to_province;// 1
        private string to_city;// 41
        private string price_unit;// 
        private string price;// 
        private string isread;// 1
        private string submit;// 同意服务条款，立即发布
        public string Mypretime
        {
            get
            {
                DateTime dt = new DateTime();
                var NowHour = dt.Hour;
                var NowMinute = dt.Minute;
                var NowSecond = dt.Second;
                var mypretime = (NowHour * 3600) + (NowMinute * 60) + NowSecond;
                return mypretime.ToString();
            }
            set { mypretime = value; }
        }


        public string Id
        {
            get { return id; }
            set { id = value; }
        }


        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        public string Typeid
        {
            get { return typeid; }
            set { typeid = value; }
        }


        public string Utype
        {
            get { return utype; }
            set { utype = value; }
        }


        public string Body_simple
        {
            get { return body_simple; }
            set { body_simple = value; }
        }


        public string Keywords_main
        {
            get { return keywords_main; }
            set { keywords_main = value; }
        }


        public string Keywords_more
        {
            get { return keywords_more; }
            set { keywords_more = value; }
        }


        public string Pic1
        {
            get { return pic1; }
            set { pic1 = value; }
        }


        public string Content
        {
            get { return content; }
            set { content = value; }
        }


        public string Period
        {
            get { return period; }
            set { period = value; }
        }


        public string From_province
        {
            get { return from_province; }
            set { from_province = value; }
        }


        public string From_city
        {
            get { return from_city; }
            set { from_city = value; }
        }


        public string To_province
        {
            get { return to_province; }
            set { to_province = value; }
        }


        public string To_city
        {
            get { return to_city; }
            set { to_city = value; }
        }


        public string Price_unit
        {
            get { return price_unit; }
            set { price_unit = value; }
        }


        public string Price
        {
            get { return price; }
            set { price = value; }
        }


        public string Isread
        {
            get { return isread; }
            set { isread = value; }
        }


        public string Submit
        {
            get { return submit; }
            set { submit = value; }
        }
    }
}
