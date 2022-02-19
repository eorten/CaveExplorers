using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    protected CardType cardType;
    protected int id;
    public CardType GetCardType() => cardType;
}
public enum CardType{
    Flood,
    Treasure,
    SpecialTresure,
    WatersRise
}