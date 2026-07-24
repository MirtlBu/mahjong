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
        public string id;       // "hint", "shuffle", "bomb", etc.
        public Button button;
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
    [SerializeField] private Button looseOkButton;
    [SerializeField] private TMP_Text shuffleHintLabel;    // маленькая надпись внизу доски (вне попапа)

    [Header("Message Popup")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageHeader;
    [SerializeField] private UnityEngine.UI.Image messageImage;
    [SerializeField] private Button messageOkButton;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        if (victoryPanel != null)    victoryPanel.SetActive(false);
        if (loosePanel != null)      loosePanel.SetActive(false);
        if (messagePanel != null)    messagePanel.SetActive(false);
        if (shuffleHintLabel != null) shuffleHintLabel.gameObject.SetActive(false);
    }

    void Start()
    {
        foreach (var p in powerUpButtons)
        {
            var captured = p;
            captured.button?.onClick.AddListener(() => OnPowerUpClicked(captured.id));
        }

        backButton?.onClick.AddListener(() => GameManager.Instance?.AbandonLevel());
        Debug.Log($"[HUD] okButton={okButton}, looseOkButton={looseOkButton}");
        okButton?.onClick.AddListener(OnOkClicked);
        looseOkButton?.onClick.AddListener(OnOkClicked);
        messageOkButton?.onClick.AddListener(OnMessageOkClicked);

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
    [SerializeField] private bool showAllPowerUps = false;

    public void RefreshPowerUps()
    {
        if (showAllPowerUps)
            foreach (var p in powerUpButtons)
                if (p.button != null) p.button.gameObject.SetActive(true);
    }

    public void RefreshResourceCounts()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;
        foreach (var p in powerUpButtons)
        {
            if (p.button == null) continue;
            bool unlocked = p.id switch
            {
                "hint"    => gm.HintCount    > 0,
                "shuffle" => gm.ShuffleCount > 0,
                "bomb"    => gm.BombCount    > 0,
                _         => false
            };
            p.button.gameObject.SetActive(unlocked);
        }
    }

    public void ShowNoMoves(bool isGameOver)
    {
        if (isGameOver)
        {
            if (loosePanel != null) loosePanel.SetActive(true);
        }
        else
        {
            if (shuffleHintLabel != null)
            {
                shuffleHintLabel.text = GameStrings.NoMovesUseShuffle;
                shuffleHintLabel.gameObject.SetActive(true);
            }
        }
    }

    public void HideNoMoves()
    {
        if (loosePanel != null)       loosePanel.SetActive(false);
        if (shuffleHintLabel != null) shuffleHintLabel.gameObject.SetActive(false);
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
        Debug.Log($"[OK] clicked, GameManager={GameManager.Instance}");
        GameManager.Instance?.ReturnToMap();
    }

    private System.Action _messageOnClose;

    void OnMessageOkClicked()
    {
        if (messagePanel != null) messagePanel.SetActive(false);
        RefreshResourceCounts();
        var cb = _messageOnClose;
        _messageOnClose = null;
        cb?.Invoke();
    }

    public void ShowMessage(SpecialTileRewardSO reward, System.Action onClose = null)
    {
        if (messagePanel == null) return;
        if (messageHeader != null) messageHeader.text = reward.messageText;
        if (messageImage  != null) messageImage.sprite = reward.icon;
        _messageOnClose = onClose;
        messagePanel.SetActive(true);
    }

    public void ShowVictory(int fromScore, int toScore, int stars)
    {
        int gained = toScore - fromScore;
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
