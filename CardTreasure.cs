using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTreasure : Card
{
    public void Initialize(int id, CardType cardType)
    {
        this.id = id;
        this.cardType = cardType;
    }
    public int GetID() => id;

}
