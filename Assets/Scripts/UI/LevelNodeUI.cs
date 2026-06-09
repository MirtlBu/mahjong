using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelNodeUI : MonoBehaviour
{
    [SerializeField] public int levelIndex;
    [SerializeField] private TMP_Text levelNameLabel;
    [SerializeField] private Image previewImage;
    [SerializeField] private GameObject lockOverlay;
    [SerializeField] private GameObject completedMark;
    [SerializeField] private Button button;

    private LevelConfig config;

    public void Setup(LevelConfig cfg, int index)
    {
        config = cfg;
        levelIndex = index;

        if (levelNameLabel != null)
            levelNameLabel.text = cfg.levelName;

        if (previewImage != null && cfg.previewSprite != null)
            previewImage.sprite = cfg.previewSprite;

        Refresh();

        button?.onClick.AddListener(() => GameManager.Instance?.StartLevel(index));
    }

    public void Refresh()
    {
        bool unlocked = LevelProgress.IsUnlocked(levelIndex);
        bool completed = LevelProgress.IsCompleted(levelIndex);

        if (lockOverlay != null) lockOverlay.SetActive(!unlocked);
        if (completedMark != null) completedMark.SetActive(completed);
        if (button != null) button.interactable = unlocked;
    }
}
