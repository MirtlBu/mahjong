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

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
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
}
