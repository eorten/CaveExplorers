using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlood : Card
{
    private Tile tile;
    public void Initialize(Tile tile)
    {
        this.tile = tile;
        cardType = CardType.Flood;
    }
    public Tile GetTile() => tile;
}
