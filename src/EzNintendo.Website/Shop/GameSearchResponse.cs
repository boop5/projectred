using System.Collections.Generic;
using EzNintendo.Domain;
using EzNintendo.Domain.Nintendo;
using Newtonsoft.Json;

namespace EzNintendo.Website.Shop
{
    internal class GameSearchResponse
    {
        [JsonProperty(PropertyName = "numFound")]
        public int FoundGames { get; set; }

        [JsonProperty(PropertyName = "start")]
        public string Start { get; set; }

        [JsonProperty(PropertyName = "docs")]
        public List<GameDTO> Games { get; set; }
    }
}