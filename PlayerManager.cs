using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    #region singleton
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
        Deck.onDeckClicked += Deck_onDeckClicked;
    }


    #endregion
    //[SerializeField] private Player playerPrefab;
    [SerializeField] private Player[] playerPrefabs;
    private Player activePlayer;
    private List<Player> players = new List<Player>();
    public void CreatePlayers(int amount)
    {
        InstantiatePlayers(amount); //Instansier spillere
        Player.onPlayerClicked += Player_onPlayerClicked; // subscribe til event
        MovePlayersRandomPos(); //Flytt til tilfeldige plasser
    }
    private void Deck_onDeckClicked(DeckType obj)
    {
        

    }
    private void Player_onPlayerClicked(Player player)
    {

    }
    private void MovePlayersRandomPos()
    {
        foreach (var player in players)
        {
            player.Move(TileManager.instance.GetRandomEmptyTile().GetPos());
        }
    }
    private void InstantiatePlayers(int amount)
    {
        amount = Mathf.Clamp(amount, 1, 4);
        for (int i = 0; i < amount; i++)
        {
            print("Spiller lag");
            Player p = Instantiate(playerPrefabs[i]);
            players.Add(p);
        }
    }
    public void NextActivePlayer()
    {
        if (activePlayer == null) ChangeActivePlayer(players[0]);
        else
        {
            int currentActiveIndex = players.IndexOf(activePlayer);
            ChangeActivePlayer((players.Count > (currentActiveIndex + 1)) ? players[currentActiveIndex + 1] : players[0]);
        }
        activePlayer.StartTurn();
    }
    public void ChangeActivePlayer(Player newActive)
    {
        ChangeAllPlayerStates(PlayerState.unactive);
        activePlayer = newActive;
        newActive.SetPlayerState(PlayerState.active);
    }
    private void ChangeAllPlayerStates(PlayerState newState)
    {
        foreach (var player in players)
        {
            player.SetPlayerState(newState);
        }
    }
    private void OnDestroy()
    {
        Player.onPlayerClicked -= Player_onPlayerClicked;
        Deck.onDeckClicked -= Deck_onDeckClicked;
    }
    private PlayerState ActivePlayerState()
    {
        return activePlayer.GetPlayerState();
    }
    private bool HasActions()
    {
        return GameManager.instance.GetActions() > 0;
    }
    public List<Player> GetPlayers() => players;
}

/*
 public class PlayerManager : MonoBehaviour
{
    #region singleton
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    //[SerializeField] private Player playerPrefab;
    [SerializeField] private Player[] playerPrefabs;
    private Player activePlayer;
    private List<Player> players = new List<Player>();
    public void CreatePlayers(int amount)
    {
        InstantiatePlayers(amount); //Instansier spillere
        Player.onPlayerClicked += Player_onPlayerClicked; // subscribe til event
        Tile.onTileClicked += Tile_onTileClicked; // subscribe til tile-event
        MovePlayers(); //Flytt til tilfeldige plasser
    }
    private void Tile_onTileClicked(Tile tile)
    {
        //Action: Move
        if (CanMove(tile))
        {
            activePlayer.Move(tile.GetPos());
            GameManager.instance.ReduceAction();
        }
        else if (CanDrain(tile))
        {
            tile.SwitchTileState(TileState.normal);
            GameManager.instance.ReduceAction();
        }
    }
    private bool CanMove(Tile targetTile)
    {
        List<Tile> surroundingTiles = TileManager.instance.GetSurroundingTiles(activePlayer.GetPos());
        if (activePlayer.GetPlayerState() == PlayerState.selected && surroundingTiles.Contains(targetTile) && GameManager.instance.GetActions() > 0)
        {
            return true;
        }
        return false;
    }
    private bool CanDrain(Tile targetTile)
    {
        List<Tile> surroundingTiles = TileManager.instance.GetSurroundingTiles(activePlayer.GetPos()); //Tiles rundt
        surroundingTiles.Add(TileManager.instance.GetTileAtPos(activePlayer.GetPos())); //Tile spilleren står på
        if (ActivePlayerState() == PlayerState.active && surroundingTiles.Contains(targetTile) && HasActions() && targetTile.GetState() == TileState.flooded)
        {
            return true;
        }
        return false;
    }
    private void MovePlayers()
    {
        foreach (var player in players)
        {
            player.Move(TileManager.instance.GetRandomEmptyTile().GetPos());
        }
    }
    private void Player_onPlayerClicked(Player player)
    {
        //Change clicked player state
        if (player == activePlayer)
        {
            player.SetPlayerState(player.GetPlayerState() == PlayerState.active ? PlayerState.selected : PlayerState.active);
        }
    }
    private void InstantiatePlayers(int amount)
    {
        amount = Mathf.Clamp(amount, 1, 4);
        for (int i = 0; i < amount; i++)
        {
            Player p = Instantiate(playerPrefabs[i]);
            players.Add(p);
        }
    }
    public void NextActivePlayer()
    {
        if (activePlayer == null) ChangeActivePlayer(players[0]);
        else
        {
            int currentActiveIndex = players.IndexOf(activePlayer);
            ChangeActivePlayer((players.Count > (currentActiveIndex + 1)) ? players[currentActiveIndex + 1] : players[0]);
        }

    }
    public void ChangeActivePlayer(Player newActive)
    {
        ChangeAllPlayerStates(PlayerState.unactive);
        activePlayer = newActive;
        newActive.SetPlayerState(PlayerState.active);
    }
    private void ChangeAllPlayerStates(PlayerState newState)
    {
        foreach (var player in players)
        {
            player.SetPlayerState(newState);
        }
    }
    private void OnDestroy()
    {
        Player.onPlayerClicked -= Player_onPlayerClicked;
    }
    private PlayerState ActivePlayerState()
    {
        return activePlayer.GetPlayerState();
    }
    private bool HasActions()
    {
        return GameManager.instance.GetActions() > 0;
    }
}
 */