using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }

    [Header("3D Buttons")]
    [SerializeField] private GameObject hintButtonObject;
    [SerializeField] private GameObject shuffleButtonObject;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreLabel;

[Header("No Moves")]
    [SerializeField] private TMP_Text noMovesLabel;

    [Header("Navigation")]
    [SerializeField] private Button backButton;

    [Header("Victory")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text victoryStarsLabel;
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

        backButton?.onClick.AddListener(() =>
        {
            Debug.Log("Back button clicked — abandoning level");
            GameManager.Instance?.AbandonLevel();
        });

        RefreshResourceCounts();
    }

    public void RefreshResourceCounts() { }

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
        var gm = GameManager.Instance;
        if (scoreLabel != null)
            scoreLabel.text = gm != null ? gm.TotalScore.ToString() : "0";
    }

    public void HideVictory()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void ShowVictory(int fromScore, int toScore, int stars)
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            foreach (var t in victoryPanel.GetComponentsInChildren<TMP_Text>(true))
                t.gameObject.SetActive(t != noMovesLabel);
        }
        if (noMovesLabel != null)
            noMovesLabel.gameObject.SetActive(false);

        if (victoryStarsLabel != null)
            victoryStarsLabel.text = StarsToString(stars);

        foreach (var ps in confettiSystems)
            if (ps != null) ps.Play();
    }

    static string StarsToString(int stars) => stars switch
    {
        3 => "\u2605\u2605\u2605",
        2 => "\u2605\u2605\u2606",
        _ => "\u2605\u2606\u2606",
    };
}
