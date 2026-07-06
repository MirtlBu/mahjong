using UnityEngine;
using TMPro;

public class MapSceneUI : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] private TMP_Text totalScoreLabel;

    [Header("Map")]
    [SerializeField] private LevelMapUI levelMap;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[MapSceneUI] GameManager.Instance is NULL");
            return;
        }

        levelMap?.Build(GameManager.Instance.Levels);
        levelMap?.Show();

        Refresh();
    }

    void Refresh()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;
        if (totalScoreLabel != null) totalScoreLabel.text = gm.TotalScore.ToString();
    }
}
