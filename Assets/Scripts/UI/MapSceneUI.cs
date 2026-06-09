using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapSceneUI : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] private TMP_Text totalScoreLabel;
    [SerializeField] private TMP_Text hintCountLabel;
    [SerializeField] private TMP_Text shuffleCountLabel;

    [Header("Shop")]
    [SerializeField] private Button buyHintButton;
    [SerializeField] private Button buyShuffleButton;

    [Header("Map")]
    [SerializeField] private LevelMapUI levelMap;

    void Start()
    {
        if (GameManager.Instance == null) return;

        levelMap?.Build(GameManager.Instance.Levels);
        levelMap?.Show(GameManager.Instance.CurrentLevelIndex);

        buyHintButton?.onClick.AddListener(OnBuyHint);
        buyShuffleButton?.onClick.AddListener(OnBuyShuffle);

        Refresh();
    }

    void Refresh()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        if (totalScoreLabel   != null) totalScoreLabel.text   = gm.TotalScore.ToString();
        if (hintCountLabel    != null) hintCountLabel.text    = gm.HintCount.ToString();
        if (shuffleCountLabel != null) shuffleCountLabel.text = gm.ShuffleCount.ToString();

        if (buyHintButton    != null) buyHintButton.interactable    = gm.TotalScore >= gm.hintCost;
        if (buyShuffleButton != null) buyShuffleButton.interactable = gm.TotalScore >= gm.shuffleCost;
    }

    void OnBuyHint()
    {
        if (GameManager.Instance.BuyHint()) Refresh();
    }

    void OnBuyShuffle()
    {
        if (GameManager.Instance.BuyShuffle()) Refresh();
    }
}
