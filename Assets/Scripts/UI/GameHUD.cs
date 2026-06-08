using System.Collections;
using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }

    [Header("3D Buttons")]
    [SerializeField] private GameObject hintButtonObject;
    [SerializeField] private GameObject shuffleButtonObject;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreLabel;

    [Header("Victory")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text victoryScoreLabel;
    [SerializeField] private ParticleSystem[] confettiSystems;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    void Start()
    {
        var hint = hintButtonObject?.GetComponentInChildren<ButtonView>();
        if (hint != null) hint.OnClick += () => BoardManager.Instance?.ShowHint();

        var shuffle = shuffleButtonObject?.GetComponentInChildren<ButtonView>();
        if (shuffle != null) shuffle.OnClick += () => BoardManager.Instance?.Shuffle();
    }

    public void SetScore(int score)
    {
        if (scoreLabel != null)
            scoreLabel.text = score.ToString();
    }

    public void ShowVictory(int fromScore, int toScore)
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        foreach (var ps in confettiSystems)
            if (ps != null) ps.Play();

        StartCoroutine(AnimateScore(fromScore, toScore, 1.5f));
    }

    IEnumerator AnimateScore(int from, int to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            int current = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
            if (victoryScoreLabel != null)
                victoryScoreLabel.text = $"Score: {current}";
            yield return null;
        }
        if (victoryScoreLabel != null)
            victoryScoreLabel.text = $"Score: {to}";
    }
}
