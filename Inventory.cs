using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int handSize = 5;
    private List<CardTreasure> inventory = new List<CardTreasure>();



    public void AddCard(CardTreasure card)
    {
        if (card == null) return;
        inventory.Add(card);
        if (inventory.Count > handSize)
        {
            //Kast kort
        }
    }
    public int[] GetInventoryIndexes()
    {
        int[] resArr = new int[6]; //Antall forskjellige treasure-kort. Ikke vannet stiger
        foreach (var item in inventory)
        {
            resArr[item.GetID()] += 1;
        }
        return resArr;
    }
    public void RemoveCard(CardTreasure card)
    {
        if (card == null) return;
        if (!inventory.Contains(card)) return;

        inventory.Remove(card);
    }

    private bool InventoryFull()
    {
        return inventory.Count >= handSize;
    }
}
