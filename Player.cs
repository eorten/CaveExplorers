using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private bool thisPlayerTurn;
    private int actions = 0;
    Collider2D m_Collider;
    [SerializeField] private Color32 unactiveColor, activeColor, selectedColor;
    [SerializeField] private Sprite unactiveSprite, activeSprite;
    private int xPos, yPos;
    private PlayerState playerState;
    public static event Action<Player> onPlayerClicked;
    public static event Action<Player> onPlayerAction;
    private SpriteRenderer sr;
    private Inventory inventory;


    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        m_Collider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        SetPlayerState(PlayerState.unactive);
        MoveBackZ();
        Deck.onDeckClicked += Deck_onDeckClicked;
        Tile.onTileClicked += Tile_onTileClicked; // subscribe til tile-event
    }

    private void Deck_onDeckClicked(DeckType obj)
    {


    }

    private void OnMouseDown()
    {
        if (thisPlayerTurn)
        {
            SetPlayerState(playerState == PlayerState.active ? PlayerState.selected : PlayerState.active);
        }
        CheckSurroundingTileFrame();
        onPlayerClicked?.Invoke(this);
    }
    private void Tile_onTileClicked(Tile tile)
    {
        //Action: Move

        if (CanDrain(tile))
        {
            tile.SwitchTileState(TileState.normal);
            ReduceActions();
        }
        else if (CanMove(tile))
        {
            Move(tile.GetPos());
            ReduceActions();
        }
    }
    public virtual void StartTurn()
    {
        thisPlayerTurn = true;
        actions = 3;
    }
    public void SetPlayerState(PlayerState newState)
    {
        playerState = newState;
        switch (newState)
        {
            case PlayerState.unactive:
                SetColor(unactiveColor);
                SetSprite(unactiveSprite);
                MoveBackZ();
                break;
            case PlayerState.active:
                SetColor(activeColor);
                SetSprite(unactiveSprite);
                MoveFrontZ();               
                break;
            case PlayerState.selected:
                SetColor(selectedColor);
                SetSprite(activeSprite);
                MoveFrontZ();
                break;
            default:
                break;
        }
    }
    public void Move(Vector2Int pos)
    {
        SetSurroundingTileFrame(false);

        xPos = pos.x;
        yPos = pos.y;
        Tile t = TileManager.instance.GetTileAtPos(pos);
        transform.position = t.transform.position;
        t.SetItemOnTile(gameObject);

        CheckSurroundingTileFrame();
    }
    public Vector2Int GetPos() => new Vector2Int(xPos, yPos);
    public PlayerState GetPlayerState() => playerState;
    public int GetActions() => actions;
    public Inventory GetInventory() => inventory;
    protected virtual bool CanMove(Tile targetTile)
    {
        if (GetPlayerState() == PlayerState.selected && GetSurroundingTiles().Contains(targetTile) && GameManager.instance.GetActions() > 0)
        {
            return true;
        }
        return false;
    }
    private bool CanDrain(Tile targetTile)
    {
        List<Tile> surroundingTiles = GetSurroundingTiles(); //Tiles rundt
        surroundingTiles.Add(TileManager.instance.GetTileAtPos(GetPos())); //Tile spilleren står på

        if (playerState == PlayerState.active && surroundingTiles.Contains(targetTile) && actions > 0 && targetTile.GetState() == TileState.flooded)
        {
            return true;
        }
        return false;
    }
    protected virtual List<Tile> GetSurroundingTiles()
    {
        return TileManager.instance.GetSurroundingCardinalTiles(GetPos());
    }
    protected void CheckSurroundingTileFrame()
    {
        switch (playerState)
        {
            case PlayerState.unactive:
                break;
            case PlayerState.active:
                foreach (Tile tile in GetSurroundingTiles())
                {

                    tile.SetMoveToSprite(false);
                    
                    
                }
                break;
            case PlayerState.selected:
                foreach (Tile tile in GetSurroundingTiles())
                {

                    tile.SetMoveToSprite(true);
                    
                }
                break;
            default:
                break;
        }

    }
    protected void SetSurroundingTileFrame(bool state)
    {
        foreach (Tile tile in GetSurroundingTiles())
        {

            tile.SetMoveToSprite(state);
            
        }
    }
    private void SetColor(Color32 newColor)
    {
        sr.color = newColor;
    }
    private void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }
    private void MoveFrontZ()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        m_Collider.enabled = true;
    }
    private void MoveBackZ()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.9f);
        m_Collider.enabled = false;
    }
    private void ReduceActions()
    {
        actions--;
        if (actions <= 0)
        {
            DrawTreasureCards(2);
            DrawFloodCards();
            thisPlayerTurn = false;
            SetSurroundingTileFrame(false);
            GameManager.instance.ChangeGameState(GameState.turn);
        }
        onPlayerAction?.Invoke(this);
    }
    private void DrawTreasureCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GetInventory().AddCard(CardManager.instance.DrawTreasureCard());
        }
    }
    private void DrawFloodCards()
    {
        for (int i = 0; i < FloodManager.instance.GetWaterLevel(); i++)
        {
            CardManager.instance.DrawFloodCard();
        }
    }
    private void OnDestroy()
    {
        Deck.onDeckClicked -= Deck_onDeckClicked;
        Tile.onTileClicked -= Tile_onTileClicked;
    }
}
public enum PlayerState
{
    unactive,
    active,  
    selected
}
/*
 public class Player : MonoBehaviour
{
    Collider2D m_Collider;
    [SerializeField] private Color32 unactiveColor, activeColor, selectedColor;
    [SerializeField] private Sprite unactiveSprite, activeSprite;
    private int xPos, yPos;
    private PlayerState playerState;
    public static event Action<Player> onPlayerClicked;
    private SpriteRenderer sr;
    private void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        SetPlayerState(PlayerState.unactive);
        MoveBackZ();
    }
    private void OnMouseDown()
    {
        onPlayerClicked?.Invoke(this);
    }
    public void SetPlayerState(PlayerState newState)
    {
        playerState = newState;
        switch (newState)
        {
            case PlayerState.unactive:
                SetColor(unactiveColor);
                SetSprite(unactiveSprite);
                MoveBackZ();
                break;
            case PlayerState.active:
                SetColor(activeColor);
                SetSprite(unactiveSprite);
                MoveFrontZ();
               
                break;
            case PlayerState.selected:
                SetColor(selectedColor);
                SetSprite(activeSprite);
                MoveFrontZ();
                break;
            default:
                break;
        }
    }

    private void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }

    private void MoveFrontZ()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        m_Collider.enabled = true;
    }
    private void MoveBackZ()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.9f);
        m_Collider.enabled = false;
    }
    public void Move(Vector2Int pos)
    {
        xPos = pos.x;
        yPos = pos.y;
        Tile t = TileManager.instance.GetTileAtPos(pos);
        transform.position = t.transform.position;
        t.SetItemOnTile(gameObject);
    }
    public Vector2Int GetPos()
    {
        return new Vector2Int(xPos, yPos);
    }
    public PlayerState GetPlayerState()
    {
        return playerState;
    }
    private void SetColor(Color32 newColor)
    {
        sr.color = newColor;
    }
}
public enum PlayerState
{
    unactive,
    active,  
    selected
}
 */
