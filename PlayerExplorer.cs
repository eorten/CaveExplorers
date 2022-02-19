using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplorer : Player
{
    protected override List<Tile> GetSurroundingTiles()
    {
        return TileManager.instance.GetSurroundingTiles(GetPos());
    }
}
