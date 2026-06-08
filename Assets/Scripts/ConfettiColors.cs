using UnityEngine;

public class ConfettiColors : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettiSystem;
    [SerializeField] private Material colorSource;
    [SerializeField] private string[] colorProperties = { "_Color1", "_Color2", "_Color3" };

    void Awake()
    {
        ApplyColors();
    }

    public void ApplyColors()
    {
        if (colorSource == null || confettiSystem == null) return;

        var keys = new GradientColorKey[colorProperties.Length];
        for (int i = 0; i < colorProperties.Length; i++)
        {
            Color c = colorSource.HasColor(colorProperties[i])
                ? colorSource.GetColor(colorProperties[i])
                : Color.white;
            keys[i] = new GradientColorKey(c, (float)i / Mathf.Max(1, colorProperties.Length - 1));
        }

        var gradient = new Gradient();
        gradient.colorKeys = keys;
        gradient.alphaKeys = new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) };

        var minMaxGradient = new ParticleSystem.MinMaxGradient(gradient);
        minMaxGradient.mode = ParticleSystemGradientMode.RandomColor;

        var main = confettiSystem.main;
        main.startColor = minMaxGradient;
    }
}
