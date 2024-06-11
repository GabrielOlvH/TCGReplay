public class CardData
{
    public string Id;
    public string Name;
    public string Supertype;
    public string[] Subtypes;
    public int Hp;
    public Type[] Types;
    public string EvolvesFrom;
    public string[] Abilities;
    public Attack[] Attacks;
    public Type[] RetreatCost;
    public string Image;
    public string[] Rules;

    public CardData(string id, string name, string supertype, string[] subtypes, int hp, Type[] types, string evolvesFrom, string[] abilities, Attack[] attacks, Type[] retreatCost, string image, string[] rules)
    {
        Id = id;
        Name = name;
        Supertype = supertype;
        Subtypes = subtypes;
        Hp = hp;
        Types = types;
        EvolvesFrom = evolvesFrom;
        Abilities = abilities;
        Attacks = attacks;
        RetreatCost = retreatCost;
        Image = image;
        Rules = rules;
    }
}