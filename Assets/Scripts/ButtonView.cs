using System;
using UnityEngine;

public class ButtonView : MonoBehaviour
{
    public event Action OnClick;

    [SerializeField] private string colorProperty = "_Action_color";
    [SerializeField] private Color normalColor  = Color.white;
    [SerializeField] private Color hoverColor   = new Color(0.85f, 1f, 0.85f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.6f, 0.9f, 0.6f, 1f);

    private Renderer btnRenderer;
    private MaterialPropertyBlock propBlock;

    void Awake()
    {
        btnRenderer = GetComponentInChildren<Renderer>();
        propBlock   = new MaterialPropertyBlock();
        ApplyColor(normalColor);
    }

    void OnMouseEnter() => ApplyColor(hoverColor);
    void OnMouseExit()  => ApplyColor(normalColor);

    void OnMouseDown()  => ApplyColor(pressedColor);

    void OnMouseUp()
    {
        ApplyColor(hoverColor);
        OnClick?.Invoke();
    }

    private void ApplyColor(Color color)
    {
        btnRenderer.GetPropertyBlock(propBlock, 0);
        propBlock.SetColor(colorProperty, color);
        btnRenderer.SetPropertyBlock(propBlock, 0);
    }
}
