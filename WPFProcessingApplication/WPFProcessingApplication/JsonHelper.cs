using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WPFProcessingApplication
{
    public class JsonHelper
    {
        private static JsonHelper instance;
        public static JsonHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JsonHelper();
                }
                return instance;
            }
        }

        public List<Builder> JsonFile { get; set; }
        public int Duration { get; set; }

        public void LoadJson(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                JsonFile = JsonConvert.DeserializeObject<List<Builder>>(reader.ReadToEnd());
            }
        }

        public List<Builder> GetFromTime(int start, int stop)
        {
            int umbral = Duration - JsonFile.Max(x => x.Time);
            start -= umbral;
            stop -= umbral;
            start = (start > 0) ? start : 0;
            stop = (stop > 0) ? stop : 0;
            return JsonFile.Where(x => x.Time >= start && x.Time <= stop).ToList();
        }
    }
}
