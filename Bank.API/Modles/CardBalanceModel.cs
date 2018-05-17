using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bank.API.Modles
{
    class CardBalanceModel:AbstractModel
    {

        [JsonProperty("available")]
        public decimal available { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("current")]
        public decimal current { get; set; }
        [JsonProperty("credit_limit")]
        public decimal credit_limit { get; set; }
        [JsonProperty("last_statement_balance")]
        public decimal last_statement_balance { get; set; }
        [JsonProperty("last_statement_date")]
        public string last_statement_date { get; set; }
        [JsonProperty("payment_due	")]
        public decimal payment_due { get; set; }
        [JsonProperty("payment_due_date")]
        public decimal payment_due_date { get; set; }
        [JsonProperty("update_timestamp")]
        public DateTime update_timestamp { get; set; }

    }
}
