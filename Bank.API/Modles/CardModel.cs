using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bank.API.Modles
{
    class CardModel:AbstractModel
    {
        [JsonProperty("account_id")]
        public string Account_id { get; set; }
        [JsonProperty("card_network")]
        public string card_network { get; set; }
        [JsonProperty("card_type")]
        public string card_type { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("display_name")]
        public string display_name { get; set; }
        [JsonProperty("partial_card_number")]
        public string partial_card_number { get; set; }
        [JsonProperty("name_on_card")]
        public string name_on_card { get; set; }
        [JsonProperty("valid_from")]
        public string valid_from { get; set; }
        [JsonProperty("valid_to")]
        public string valid_to { get; set; }
        [JsonProperty("update_timestamp")]
        public DateTime update_timestamp { get; set; }
        [JsonProperty("provider.display_name")]
        public string providerdisplay_name { get; set; }
        [JsonProperty("provider.logo_uri")]
        public string providerlogo_uri { get; set; }
        [JsonProperty("provider.provider_id")]
        public string providerprovider_id { get; set; }
    }

}
