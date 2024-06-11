using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardMatcher
{
    public string DisplayName;
    public List<CardData> Matches = new();
    public List<string> AttacksUsed = new();
    public List<string> AbilitiesUsed = new();
    

    public CardMatcher(string displayName)
    {
        DisplayName = displayName.Trim();
    }

    public bool IsPokemon()
    {
        Debug.Log(Matches.Count);
        return Matches.All(data => data.Supertype == "Pokémon");
    }

    public string SelectDisplay()
    {
        if (Matches.Count == 0)
        {
            Matches.AddRange(TCGApi.FindCardByName(DisplayName));
        }
        
        var results = Matches.Where(data =>
        {
            var valid = AttacksUsed.Count == 0 || AttacksUsed.All(attackName => data.Attacks.Any(attack => attack.Name == attackName));
            if (AbilitiesUsed.Count != 0 && data.Abilities.Any(ability => !AbilitiesUsed.Contains(ability)))
            {
                valid = false;
            }
            return valid;
        }).ToArray();

        if (results.Length == 0)
        {
            Debug.Log("No matches found for " + DisplayName);
            return null;
        }
        return results[0].Image;
    }

    public void AddAttack(string attack)
    {
        AttacksUsed.Add(attack);
    }

    public void AddAbility(string ability)
    {
        AbilitiesUsed.Add(ability);
    }

}