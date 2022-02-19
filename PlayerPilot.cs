using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPilot : Player
{
    private bool hasUsedFlight = false;
    protected override bool CanMove(Tile targetTile)
    {
        List<Tile> allTilesButCurrent = TileManager.instance.GetTiles();
        allTilesButCurrent.Remove(TileManager.instance.GetTileAtPos(GetPos())); //
        if (GetPlayerState() == PlayerState.selected && GetSurroundingTiles().Contains(targetTile) && GameManager.instance.GetActions() > 0)
        {
            return true;
        }
        else if (GetPlayerState() == PlayerState.selected && allTilesButCurrent.Contains(targetTile) && GameManager.instance.GetActions() > 0 && !hasUsedFlight)
        {
            hasUsedFlight = true;
            return true;
        }
        return false;
    }
    public override void StartTurn()
    {
        base.StartTurn();
        hasUsedFlight = false;
    }
}
