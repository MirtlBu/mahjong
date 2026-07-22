using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }

    [Serializable]
    public class PowerUpButton
    {
        public string id;            // "hint", "shuffle", "bomb", etc.
        public Button button;
        public int unlockAtLevel;    // 0 = доступно с первого уровня
    }

    [Header("Power-Ups")]
    [SerializeField] private PowerUpButton[] powerUpButtons;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreLabel;

    [Header("Navigation")]
    [SerializeField] private Button backButton;

    [Header("Victory")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private StarsDisplay victoryStars;
    [SerializeField] private ParticleSystem[] confettiSystems;
    [SerializeField] private Button okButton;

    [Header("Defeat")]
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private TMP_Text noMovesLabel;
    [SerializeField] private Button looseOkButton;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (loosePanel != null)   loosePanel.SetActive(false);
    }

    void Start()
    {
        foreach (var p in powerUpButtons)
        {
            var captured = p;
            captured.button?.onClick.AddListener(() => OnPowerUpClicked(captured.id));
        }

        backButton?.onClick.AddListener(() => GameManager.Instance?.AbandonLevel());
        okButton?.onClick.AddListener(OnOkClicked);
        looseOkButton?.onClick.AddListener(OnOkClicked);

        RefreshPowerUps();
        RefreshResourceCounts();
    }

    void OnPowerUpClicked(string id)
    {
        switch (id)
        {
            case "hint":    BoardManager.Instance?.ShowHint(); break;
            case "shuffle": BoardManager.Instance?.Shuffle();  break;
            case "bomb":    BoardManager.Instance?.UseBomb();  break;
        }
    }

    [Header("Debug")]
    [SerializeField] private bool showAllPowerUps = true; // TODO: убрать после настройки

    public void RefreshPowerUps()
    {
        int levelIndex = GameManager.Instance != null ? GameManager.Instance.CurrentLevelIndex : 0;
        foreach (var p in powerUpButtons)
            if (p.button != null)
                p.button.gameObject.SetActive(showAllPowerUps || levelIndex >= p.unlockAtLevel);
    }

    public void RefreshResourceCounts() { }

    public void ShowNoMoves(bool isGameOver)
    {
        if (noMovesLabel != null)
            noMovesLabel.text = isGameOver ? GameStrings.GameOver : GameStrings.NoMovesUseShuffle;
        if (loosePanel != null)
            loosePanel.SetActive(true);
    }

    public void HideNoMoves()
    {
        if (loosePanel != null) loosePanel.SetActive(false);
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

    void OnOkClicked()
    {
        GameManager.Instance?.ReturnToMap();
    }

    public void ShowVictory(int fromScore, int toScore, int stars)
    {
        int gained = toScore - fromScore;
        Debug.Log($"[PlusScore] from={fromScore} to={toScore} gained={gained} instance={PlusScoreLabel.Instance}");
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            PlusScoreLabel.Instance?.Show(gained);
        }

        victoryStars?.Show(stars);

        foreach (var ps in confettiSystems)
            if (ps != null) ps.Play();
    }
}
