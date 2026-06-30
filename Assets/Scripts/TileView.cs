using System.Collections;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [HideInInspector] public TileData data;
    [HideInInspector] public int boardX, boardY, boardLayer;

    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem pulsationParticles;

    private Renderer tileRenderer;
    private MaterialPropertyBlock propBlock;
    private MaterialPropertyBlock facePropBlock;

    [SerializeField] private Color blockedTint    = new Color(0.5f, 0.5f, 0.7f, 1f);
    [SerializeField] private Color selectedTint   = new Color(0.3f, 0.8f, 0.2f, 1f);

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
        tileRenderer.GetPropertyBlock(propBlock, 0);

        Color bodyColor;
        if (IsSelected)
            bodyColor = Color.Lerp(baseColor, selectedTint, 0.55f);
        else if (!isFree)
            bodyColor = Color.Lerp(baseColor, blockedTint, 0.5f);
        else
            bodyColor = baseColor;

        bodyColor.a = 1f;
        propBlock.SetColor("_BaseColor", bodyColor);
        tileRenderer.SetPropertyBlock(propBlock, 0);

        if (tileRenderer.sharedMaterials.Length > 1)
        {
            tileRenderer.GetPropertyBlock(facePropBlock, 1);
            Color faceColor;
            if (IsSelected)
                faceColor = Color.Lerp(Color.white, selectedTint, 0.3f);
            else if (!isFree)
                faceColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            else
                faceColor = Color.white;
            facePropBlock.SetColor("_BaseColor", faceColor);
            tileRenderer.SetPropertyBlock(facePropBlock, 1);
        }
    }

    public void Blink(int times = 2)
    {
        if (pulsationParticles != null)
            pulsationParticles.Play();
    }

    public void StopBlink()
    {
        if (pulsationParticles != null)
            pulsationParticles.Stop();
    }

    public void PlayDeathEffect()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        tileRenderer.enabled = false;
        if (deathParticles != null)
        {
            deathParticles.Play();
            yield return new WaitUntil(() => !deathParticles.IsAlive(true));
        }
        Destroy(gameObject);
    }

    void OnMouseUp()
    {
        BoardManager.Instance.OnTileClicked(this);
    }
}
