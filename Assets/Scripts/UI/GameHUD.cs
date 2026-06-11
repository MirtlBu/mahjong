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

    [Header("Resource Counts")]
    [SerializeField] private TMP_Text hintCountLabel;
    [SerializeField] private TMP_Text shuffleCountLabel;

    [Header("No Moves")]
    [SerializeField] private TMP_Text noMovesLabel;

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
        if (noMovesLabel != null)
            noMovesLabel.gameObject.SetActive(false);
    }

    void Start()
    {
        var hint = hintButtonObject?.GetComponentInChildren<ButtonView>();
        if (hint != null) hint.OnClick += () => BoardManager.Instance?.ShowHint();

        var shuffle = shuffleButtonObject?.GetComponentInChildren<ButtonView>();
        if (shuffle != null) shuffle.OnClick += () => BoardManager.Instance?.Shuffle();

        RefreshResourceCounts();
    }

    public void RefreshResourceCounts()
    {
        var gm = GameManager.Instance;
        if (hintCountLabel    != null) hintCountLabel.text    = gm != null ? $"{gm.HintCount}"    : "";
        if (shuffleCountLabel != null) shuffleCountLabel.text = gm != null ? $"{gm.ShuffleCount}" : "";
    }

    public void ShowNoMoves(bool isGameOver)
    {
        if (noMovesLabel == null) return;
        var canvas = noMovesLabel.GetComponentInParent<Canvas>(true);
        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
            foreach (var t in canvas.GetComponentsInChildren<TMP_Text>(true))
                t.gameObject.SetActive(t == noMovesLabel);
        }
        noMovesLabel.gameObject.SetActive(true);
        noMovesLabel.text = isGameOver ? GameStrings.GameOver : GameStrings.NoMovesUseShuffle;
    }

    public void HideNoMoves()
    {
        if (noMovesLabel != null) noMovesLabel.gameObject.SetActive(false);
    }

    public void SetScore(int score)
    {
        if (scoreLabel != null)
            scoreLabel.text = score.ToString();
    }

    public void ResetForNewLevel()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
        if (scoreLabel != null)
            scoreLabel.text = "0";
    }

    public void HideVictory()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void ShowVictory(int fromScore, int toScore)
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            foreach (var t in victoryPanel.GetComponentsInChildren<TMP_Text>(true))
                t.gameObject.SetActive(t != noMovesLabel);
        }
        if (noMovesLabel != null)
            noMovesLabel.gameObject.SetActive(false);

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
                victoryScoreLabel.text = string.Format(GameStrings.ScoreFormat, current);
            yield return null;
        }
        if (victoryScoreLabel != null)
            victoryScoreLabel.text = string.Format(GameStrings.ScoreFormat, to);
    }
}
