using UnityEngine;

[CreateAssetMenu(fileName = "TileVisualSettings", menuName = "Mahjong/Tile Visual Settings")]
public class TileVisualSettings : ScriptableObject
{
    [Header("Материалы тайла")]
    public Material bodyMaterial;  // Element 0 — тело (base)
    public Material faceMaterial;  // Element 1 — лицо (front), должен иметь атлас в Base Map

    [Header("Атлас")]
    public bool useAtlas = false;
    public int atlasColumns = 6;
    public int atlasRows    = 6;
    // Порядок в атласе (слева-направо, сверху-вниз):
    //  0- 2 : Dragons 1-3
    //  3- 6 : Winds 1-4
    //  7-15 : Characters 1-9
    // 16-24 : Bamboo 1-9
    // 25-33 : Circles 1-9

    public int GetAtlasIndex(TileData data) => data.suit switch
    {
        TileSuit.Dragons    => data.value - 1,
        TileSuit.Winds      => 3  + (data.value - 1),
        TileSuit.Characters => 7  + (data.value - 1),
        TileSuit.Bamboo     => 16 + (data.value - 1),
        TileSuit.Circles    => 25 + (data.value - 1),
        _                   => 0
    };

    // Создаёт 34 инстанса faceMaterial с запечёнными UV — один на каждый тип тайла
    public Material[] BuildAtlasMaterials()
    {
        int total = atlasColumns * atlasRows;
        var mats  = new Material[total];
        float tw  = 1f / atlasColumns;
        float th  = 1f / atlasRows;

        for (int i = 0; i < total; i++)
        {
            var mat  = new Material(faceMaterial);
            int col  = i % atlasColumns;
            int row  = atlasRows - 1 - (i / atlasColumns); // UV снизу
            mat.SetTextureOffset("_BaseMap", new Vector2(col * tw, row * th));
            mat.SetTextureScale ("_BaseMap", new Vector2(tw, th));
            mats[i] = mat;
        }
        return mats;
    }

    [Header("Цвета мастей (если useAtlas = false)")]
    public Color charactersColor = new Color(0.85f, 0.25f, 0.20f);
    public Color bambooColor     = new Color(0.15f, 0.65f, 0.25f);
    public Color circlesColor    = new Color(0.15f, 0.35f, 0.85f);
    public Color windsColor      = new Color(0.75f, 0.65f, 0.10f);
    public Color dragonsColor    = new Color(0.60f, 0.10f, 0.75f);

    public Color GetBaseColor(TileData data) => data.suit switch
    {
        TileSuit.Characters => charactersColor,
        TileSuit.Bamboo     => bambooColor,
        TileSuit.Circles    => circlesColor,
        TileSuit.Winds      => windsColor,
        TileSuit.Dragons    => dragonsColor,
        _                   => Color.white
    };
}
