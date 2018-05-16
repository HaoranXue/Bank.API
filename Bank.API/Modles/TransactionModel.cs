using System;
using Newtonsoft.Json;

namespace Bank.API.Modles
{
    class TransactionModel:AbstractModel
    {
        [JsonProperty("transaction_id")]
        public string transaction_id { get; set; }
        [JsonProperty("timestamp")]
        public DateTime timestamp { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("transaction_type")]
        public string transaction_type { get; set; }
        [JsonProperty("Category")]
        public Transaction_Category Reference { get; set; }
        [JsonProperty("transaction_classification")]
        public Array transaction_classification { get; set; }
        [JsonProperty("merchant_name")]
        public string merchant_name { get; set; }
        [JsonProperty("amount")]
        public decimal amount { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("meta")]
        public object meta { get; set; }
    }
}
