using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardHolder
{
    public Type type;
    public List<Card> cards = new();
    protected Transform anchor;
    private CardController controller;

    public CardHolder(Type type, Transform anchor, CardController controller)
    {
        this.type = type;
        this.anchor = anchor;
        this.controller = controller;
    }

    public void Add(Card card)
    {
        cards.Add(card);
    }

    public void MoveTo(Card card, CardHolder other, bool simulation)
    {
        //Debug.Log("MOVED " + card.Name + " FROM " + type + " TO " + other.type);
        cards.Remove(card);
        other.cards.Add(card);
        if (!simulation)
        {
            if (other.type == Type.DISCARD)
            {
                card.isAlreadyRevealed = false;
            }

            card.MoveTo(other.anchor.position, other.anchor.rotation);
            Rearrange();
            other.Rearrange();
        }
    }

    public void Rearrange()
    {
        if (type == Type.PRIZE)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                var x = i / 3;
                var y = i % 3;
                var top = anchor.position;
                cards[i].MoveTo(top + new Vector3(x*2.5f, x % 2 != 0 ? 0 : 0.1f, y* 5.5f + (x % 2 != 0 ? 1f : 0)), anchor.rotation);
            }
        }
        else if (type == Type.BENCH)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                var center = anchor.position;
                cards[i].MoveTo(center+ (Vector3.left * ((cards.Count/2 - i)*4.5f)), anchor.rotation);
            }
        }
        else if (type == Type.HAND)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                var center = anchor.position;
                cards[i].MoveTo(center+ (Vector3.left * ((cards.Count/2 - i)*2.0f)), anchor.rotation);
            }
        }
    }


    public void AttachTo(Card pokemon, Card attached, CardHolder to, bool simulation)
    {
        // if (wasEvolution)
        // {
        //  var drawn = DrawCards(1)[0];
        //   hand.Remove(drawn);
        //   drawn.SetCardMatcher(new CardMatcher(to));
        //    drawn.MoveTo(fromCard.transform.position, board.benchAnchor.transform.rotation);
        //    fromCard.attachedCards.Add(drawn);
        // }
        //else
        // {
        attached.isAlreadyRevealed = false;
        if (!simulation) 
            attached.MoveTo(pokemon.transform.position, to.anchor.rotation);
        pokemon.attachedCards.Add(attached);
        cards.Remove(attached);
        //}
    }

    public Card GetOrPut(string name)
    {
        return GetOrPut(name, _ => true);
    }

    public Card GetOrPut(string name, Func<Card, bool> predicate)
    {
        var index = cards.FindIndex(card => card.Matcher != null && card.IsThisOrAttached(name) && predicate.Invoke(card));
        if (index != -1) return cards[index];

        var find = cards.Find(card => card.Matcher == null);
        if (find == null)
        {
            Debug.Log("Unable to find available card for " + name);
        }

        find.SetCardMatcher(new CardMatcher(name));
        index = cards.FindIndex(card => card.Matcher != null && card.IsThisOrAttached(name));
        if (index == -1)
        {
            Debug.Log("STILL NOT FOUND " + name + " IN HAND" + find);
        }


        return cards[index];
    }

    public List<Card> GetMatches(string name)
    {
        if (type is Type.DISCARD or Type.DECK)
        {
            var newCard = controller.NewCard(anchor.position);
            newCard.SetCardMatcher(new CardMatcher(name));
            return new List<Card> { newCard };
        }
        var matches = cards.Where(c => c.Matcher != null && c.IsThisOrAttached(name)).ToList();
        return matches.Count > 0 ? matches : new List<Card> { GetOrPut(name) };
    }

    public enum Type
    {
        DECK, DISCARD, BENCH, PRIZE, HAND, ACTIVE
    }
}