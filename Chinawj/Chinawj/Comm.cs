using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace ChinaWJ
{
    public class Comm
    {
        /// <summary>
        /// 获取pre时间
        /// </summary>
        /// <returns></returns>
        public string pretime()
        {
            DateTime dt = DateTime.Now;
            var NowHour = dt.Hour;
            var NowMinute = dt.Minute;
            var NowSecond = dt.Second;
            var mypretime = (NowHour * 3600) + (NowMinute * 60) + NowSecond;
            return mypretime.ToString();
        }
        public string diction_to_url_string(Dictionary<string, string> dict)
        {
            string url = "";
            foreach (string key in dict.Keys)
            {
                url += "&" + key + "=" + dict[key];
            }
            return url;
        }
        public string diction_to_boundary(Dictionary<string, string> dict, string boundary)
        {
            string url = "";
            foreach (string key in dict.Keys)
            {
                url += "--" + boundary + "\nContent-Disposition: form-data; name=\"" + key + "\"\n\n" + dict[key] + "\n";
            }
            return url;
        }
        public object Clone()
        {
            BinaryFormatter Formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            Formatter.Serialize(stream, this);
            stream.Position = 0;
            object clonedObj = Formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }
    }
}
