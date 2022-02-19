using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Deck : MonoBehaviour
{
    [SerializeField] private DeckType deckType;
    public static event Action<DeckType> onDeckClicked;
    private void OnMouseDown()
    {
        onDeckClicked?.Invoke(deckType);
    }
}
public enum DeckType
{
    treasureDeck,
    floodDeck
}
