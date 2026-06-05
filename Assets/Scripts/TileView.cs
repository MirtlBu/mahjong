using UnityEngine;

public class TileView : MonoBehaviour
{
    [HideInInspector] public TileData data;
    [HideInInspector] public int boardX, boardY, boardLayer;

    private Renderer tileRenderer;
    private MaterialPropertyBlock propBlock;
    private MaterialPropertyBlock facePropBlock;

    private Color baseColor = Color.white;
    private bool isFree = true;

    public bool IsSelected { get; private set; }

    void Awake()
    {
        tileRenderer  = GetComponentInChildren<Renderer>();
        propBlock     = new MaterialPropertyBlock();
        facePropBlock = new MaterialPropertyBlock();
    }

    public void SetData(TileData tileData)
    {
        data = tileData;
    }

    public void SetBaseColor(Color color)
    {
        baseColor = color;
        ApplyVisualState();
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
        transform.localScale = selected ? Vector3.one * 1.05f : Vector3.one;
        ApplyVisualState();
    }

    public void SetFree(bool free)
    {
        isFree = free;
        ApplyVisualState();
    }

    void ApplyVisualState()
    {
        // Тело тайла (материал 0) — цветовой тинт для выделения/блокировки
        tileRenderer.GetPropertyBlock(propBlock, 0);

        Color bodyColor;
        if (IsSelected)
            bodyColor = Color.Lerp(baseColor, new Color(1f, 0.85f, 0f), 0.55f);
        else if (!isFree)
            bodyColor = baseColor * 0.45f;
        else
            bodyColor = baseColor;

        bodyColor.a = 1f;
        propBlock.SetColor("_BaseColor", bodyColor);
        tileRenderer.SetPropertyBlock(propBlock, 0);

        // Лицевая часть (материал 1) — только затемняем когда заблокирован, иконка всегда читаема
        if (tileRenderer.sharedMaterials.Length > 1)
        {
            tileRenderer.GetPropertyBlock(facePropBlock, 1);
            Color faceColor = isFree ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);
            facePropBlock.SetColor("_BaseColor", faceColor);
            tileRenderer.SetPropertyBlock(facePropBlock, 1);
        }
    }

    void OnMouseUp()
    {
        BoardManager.Instance.OnTileClicked(this);
    }
}
