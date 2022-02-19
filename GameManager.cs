using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField] private int round, floodStartTilesCount = 6;
    private GameState currentState;
    public static event Action<GameState> OnGameStateChanged;

    private int actionsLeft = 0;
    private void Start()
    {
        ChangeGameState(GameState.prep);
    }
    public void ChangeGameState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.menu:
                HandleMenu();
                break;
            case GameState.prep:
                HandlePrep();
                break;
            case GameState.turn:
                HandleTurn();
                break;
            case GameState.lost:
                break;
            case GameState.won:
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandlePrep()
    {
        TileManager.instance.PlaceTiles();
        
        FloodStartTiles();
        PlayerManager.instance.CreatePlayers(3);
        CreatePlayerUI();
        CardManager.instance.MakeCardTreasure();

        //Spillere trekker 2 kort hver
        PlayersDrawCards(2);
        CardManager.instance.AddWatersRiseCards();

        ChangeGameState(GameState.turn);
    }

    private void PlayersDrawCards(int amount)
    {       
        foreach (var p in PlayerManager.instance.GetPlayers())
        {
            for (int i = 0; i < amount; i++)
            {
                CardTreasure c = CardManager.instance.DrawTreasureCard();
                p.GetInventory().AddCard(c);
            }

        }
    }

    private void FloodStartTiles()
    {
        CardManager cm = CardManager.instance;
        cm.Shuffle(cm.GetCardsFlood());
        for (int i = 0; i < floodStartTilesCount; i++)
        {
            cm.DrawFloodCard();
        }
    }
    
    private void HandleMenu()
    {
        //throw new NotImplementedException();
    }

    private void HandleTurn()
    {
        actionsLeft = 3;
        PlayerManager.instance.NextActivePlayer();
        //CardManager.instance.DrawCards(x);
        UIManager.instance.UpdateAllInfoPanels();
    }

    public int GetRound() => round;
    public int GetActions() => actionsLeft;

    private void CreatePlayerUI()
    {
        foreach (Player p in PlayerManager.instance.GetPlayers())
        {
            UIManager.instance.MakeInfoPanel(p);
        }
    }

}
public enum GameState
{
    menu,
    prep,
    turn,
    lost,
    won
}
