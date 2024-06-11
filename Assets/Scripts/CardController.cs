using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Board board;
    public Card blueprint;

    private Dictionary<CardHolder.Type, CardHolder> holders = new();
    public CardHolder deck;
    public CardHolder discard;
    public CardHolder bench;
    public CardHolder prize;
    public CardHolder hand;
    public CardHolder active;
    public bool simulation = false;

    public CardHolder Get(CardHolder.Type type)
    {
        return holders[type];
    }
    
    
    void Start()
    {
        deck = new(CardHolder.Type.DECK, board.deckAnchor, this);
        discard = new(CardHolder.Type.DISCARD, board.discardAnchor, this);
        bench = new(CardHolder.Type.BENCH, board.benchAnchor, this);
        prize = new(CardHolder.Type.PRIZE, board.prizeAnchor, this);
        hand = new(CardHolder.Type.HAND, board.handAnchor, this);
        active = new(CardHolder.Type.ACTIVE, board.activeAnchor, this);
        holders[CardHolder.Type.DECK] = deck;
        holders[CardHolder.Type.DISCARD] = discard;
        holders[CardHolder.Type.BENCH] = bench;
        holders[CardHolder.Type.PRIZE] = prize;
        holders[CardHolder.Type.HAND] = hand;
        holders[CardHolder.Type.ACTIVE] = active;
        
        
        for (var i = 0; i < 6; i++)
        {
            var newCard = NewCard(board.deckAnchor.transform.position + (Vector3.up * i / 10));
            newCard.facingUp = false;
            deck.Add(newCard);
            
            var newPrize = NewCard(board.prizeAnchor.transform.position + (Vector3.up * i / 10));
            newPrize.facingUp = false;
            prize.Add(newPrize);
        }
        if (!simulation) 
            prize.Rearrange();
        
    }

    public void Move(Card card, CardHolder.Type from, CardHolder.Type to)
    {
        holders[from].MoveTo(card, holders[to], simulation);
    }
    
    public void AttachTo( Card pokemon, Card attachment, CardHolder.Type from, CardHolder.Type to)
    {
        holders[from].AttachTo(pokemon, attachment, holders[to], simulation);
    }

    public Card NewCard(Vector3 position)
    {
      return Instantiate(blueprint, position, blueprint.transform.rotation);
    }

    public List<Card> DrawCards(int amount)
    {
        List<Card> drawnCards = new();
        for (var i = 0; i < amount; i++)
        {
            var drawn = NewCard(board.deckAnchor.transform.position);
            drawn.facingUp = true;
            hand.Add(drawn);
            drawnCards.Add(drawn);
        }
        if (!simulation) hand.Rearrange();
        return drawnCards;
    }
    
    void Update()
    {
        
    }
}
