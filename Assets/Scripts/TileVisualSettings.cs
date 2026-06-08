using UnityEngine;

[CreateAssetMenu(fileName = "TileVisualSettings", menuName = "Mahjong/Tile Visual Settings")]
public class TileVisualSettings : ScriptableObject
{
    [Header("Tile Materials")]
    public Material bodyMaterial;  // Element 0 — body
    public Material faceMaterial;  // Element 1 — face (front), must have atlas in Base Map

    [Header("Atlas")]
    public bool useAtlas = false;
    public int atlasColumns = 6;
    public int atlasRows = 6;
    // Atlas order (left-to-right, top-to-bottom):
    //  0- 2 : Dragons 1-3
    //  3- 6 : Winds 1-4
    //  7-15 : Characters 1-9
    // 16-24 : Bamboo 1-9
    // 25-33 : Circles 1-9

    public int GetAtlasIndex(TileData data) => data.suit switch
    {
        TileSuit.Dragons => data.value - 1,
        TileSuit.Winds => 3 + (data.value - 1),
        TileSuit.Characters => 7 + (data.value - 1),
        TileSuit.Bamboo => 16 + (data.value - 1),
        TileSuit.Circles => 25 + (data.value - 1),
        _ => 0
    };

    // Creates one material instance per tile type with baked UV offsets
    public Material[] BuildAtlasMaterials()
    {
        int total = atlasColumns * atlasRows;
        var mats = new Material[total];
        float tw = 1f / atlasColumns;
        float th = 1f / atlasRows;

        float faceAspect = 2f / 3f; // face width/height ratio
        for (int i = 0; i < total; i++)
        {
            var mat = new Material(faceMaterial);
            int col = i % atlasColumns;
            int row = atlasRows - 1 - (i / atlasColumns); // UV снизу
            float uniformScale = 1.2f;
            float scaleX = tw * faceAspect * uniformScale;
            float scaleY = th * uniformScale;
            float offsetX = col * tw + (tw - scaleX) * 0.5f; // center horizontally
            float offsetY = row * th + (th - scaleY) * 0.5f; // center vertically
            mat.SetTextureOffset("_BaseMap", new Vector2(offsetX, offsetY));
            mat.SetTextureScale("_BaseMap", new Vector2(scaleX, scaleY));
            mats[i] = mat;
        }
        return mats;
    }

    [Header("Suit Colors (used when useAtlas = false)")]
    public Color charactersColor = new Color(0.85f, 0.25f, 0.20f);
    public Color bambooColor = new Color(0.15f, 0.65f, 0.25f);
    public Color circlesColor = new Color(0.15f, 0.35f, 0.85f);
    public Color windsColor = new Color(0.75f, 0.65f, 0.10f);
    public Color dragonsColor = new Color(0.60f, 0.10f, 0.75f);

    public Color GetBaseColor(TileData data) => data.suit switch
    {
        TileSuit.Characters => charactersColor,
        TileSuit.Bamboo => bambooColor,
        TileSuit.Circles => circlesColor,
        TileSuit.Winds => windsColor,
        TileSuit.Dragons => dragonsColor,
        _ => Color.white
    };
}
