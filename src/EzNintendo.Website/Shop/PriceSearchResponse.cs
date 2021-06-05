using System.Collections.Generic;
using EzNintendo.Domain;
using EzNintendo.Domain.Nintendo;
using Newtonsoft.Json;

namespace EzNintendo.Website.Shop
{
    internal class PriceSearchResponse
    {
        [JsonProperty("personalized")]
        public bool Personalized { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; } // todo: convert to Country Object

        public List<Price> Prices { get; set; }
    }
}