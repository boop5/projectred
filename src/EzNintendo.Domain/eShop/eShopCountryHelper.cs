using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace EzNintendo.Domain.eShop
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API")]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "It's eShop")]
    public static class eShopCountryHelper
    {
        public static string GetKeyFromRegion(eShopRegion region) => region switch
        {
            eShopRegion.Japan => "JP",
            eShopRegion.Europe => "EU",
            eShopRegion.UnitedStates => "US",
            _ => throw new ArgumentOutOfRangeException(nameof(region), region, "Unknown Country")
        };

        public static IEnumerable<eShopCountry> GetCountriesFromRegion(eShopRegion region)
        {
            return Enum.GetValues(typeof(eShopCountry))
                .OfType<eShopCountry>()
                .Where(country => GetRegionFromCountry(country) == region);
        }

        public static eShopRegion GetRegionFromCountry(eShopCountry country)
        {
            switch (country)
            {
                case eShopCountry.Australia:
                case eShopCountry.Austria:
                case eShopCountry.Belgium:
                case eShopCountry.Bulgaria:
                case eShopCountry.Croatia:
                case eShopCountry.Cyprus:
                case eShopCountry.CzechRepublic:
                case eShopCountry.Denmark:
                case eShopCountry.Estonia:
                case eShopCountry.Finland:
                case eShopCountry.France:
                case eShopCountry.Germany:
                case eShopCountry.Greece:
                case eShopCountry.Hungary:
                case eShopCountry.Ireland:
                case eShopCountry.Italy:
                case eShopCountry.Latvia:
                case eShopCountry.Lithuania:
                case eShopCountry.Luxembourg:
                case eShopCountry.Malta:
                case eShopCountry.Netherlands:
                case eShopCountry.NewZealand:
                case eShopCountry.Norway:
                case eShopCountry.Poland:
                case eShopCountry.Portugal:
                case eShopCountry.Romania:
                case eShopCountry.Russia:
                case eShopCountry.Slovakia:
                case eShopCountry.Slovenia:
                case eShopCountry.SouthAfrica:
                case eShopCountry.Spain:
                case eShopCountry.Sweden:
                case eShopCountry.Switzerland:
                case eShopCountry.UnitedKingdom:
                    return eShopRegion.Europe;
                case eShopCountry.Canada:
                case eShopCountry.UnitedStates:
                case eShopCountry.Mexico:
                    return eShopRegion.UnitedStates;
                default: throw new ArgumentOutOfRangeException(nameof(country), country, "Unknown Country.");
            }
        }

        public static string GetKeyFromCountry(eShopCountry country)
        {
            switch (country)
            {
                case eShopCountry.Australia: return "AU";
                case eShopCountry.Austria: return "AT";
                case eShopCountry.Belgium: return "BE";
                case eShopCountry.Bulgaria: return "BG";
                case eShopCountry.Canada: return "CA";
                case eShopCountry.Croatia: return "HR";
                case eShopCountry.Cyprus: return "CY";
                case eShopCountry.CzechRepublic: return "CZ";
                case eShopCountry.Denmark: return "DK";
                case eShopCountry.Estonia: return "EE";
                case eShopCountry.Finland: return "FI";
                case eShopCountry.France: return "FR";
                case eShopCountry.Germany: return "DE";
                case eShopCountry.Greece: return "GR";
                case eShopCountry.Hungary: return "HU";
                case eShopCountry.Ireland: return "IE";
                case eShopCountry.Italy: return "IT";
                case eShopCountry.Latvia: return "LV";
                case eShopCountry.Lithuania: return "LT";
                case eShopCountry.Luxembourg: return "LU";
                case eShopCountry.Malta: return "MT";
                case eShopCountry.Mexico: return "MX";
                case eShopCountry.Netherlands: return "NL";
                case eShopCountry.NewZealand: return "NZ";
                case eShopCountry.Norway: return "NO";
                case eShopCountry.Poland: return "PL";
                case eShopCountry.Portugal: return "PT";
                case eShopCountry.Romania: return "RO";
                case eShopCountry.Russia: return "RU";
                case eShopCountry.Slovakia: return "SK";
                case eShopCountry.Slovenia: return "SI";
                case eShopCountry.SouthAfrica: return "ZA";
                case eShopCountry.Spain: return "ES";
                case eShopCountry.Sweden: return "SE";
                case eShopCountry.Switzerland: return "CH";
                case eShopCountry.UnitedKingdom: return "GB";
                case eShopCountry.UnitedStates: return "US";
                default: throw new ArgumentOutOfRangeException(nameof(country), country, null);
            }
        }

        public static string GetCurrencyFromCountry(eShopCountry country)
        {
            switch (country)
            {
                case eShopCountry.Australia: return "AUD";
                case eShopCountry.Austria: return "EUR";
                case eShopCountry.Belgium: return "EUR";
                case eShopCountry.Bulgaria: return "EUR";
                case eShopCountry.Canada: return "CAD";
                case eShopCountry.Croatia: return "EUR";
                case eShopCountry.Cyprus: return "EUR";
                case eShopCountry.CzechRepublic: return "CZK";
                case eShopCountry.Denmark: return "DKK";
                case eShopCountry.Estonia: return "EUR";
                case eShopCountry.Finland: return "EUR";
                case eShopCountry.France: return "EUR";
                case eShopCountry.Germany: return "EUR";
                case eShopCountry.Greece: return "EUR";
                case eShopCountry.Hungary: return "EUR";
                case eShopCountry.Ireland: return "EUR";
                case eShopCountry.Italy: return "EUR";
                case eShopCountry.Latvia: return "EUR";
                case eShopCountry.Lithuania: return "EUR";
                case eShopCountry.Luxembourg: return "EUR";
                case eShopCountry.Malta: return "EUR";
                case eShopCountry.Mexico: return "MXN";
                case eShopCountry.Netherlands: return "EUR";
                case eShopCountry.NewZealand: return "NZD";
                case eShopCountry.Norway: return "NOK";
                case eShopCountry.Poland: return "PLN";
                case eShopCountry.Portugal: return "EUR";
                case eShopCountry.Romania: return "EUR";
                case eShopCountry.Russia: return "RUB";
                case eShopCountry.Slovakia: return "EUR";
                case eShopCountry.Slovenia: return "EUR";
                case eShopCountry.SouthAfrica: return "ZAR";
                case eShopCountry.Spain: return "EUR";
                case eShopCountry.Sweden: return "SEK";
                case eShopCountry.Switzerland: return "CHF";
                case eShopCountry.UnitedKingdom: return "GBP";
                case eShopCountry.UnitedStates: return "USD";
                default: throw new ArgumentOutOfRangeException(nameof(country), country, null);
            }
        }

        public static string GetCurrencySymbol(string currency)
        {
            switch (currency)
            {
                case "AUD":
                case "CAD":
                case "NZD":
                case "USD":
                case "MXN":
                    return "$";

                case "DKK":
                case "SEK":
                case "NOK":
                    return "kr";

                case "CHF": return "₣";
                case "CZK": return "h";
                case "EUR": return "€";
                case "GBP": return "£";
                case "PLN": return "zł";
                case "RUB": return "₽";
                case "ZAR": return "R";
                default: throw new ArgumentOutOfRangeException(nameof(currency), currency, "Unknown Currency.");
            }
        }
    }
}