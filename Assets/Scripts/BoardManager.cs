using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    [Header("Tile")]
    public GameObject tilePrefab;
    public float tileWidth  = 2.0f;
    public float tileHeight = 3.0f;
    public float tileDepth  = 1.05f;

    [Header("Visual Settings")]
    public TileVisualSettings visualSettings;

    [Header("Scoring")]
    public int pointsPerMatch = 5;

    private List<TileView> allTiles = new List<TileView>();
    private TileView selectedTile;
    private int score;

    private Material[] atlasMaterials;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (visualSettings != null && visualSettings.useAtlas)
            atlasMaterials = visualSettings.BuildAtlasMaterials();

        BuildBoard();
    }

    void BuildBoard()
    {
        List<TileData> tileSet = GenerateTileSet();
        ShuffleTiles(tileSet);

        List<Vector3Int> layout = TurtleLayout.GetPositions();

        int count = Mathf.Min(layout.Count, tileSet.Count);
        for (int i = 0; i < count; i++)
        {
            Vector3Int pos = layout[i];
            SpawnTile(tileSet[i], pos.x, pos.y, pos.z);
        }

        RefreshFreeStates();
    }

    void SpawnTile(TileData data, int x, int y, int layer)
    {
        float worldX = x * tileWidth  * 0.5f;
        float worldY = y * tileHeight * 0.5f;
        float worldZ = -layer * tileDepth;

        GameObject go = Instantiate(tilePrefab, new Vector3(worldX, worldY, worldZ), Quaternion.identity);

        TileView view = go.GetComponent<TileView>();
        view.SetData(data);

        if (visualSettings != null)
        {
            var rend = go.GetComponentInChildren<Renderer>();
            if (rend == null)
            {
                Debug.LogError("Tile prefab has no Renderer — recreate the prefab from the FBX.");
                return;
            }
            ApplyTileVisuals(view, rend, data);
        }

        view.boardX     = x;
        view.boardY     = y;
        view.boardLayer = layer;
        allTiles.Add(view);
    }

    public void OnTileClicked(TileView tile)
    {
        if (!IsFree(tile))
        {
            Debug.Log($"Tile {tile.data?.suit} {tile.data?.value} is NOT free");
            return;
        }

        if (selectedTile == null)
        {
            selectedTile = tile;
            tile.SetSelected(true);
            Debug.Log($"Selected: {tile.data?.suit} {tile.data?.value}");
            return;
        }

        if (selectedTile == tile)
        {
            tile.SetSelected(false);
            selectedTile = null;
            return;
        }

        Debug.Log($"Comparing: {selectedTile.data?.suit} {selectedTile.data?.value}  vs  {tile.data?.suit} {tile.data?.value}");
        if (selectedTile.data.Matches(tile.data))
        {
            allTiles.Remove(selectedTile);
            allTiles.Remove(tile);
            Destroy(selectedTile.gameObject);
            Destroy(tile.gameObject);
            selectedTile = null;

            score += pointsPerMatch;
            GameHUD.Instance?.SetScore(score);

            RefreshFreeStates();
            CheckWinLose();
        }
        else
        {
            selectedTile.SetSelected(false);
            selectedTile = null;
        }
    }

    public void ShowHint()
    {
        var freeTiles = allTiles.FindAll(t => IsFree(t));
        for (int i = 0; i < freeTiles.Count; i++)
            for (int j = i + 1; j < freeTiles.Count; j++)
                if (freeTiles[i].data.Matches(freeTiles[j].data))
                {
                    freeTiles[i].Blink();
                    freeTiles[j].Blink();
                    return;
                }
        Debug.Log("No hints available");
    }

    public bool IsFree(TileView tile)
    {
        foreach (TileView other in allTiles)
        {
            if (other == tile) continue;
            if (other.boardLayer == tile.boardLayer + 1 &&
                Mathf.Abs(other.boardX - tile.boardX) < 2 &&
                Mathf.Abs(other.boardY - tile.boardY) < 2)
                return false;
        }

        bool leftBlocked  = false;
        bool rightBlocked = false;

        foreach (TileView other in allTiles)
        {
            if (other == tile || other.boardLayer != tile.boardLayer) continue;

            if (other.boardX == tile.boardX - 2 && Mathf.Abs(other.boardY - tile.boardY) < 2)
                leftBlocked = true;

            if (other.boardX == tile.boardX + 2 && Mathf.Abs(other.boardY - tile.boardY) < 2)
                rightBlocked = true;
        }

        return !leftBlocked || !rightBlocked;
    }

    void RefreshFreeStates()
    {
        foreach (TileView tile in allTiles)
            tile.SetFree(IsFree(tile));
    }

    void CheckWinLose()
    {
        if (allTiles.Count == 0)
        {
            Debug.Log("You win!");
            return;
        }

        List<TileView> freeTiles = allTiles.FindAll(t => IsFree(t));
        for (int i = 0; i < freeTiles.Count; i++)
            for (int j = i + 1; j < freeTiles.Count; j++)
                if (freeTiles[i].data.Matches(freeTiles[j].data))
                    return;

        Debug.Log("No moves left — game over!");
    }

    List<TileData> GenerateTileSet()
    {
        var tiles = new List<TileData>();

        foreach (TileSuit suit in new[] { TileSuit.Characters, TileSuit.Bamboo, TileSuit.Circles })
            for (int v = 1; v <= 9; v++)
                for (int c = 0; c < 4; c++)
                    tiles.Add(new TileData(suit, v));

        for (int v = 1; v <= 4; v++)
            for (int c = 0; c < 4; c++)
                tiles.Add(new TileData(TileSuit.Winds, v));

        for (int v = 1; v <= 3; v++)
            for (int c = 0; c < 4; c++)
                tiles.Add(new TileData(TileSuit.Dragons, v));

        return tiles;
    }

    public void Shuffle()
    {
        if (selectedTile != null)
        {
            selectedTile.SetSelected(false);
            selectedTile = null;
        }

        var dataList = new List<TileData>();
        foreach (var tile in allTiles)
            dataList.Add(tile.data);

        ShuffleTiles(dataList);

        for (int i = 0; i < allTiles.Count; i++)
        {
            allTiles[i].SetData(dataList[i]);
            if (visualSettings != null)
            {
                var rend = allTiles[i].GetComponentInChildren<Renderer>();
                if (rend != null)
                    ApplyTileVisuals(allTiles[i], rend, dataList[i]);
            }
        }

        RefreshFreeStates();
    }

    void ApplyTileVisuals(TileView view, Renderer rend, TileData data)
    {
        var mats = rend.sharedMaterials;
        if (visualSettings.bodyMaterial != null && mats.Length > 0)
            mats[0] = visualSettings.bodyMaterial;

        if (visualSettings.useAtlas && atlasMaterials != null && mats.Length > 1)
        {
            mats[1] = atlasMaterials[visualSettings.GetAtlasIndex(data)];
            rend.sharedMaterials = mats;
            view.SetSelected(false);
        }
        else
        {
            rend.sharedMaterials = mats;
            view.SetBaseColor(visualSettings.GetBaseColor(data));
        }
    }

    void ShuffleTiles(List<TileData> tiles)
    {
        for (int i = tiles.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (tiles[i], tiles[j]) = (tiles[j], tiles[i]);
        }
    }
}
