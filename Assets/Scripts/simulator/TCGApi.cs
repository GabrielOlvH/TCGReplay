using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class TCGApi
{
    private const string SearchURL = "https://api.pokemontcg.io/v2/cards?q=name:\"{0}\"+legalities.standard:legal";
    public static readonly Dictionary<string, List<CardData>> Cache = new();
    public static readonly List<string> Awaiting = new();

    public static IEnumerable<CardData> FindCardByName(string term)
    {
        if (Cache.TryGetValue(term, out var matches))
        {
            return matches;
        }

        if (Awaiting.Contains(term)) return new List<CardData>();
        Awaiting.Add(term);
        matches = new List<CardData>();
        
        if (PlayerPrefs.GetString(term).Length != 0)
        {
            asdas(term, matches, PlayerPrefs.GetString(term));
            return matches;
        }

        using var wc = new WebClient();
        wc.Headers.Add("X-Api-Key", ConfigReader.ReadConfig().token);
        var url = string.Format(SearchURL, term);
        wc.DownloadStringTaskAsync(new Uri(url));
        wc.DownloadStringCompleted += (sender, e) =>
        {
            if (e.Error != null)
            {
                Debug.Log("Failed to fetch " + term);
            }
            var str = e.Result;
            asdas(term, matches, str);
            PlayerPrefs.SetString(term, str);
            PlayerPrefs.Save();
        };
        return matches;

    }

    private static void asdas(string term, List<CardData> matches, string str)
    {
        var json = JObject.Parse(str);
            var selectToken = json.SelectToken("data");
            foreach (var jToken in selectToken)
            {
                if (jToken.SelectToken("legalities").SelectToken("standard") == null) continue;
                var id = jToken["id"].Value<string>();
                var name = jToken["name"].Value<string>();
                var supertype = jToken["supertype"].Value<string>();
                var subtypes = jToken["subtypes"] != null ? jToken["subtypes"].Select((s) => s.Value<string>()).ToArray() : new string[]{};
       
                var hp = jToken["hp"]?.Value<int>() ?? 0;
                var types = jToken["types"] != null
                    ? jToken["types"].Select(s =>
                    {
                        Enum.TryParse(typeof(Type), s.Value<string>(), true, out var type);
                        return (Type)type;
                    }).ToArray()
                    : new Type[] { };
              
                var evolvesFrom = jToken["evolvesFrom"]?.Value<string>();
                var abilities = jToken["abilities"] != null
                    ? jToken["abilities"].Select((s) => s["name"].Value<string>()).ToArray()
                    : new string[] { };
               
                var attacks = jToken["attacks"] != null
                    ? jToken["attacks"].Select((s) =>
                    {
                        var attackName = s["name"].Value<string>();
                        var attackCost = s["cost"].Select(s =>
                        {
                            Enum.TryParse(typeof(Type), s.Value<string>(), true, out var type);
                            return (Type)type;
                        }).ToArray();
                        return new Attack(attackName, attackCost);
                    }).ToArray()
                    : new Attack[] { };
                var retreatCost = jToken["retreatCost"] != null
                    ? jToken["retreatCost"].Select(s =>
                    {
                        Enum.TryParse(typeof(Type), s.Value<string>(), true, out var type);
                        return (Type)type;
                    }).ToArray()
                    : new Type[] { };
               
                var image = jToken["images"]["large"].Value<string>();
                
                var rules =  jToken["rules"] != null
                    ? jToken["rules"].Select(s => s.Value<string>()).ToArray()
                    : new string[] { };
                var data = new CardData(id, name, supertype, subtypes, hp, types, evolvesFrom, abilities, attacks,
                    retreatCost, image, rules);
                matches.Add(data);
                
            }
            Cache.Add(term, matches);
    }
}
