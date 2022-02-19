using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Tile : MonoBehaviour
{

    [SerializeField] private Color32 normalColor, floodedColor;
    [SerializeField] private GameObject hoverSprite, canMoveSprite;
    private SpriteRenderer sr;
    private TileState tileState = TileState.normal;
    private GameObject itemOnTile;
    public static event Action<Tile> onTileClicked;
    [SerializeField] private int x, y;
    private string tileName;

    public void Initialize(int x, int y, string tileName)
    {
        SetPos(x, y);
        this.tileName = tileName;
    }
    private void OnMouseEnter()
    {
        hoverSprite.SetActive(true);
    }
    private void OnMouseExit()
    {
        hoverSprite.SetActive(false);
    }
    private void OnMouseDown()
    {
        onTileClicked?.Invoke(this);
    }
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        hoverSprite.SetActive(false);
    }
    private void Start()
    {
        canMoveSprite.SetActive(false);
    }
    public void SetPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Vector2Int GetPos()
    {
        return new Vector2Int(x, y);
    }
    public void SwitchTileState(TileState newState)
    {
        tileState = newState;
        print("STS2");
        SetRegularColor();
    }
    public void SetRegularColor()
    {
        switch (tileState)
        {
            case TileState.normal:
                sr.color = normalColor;
                print("normal");
                break;
            case TileState.flooded:
                sr.color = floodedColor;
                print("flood");
                break;
            default:
                break;
        }
    }
    public void SetMoveToSprite(bool state)
    {
        canMoveSprite.SetActive(state);
    }
    public GameObject GetItemOnTile() => itemOnTile;
    public void SetItemOnTile(GameObject item) {
        itemOnTile = item;
    }
    public TileState GetState()
    {
        return tileState;
    }
    public string GetName() => tileName;
}
public enum TileState
{
    normal,
    flooded
}
