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
        Debug.Log($"[MapSceneUI] Start — GameManager.Instance={GameManager.Instance}");
        Debug.Log($"[MapSceneUI] buyHintButton={buyHintButton}, buyShuffleButton={buyShuffleButton}");

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[MapSceneUI] GameManager.Instance is NULL — listeners not added!");
            return;
        }

        levelMap?.Build(GameManager.Instance.Levels);
        levelMap?.Show(GameManager.Instance.CurrentLevelIndex);

        buyHintButton?.onClick.AddListener(OnBuyHint);
        buyShuffleButton?.onClick.AddListener(OnBuyShuffle);

        Debug.Log($"[MapSceneUI] TotalScore={GameManager.Instance.TotalScore}, hintCost={GameManager.Instance.hintCost}");

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
        Debug.Log($"[MapSceneUI] OnBuyHint — score={GameManager.Instance?.TotalScore}, cost={GameManager.Instance?.hintCost}");
        if (GameManager.Instance.BuyHint()) Refresh();
        else Debug.LogWarning("[MapSceneUI] BuyHint failed (not enough score?)");
    }

    void OnBuyShuffle()
    {
        Debug.Log($"[MapSceneUI] OnBuyShuffle — score={GameManager.Instance?.TotalScore}, cost={GameManager.Instance?.shuffleCost}");
        if (GameManager.Instance.BuyShuffle()) Refresh();
        else Debug.LogWarning("[MapSceneUI] BuyShuffle failed (not enough score?)");
    }
}
