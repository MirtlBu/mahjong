using System.Collections;
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
    public int winBonus = 100;

    private List<TileView> allTiles = new List<TileView>();
    private TileView selectedTile;
    private TileView hintTileA, hintTileB;
    private bool isAnimating;
    private int score;

    private Material[] atlasMaterials;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
            StartCoroutine(SolveCoroutine());
#endif
    }

    [ContextMenu("Debug: Trigger Win")]
    public void DebugTriggerWin()
    {
        int baseScore = score;
        score += winBonus;
        GameHUD.Instance?.SetScore(score);
        GameHUD.Instance?.ShowVictory(baseScore, score);
        GameManager.Instance?.OnLevelComplete(score);
    }

    IEnumerator SolveCoroutine()
    {
        while (allTiles.Count > 0)
        {
            var freeTiles = allTiles.FindAll(t => IsFree(t));
            TileView tileA = null, tileB = null;
            for (int i = 0; i < freeTiles.Count && tileA == null; i++)
                for (int j = i + 1; j < freeTiles.Count && tileA == null; j++)
                    if (freeTiles[i].data.Matches(freeTiles[j].data))
                    { tileA = freeTiles[i]; tileB = freeTiles[j]; }

            if (tileA == null) break;

            allTiles.Remove(tileA);
            allTiles.Remove(tileB);
            Vector3 meet = (tileA.transform.position + tileB.transform.position) * 0.5f;
            tileA.PlayMatchAnimation(meet);
            tileB.PlayMatchAnimation(meet);
            score += pointsPerMatch;
            GameHUD.Instance?.SetScore(score);
            RefreshFreeStates();

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(0.5f);
        CheckWinLose();
    }

    void Start()
    {
        if (visualSettings != null && visualSettings.useAtlas)
            atlasMaterials = visualSettings.BuildAtlasMaterials();

        BuildBoard();
    }

    public void Rebuild()
    {
        if (selectedTile != null)
        {
            selectedTile.SetSelected(false);
            selectedTile = null;
        }

        foreach (var tile in allTiles)
            if (tile != null) Destroy(tile.gameObject);

        allTiles.Clear();
        score = 0;

        BuildBoard();
    }

    void BuildBoard()
    {
        var levelLayout = GameManager.Instance?.CurrentLevel;
        List<Vector3Int> layout = levelLayout != null
            ? levelLayout.GetPositions()
            : TurtleLayout.GetPositions();

        List<TileData> tileSet = GenerateTileSet(layout.Count);
        ShuffleTiles(tileSet);

        int count = tileSet.Count; // may be layout.Count-1 if layout has odd number of positions
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

    void StopHintBlink()
    {
        hintTileA?.StopBlink();
        hintTileB?.StopBlink();
        hintTileA = null;
        hintTileB = null;
    }

    public void OnTileClicked(TileView tile)
    {
        StopHintBlink();
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
            Vector3 meetPoint = (selectedTile.transform.position + tile.transform.position) * 0.5f;
            selectedTile.PlayMatchAnimation(meetPoint);
            tile.PlayMatchAnimation(meetPoint);
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
        if (isAnimating) return;
        if (selectedTile != null)
        {
            selectedTile.SetSelected(false);
            selectedTile = null;
        }

        var freeTiles = allTiles.FindAll(t => IsFree(t));
        for (int i = 0; i < freeTiles.Count; i++)
            for (int j = i + 1; j < freeTiles.Count; j++)
                if (freeTiles[i].data.Matches(freeTiles[j].data))
                {
                    if (GameManager.Instance != null && !GameManager.Instance.UseHint())
                    {
                        Debug.Log("No hints remaining");
                        return;
                    }
                    hintTileA = freeTiles[i];
                    hintTileB = freeTiles[j];
                    hintTileA.Blink();
                    hintTileB.Blink();
                    return;
                }

        GameHUD.Instance?.ShowNoMoves(false);
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
            int baseScore = score;
            score += winBonus;
            GameHUD.Instance?.SetScore(score);
            GameHUD.Instance?.ShowVictory(baseScore, score);
            GameManager.Instance?.OnLevelComplete(score);
            Debug.Log($"You win! Final score: {score}");
            return;
        }

        List<TileView> freeTiles = allTiles.FindAll(t => IsFree(t));
        for (int i = 0; i < freeTiles.Count; i++)
            for (int j = i + 1; j < freeTiles.Count; j++)
                if (freeTiles[i].data.Matches(freeTiles[j].data))
                    return;

        Debug.Log("CheckWinLose: no valid pairs found");
        var gm = GameManager.Instance;
        bool hasShuffles = gm == null || gm.ShuffleCount > 0;
        bool shuffleUseless = IsShuffleUseless();
        bool gameOver = !hasShuffles || shuffleUseless;
        GameHUD.Instance?.ShowNoMoves(gameOver);
        if (gameOver)
            gm?.OnGameOver();
    }

    bool IsShuffleUseless()
    {
        // No free tiles at all — shuffle only moves data, positions stay the same
        if (!allTiles.Exists(t => IsFree(t)))
            return true;

        // Exactly 2 tiles left — if they could match they'd have been found already
        if (allTiles.Count == 2)
            return true;

        return false;
    }

    List<TileData> GenerateTileSet(int count)
    {
        // Build pool of unique tile types
        var types = new List<TileData>();
        foreach (TileSuit suit in new[] { TileSuit.Characters, TileSuit.Bamboo, TileSuit.Circles })
            for (int v = 1; v <= 9; v++)
                types.Add(new TileData(suit, v));
        for (int v = 1; v <= 4; v++)
            types.Add(new TileData(TileSuit.Winds, v));
        for (int v = 1; v <= 3; v++)
            types.Add(new TileData(TileSuit.Dragons, v));

        // Generate exactly count tiles in pairs, cycling through types
        count = (count / 2) * 2; // ensure even
        var tiles = new List<TileData>(count);
        int typeIndex = 0;
        while (tiles.Count < count)
        {
            var t = types[typeIndex % types.Count];
            tiles.Add(new TileData(t.suit, t.value));
            tiles.Add(new TileData(t.suit, t.value));
            typeIndex++;
        }
        return tiles;
    }

    public void Shuffle()
    {
        if (isAnimating) return;
        if (GameManager.Instance != null && !GameManager.Instance.UseShuffle())
        {
            Debug.Log("No shuffles remaining");
            return;
        }
        StartCoroutine(ShuffleAnimation());
    }

    IEnumerator ShuffleAnimation()
    {
        isAnimating = true;
        if (selectedTile != null)
        {
            selectedTile.SetSelected(false);
            selectedTile = null;
        }
        GameHUD.Instance?.HideNoMoves();

        var tiles = new List<TileView>(allTiles);
        var originalPositions = new List<Vector3>();
        foreach (var t in tiles) originalPositions.Add(t.transform.position);

        Vector3 center = Vector3.zero;
        foreach (var p in originalPositions) center += p;
        center /= originalPositions.Count;
        float riseZ = originalPositions[0].z - 4f;

        // Phase 1: anticipation — чуть вверх по Z и разлёт от центра
        foreach (var tile in tiles)
        {
            Vector3 scatter = tile.transform.position - center;
            scatter.z = 0f;
            Vector3 target = tile.transform.position + scatter.normalized * 1.5f + new Vector3(0f, 0f, -2f);
            tile.StartCoroutine(tile.AnimateTo(target, 0.3f));
        }
        yield return new WaitForSeconds(0.32f);

        // Phase 2: летят к центру + вращение кластера на 360° по Z
        Vector3 centerRisen = new Vector3(center.x, center.y, riseZ);
        var phase2Start = new List<Vector3>();
        foreach (var tile in tiles) phase2Start.Add(tile.transform.position);

        float phase2Duration = 0.5f;
        float phase2t = 0f;
        bool shakeFired = false;
        while (phase2t < 1f)
        {
            phase2t += Time.deltaTime / phase2Duration;
            if (!shakeFired && phase2t >= 0.85f)
            {
                shakeFired = true;
                CameraOrbit.Instance?.Shake(0.45f, 0.35f);
            }
            float smooth = Mathf.SmoothStep(0f, 1f, phase2t);
            float angle = smooth * 360f * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angle), sin = Mathf.Sin(angle);

            for (int i = 0; i < tiles.Count; i++)
            {
                Vector3 rel = Vector3.Lerp(phase2Start[i] - centerRisen, Vector3.zero, smooth);
                Vector3 rotated = new Vector3(
                    rel.x * cos - rel.y * sin,
                    rel.x * sin + rel.y * cos,
                    rel.z
                );
                tiles[i].transform.position = centerRisen + rotated;
            }
            yield return null;
        }

        // Collision: новые данные

        var dataList = new List<TileData>();
        foreach (var tile in tiles) dataList.Add(tile.data);
        ShuffleTiles(dataList);
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].SetData(dataList[i]);
            if (visualSettings != null)
            {
                var rend = tiles[i].GetComponentInChildren<Renderer>();
                if (rend != null) ApplyTileVisuals(tiles[i], rend, dataList[i]);
            }
        }

        // Phase 3: разлетаемся обратно на свои места
        for (int i = 0; i < tiles.Count; i++)
            tiles[i].StartCoroutine(tiles[i].AnimateTo(originalPositions[i], 0.4f));
        yield return new WaitForSeconds(0.42f);

        RefreshFreeStates();
        isAnimating = false;
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
