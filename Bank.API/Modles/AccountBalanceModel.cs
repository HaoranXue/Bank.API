using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bank.API.Modles
{

    class AccountBalanceModel : AbstractModel
    {
        [JsonProperty("available")]
        public decimal available { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("current")]
        public decimal current { get; set; }
        [JsonProperty("update_timestamp")]
        public DateTime update_timestamp { get; set; }

    }

}
