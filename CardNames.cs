using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Names", menuName = "New card")]
public class CardNames : ScriptableObject
{
    public string[] names;
    public string GetName(int index) => names[index];
}
