using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    #region Singleton
    public static CardManager instance;
    private void Awake()
    {
        instance = this;
        Deck.onDeckClicked += Deck_onDeckClicked;
    }
    #endregion
    [SerializeField] private int treasureCardCount = 5, watersRiseCardCount = 3, helicopterCardCount = 3, sandbagCardCount = 2;
    [SerializeField] private GameObject[] treasures;
    //[SerializeField] private Card cardPrefab;
    
    private List<CardTreasure> cardsTreasure = new List<CardTreasure>();
    private List<CardFlood> cardsFlood = new List<CardFlood>();

    private List<CardTreasure> discardedCardsTreasure = new List<CardTreasure>();
    private List<CardFlood> discardedCardsFlood = new List<CardFlood>();
    

    private void Deck_onDeckClicked(DeckType deckType)
    {
        switch (deckType)
        {
            case DeckType.treasureDeck:
                DrawTreasureCard();
                DrawTreasureCard();
                break;
            case DeckType.floodDeck:
                for (int i = 0; i < FloodManager.instance.GetWaterLevel(); i++)
                {
                    DrawFloodCard();
                }
                break;
            default:
                break;
        }
    }
    public void MakeCardTreasure()
    {
        int k = 0;
        for (int i = 0; i < helicopterCardCount; i++)
        {
            AddCardTreasure(k, CardType.SpecialTresure);
        }
        k++;
        for (int i = 0; i < sandbagCardCount; i++)
        {
            AddCardTreasure(k, CardType.SpecialTresure);
        }
        k++;
        for (int i = 0; i < treasureCardCount; i++)
        {
            for (int j = k; j < treasures.Length + k; j++)
            {
                AddCardTreasure(j, CardType.Treasure);
            }
        }



        Shuffle(cardsTreasure);
    }
    public void AddFloodCard(Tile tile)
    {
        CardFlood cf = new CardFlood();
        cardsFlood.Add(cf);
        cf.Initialize(tile);
    }
    public void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T tempCard = list[k];
            list[k] = list[n];
            list[n] = tempCard;
        }
    }
    public CardTreasure DrawTreasureCard()
    {
        if (cardsTreasure.Count < 1)
        {
            cardsTreasure = discardedCardsTreasure;
            Shuffle(cardsTreasure);
            discardedCardsTreasure.Clear();
        }
        CardTreasure card = cardsTreasure[0];
        cardsTreasure.RemoveAt(0);
        if (card.GetCardType() == CardType.WatersRise)
        {
            FloodManager.instance.IncreaseWaterLevel();
            Shuffle(discardedCardsFlood);
            for (int i = 0; i < discardedCardsFlood.Count; i++)
            {
                cardsFlood.Insert(i, discardedCardsFlood[i]);
            }
            discardedCardsFlood.Clear();
            discardedCardsTreasure.Add(card);
            return null;
        }
        return card;
    }
    public CardFlood DrawFloodCard()
    {
        if (cardsFlood.Count < 1)
        {
            cardsFlood = discardedCardsFlood;
            Shuffle(cardsFlood);
            discardedCardsFlood.Clear();
        }
        CardFlood card = cardsFlood[0];
        cardsFlood.RemoveAt(0);
        card.GetTile().SwitchTileState(TileState.flooded);
        return card;
    }
    public void AddWatersRiseCards()
    {
        for (int i = 0; i < watersRiseCardCount; i++)
        {
            AddCardTreasure(treasures.Length + 2, CardType.WatersRise); //1 for sandbag, 1 for helikopter = 2
        }
        Shuffle(cardsTreasure);
    }

    private void AddCardTreasure(int id, CardType cardType)
    {
        CardTreasure ct = new CardTreasure();
        cardsTreasure.Add(ct);
        ct.Initialize(id, cardType);
    }
    public List<CardFlood> GetCardsFlood() => cardsFlood;
    private void OnDestroy()
    {
        Deck.onDeckClicked -= Deck_onDeckClicked;
    }
}
