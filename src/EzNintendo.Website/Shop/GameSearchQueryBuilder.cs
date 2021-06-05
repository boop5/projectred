using System;
using EzNintendo.Domain;
using EzNintendo.Domain.eShop;

namespace EzNintendo.Website.Shop
{
    public class GameSearchQueryBuilder
    {
        private string GetBaseUrl(eShopRegion region, string language)
        {// todo: url based on region xd

            var usAlgoliaKey = "9a20c93440cf63cf1a7008d75f7438bf";

            switch (region)
            {
                case eShopRegion.Europe: return $"https://searching.nintendo-europe.com/{language}/select?";
                case eShopRegion.UnitedStates: // return $"https://{usAlgoliaKey}-dsn.algolia.net/1/indexes/*/queries"; // todo: gotta use headers n shit
                case eShopRegion.Japan:
                default: throw new NotSupportedException();
            }
        }

        public string QueryAll(eShopRegion region, string language = "en")
        {
            return $"{GetBaseUrl(region, language)}q=*&fq=type:GAME and ((playable_on_txt: \"HAC\")) AND sorting_title:* AND *:*&rows={int.MaxValue}";
        }

        //private string QueryByField(string key, string value, string language = "en")
        //{
        //    return $"{GetBaseUrl(language)}q=*&fq=type:GAME and ((playable_on_txt: \"HAC\")) AND {key}:{value}";
        //}
    }
}