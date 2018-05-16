using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bank.API.Modles
{
    class AbstractModel
    {     
        [JsonProperty("url")]
        public string Url { get; set; }

        public int GetID()
        {
            var uri = new Uri(Url);
            var last = uri.Segments.Last();

            return int.Parse(last);
        }
    }

}

