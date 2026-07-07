using System;
using UnityEngine;

public class ButtonView : MonoBehaviour
{
    public event Action OnClick;

    [SerializeField] private string colorProperty = "_Action_color";
    [SerializeField] private Color normalColor  = Color.white;
    [SerializeField] private Color hoverColor   = new Color(0.85f, 1f, 0.85f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.6f, 0.9f, 0.6f, 1f);

    [SerializeField] private Renderer targetRenderer;

    private Renderer btnRenderer;
    private MaterialPropertyBlock propBlock;
    private Vector3 originalScale;

    void Awake()
    {
        btnRenderer   = targetRenderer != null ? targetRenderer : GetComponentInChildren<Renderer>();
        propBlock     = new MaterialPropertyBlock();
        originalScale = transform.localScale;
        ApplyColor(normalColor);
    }

    void OnMouseEnter() => ApplyColor(hoverColor);
    void OnMouseExit()
    {
        ApplyColor(normalColor);
        transform.localScale = originalScale;
    }

    void OnMouseDown()
    {
        ApplyColor(pressedColor);
        transform.localScale = originalScale * 1.05f;
    }

    void OnMouseUp()
    {
        ApplyColor(hoverColor);
        transform.localScale = originalScale;
        OnClick?.Invoke();
    }

    private void ApplyColor(Color color)
    {
        btnRenderer.GetPropertyBlock(propBlock, 0);
        propBlock.SetColor(colorProperty, color);
        btnRenderer.SetPropertyBlock(propBlock, 0);
    }
}
