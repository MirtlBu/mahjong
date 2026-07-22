using UnityEngine;

/// <summary>
/// Attach to any GameObject in GameScene.
/// Assign the same TileVisualSettings used by BoardManager.
/// In Play mode draws an atlas preview with the target cell highlighted.
/// atlasIndex is 0-based (cell "35 counting from 1" = atlasIndex 34).
/// </summary>
public class DebugAtlasViewer : MonoBehaviour
{
    [Header("Settings")]
    public TileVisualSettings visualSettings;

    [Header("Cell to inspect (0-based)")]
    [Tooltip("0-based: cell 1 = 0, cell 35 = 34, cell 36 = 35")]
    public int atlasIndex = 34;

    [Header("Display")]
    public bool show = true;
    public int previewSize = 360;   // square px

    private GUIStyle _labelStyle;

    void OnGUI()
    {
        if (!show || visualSettings == null || visualSettings.faceMaterial == null) return;

        if (_labelStyle == null)
        {
            _labelStyle = new GUIStyle(GUI.skin.label);
            _labelStyle.fontSize = 10;
            _labelStyle.normal.textColor = Color.white;
        }

        Texture2D tex = visualSettings.faceMaterial.GetTexture("_BaseMap") as Texture2D;
        if (tex == null)
        {
            GUI.Label(new Rect(10, 10, 400, 30), "[DebugAtlas] faceMaterial has no _BaseMap texture!");
            return;
        }

        int cols = visualSettings.atlasColumns;
        int rows = visualSettings.atlasRows;
        int idx  = Mathf.Clamp(atlasIndex, 0, cols * rows - 1);

        // Draw full atlas
        Rect atlasRect = new Rect(10, 10, previewSize, previewSize);
        GUI.DrawTexture(atlasRect, tex, ScaleMode.StretchToFill);

        float cw = (float)previewSize / cols;
        float ch = (float)previewSize / rows;

        // Draw 1-based cell numbers in each cell
        Color prev = GUI.color;
        GUI.color = new Color(1f, 1f, 0f, 0.85f);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int cellNum = r * cols + c + 1; // 1-based
                GUI.Label(new Rect(atlasRect.x + c * cw + 2, atlasRect.y + r * ch + 2, cw, ch),
                    cellNum.ToString(), _labelStyle);
            }
        }
        GUI.color = prev;

        // Highlight target cell
        int col  = idx % cols;
        int row  = idx / cols;   // top-to-bottom (screen coords)
        Rect cell = new Rect(atlasRect.x + col * cw, atlasRect.y + row * ch, cw, ch);

        // Draw red border
        GUI.color = Color.red;
        GUI.DrawTexture(new Rect(cell.x,                  cell.y,                   cell.width, 2), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(cell.x,                  cell.y + cell.height - 2, cell.width, 2), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(cell.x,                  cell.y,                   2, cell.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(cell.x + cell.width - 2, cell.y,                   2, cell.height), Texture2D.whiteTexture);
        GUI.color = prev;

        // UV info
        float tw     = 1f / cols;
        float th     = 1f / rows;
        float faceAspect = 3f / 2f;
        float scaleX = tw * faceAspect;
        float scaleY = th;
        int uvRow    = rows - 1 - row;
        float offsetX = col * tw + (tw - scaleX) * 0.5f;
        float offsetY = uvRow * th + (th - scaleY) * 0.5f;

        string info =
            $"atlasIndex={idx} (cell #{idx + 1} counting from 1)  col={col} row={row}\n" +
            $"UV offset=({offsetX:F4}, {offsetY:F4})  scale=({scaleX:F4}, {scaleY:F4})\n" +
            $"tex={tex.name} {tex.width}x{tex.height}  grid={cols}x{rows}";

        GUI.Label(new Rect(10, atlasRect.yMax + 5, 520, 60), info);

        if (Application.isPlaying)
        {
            var bm = BoardManager.Instance;
            if (bm != null)
            {
                var mats = bm.GetAtlasMaterials();
                if (mats != null && idx < mats.Length && mats[idx] != null)
                {
                    var m = mats[idx];
                    var o = m.GetTextureOffset("_BaseMap");
                    var s = m.GetTextureScale("_BaseMap");
                    GUI.Label(new Rect(10, atlasRect.yMax + 70, 520, 20),
                        $"material[{idx}] actual offset=({o.x:F4},{o.y:F4}) scale=({s.x:F4},{s.y:F4})");
                }
                else
                {
                    GUI.Label(new Rect(10, atlasRect.yMax + 70, 400, 20), "material array null or index out of range");
                }
            }
        }
    }
}
