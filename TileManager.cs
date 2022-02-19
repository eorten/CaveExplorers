using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileManager : MonoBehaviour
{
    #region Serialized variables
    [Header("Tile setup settings")]
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private GameObject tileParent;
    [SerializeField] private int xLength, yLength;
    [SerializeField] private float padding;
    [SerializeField] private float maxDist = 2.7f; //For å fjærne brikkene lengst unna, altså hjørnene
    [SerializeField] private CardNames cn;
    #endregion
    #region singleton
    public static TileManager instance;
    private void Awake()
    {
        instance = this;
        Player.onPlayerClicked += Player_onPlayerClicked;
    }
    #endregion
    
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public void PlaceTiles()
    {
        int nameIndex = 0;
        float offset = (tilePrefab.transform.localScale.x) / 2;
        for (int x = -xLength; x < xLength; x++)
        {
            for (int y = -yLength; y < yLength; y++)
            {
                if ((new Vector3(0, 0, 0) - new Vector3(x + 0.5f, y + 0.5f, 0)).magnitude < maxDist)
                {
                    Tile t = Instantiate(tilePrefab, new Vector3(x * (1 + padding) + offset, y * (1 + padding) + offset, 0), Quaternion.identity, tileParent.transform);
                    CardManager.instance.AddFloodCard(t);
                    t.Initialize(x, y, cn.GetName(nameIndex));
                    nameIndex++;
                    tiles[new Vector2Int(x,y)] = t;
                }               
            }
        }
        Tile.onTileClicked += Tile_onTileClicked;
    }
    private void Player_onPlayerClicked(Player player)
    {

    }
    public List<Tile> GetSurroundingCardinalTiles(Vector2Int pos)
    {
        List<Tile> resList = new List<Tile>();
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(0, 1)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(1, 0)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(0, -1)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(-1, 0)));
        return resList;
    }
    private void TryAdd(List<Tile> list, Tile element)
    {
        if (element == null)
        {
            return;
        }
        if (tiles.TryGetValue(element.GetPos(), out Tile tile))
        {
            list.Add(tile);
        }
    }
    public List<Tile> GetSurroundingTiles(Vector2Int pos)
    {
        List<Tile> resList = new List<Tile>();
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(0, 1)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(1, 0)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(0, -1)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(-1, 0)));

        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(-1, -1)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(-1, 1)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(1, -1)));
        TryAdd(resList, GetTileAtPos(pos + new Vector2Int(1, 1)));
        return resList;
    }
    public Tile GetTileAtPos(Vector2Int pos)
    {
        if (tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }
        return null;
    }
    private void Tile_onTileClicked(Tile tile)
    {
        
    }
    public Tile GetRandomEmptyTile()
    {
        Tile randTile = tiles.Values.ToArray<Tile>()[Random.Range(0, tiles.Values.ToArray().Count())];
        while (randTile.GetItemOnTile() != null)
        {
            randTile = tiles.Values.ToArray()[Random.Range(0, tiles.Values.ToArray().Count())];
        }
        return randTile;
    }
    public List<Tile> GetTiles()
    {
        List<Tile> res = new List<Tile>();
        res.AddRange(tiles.Values.ToArray<Tile>());
        return res;
    }
    public Tile GetTile(int x, int y)
    {
        return tiles[new Vector2Int(x, y)];
    }
    public void RemoveTile(int x, int y)
    {
        tiles.Remove(new Vector2Int(x, y));
    }
    public void RemoveTile(Tile tile)
    {
        tiles.Remove(tile.GetPos());
    }
    private void OnDestroy()
    {
        Tile.onTileClicked -= Tile_onTileClicked;
    }
}
