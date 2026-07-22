using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelNodeUI : MonoBehaviour
{
    [SerializeField] public int levelIndex;
    [SerializeField] private TMP_Text levelNameLabel;
    [SerializeField] private Image previewImage;
    [SerializeField] private GameObject lockOverlay;
    [SerializeField] private Button button;
    [SerializeField] private StarsDisplay starsDisplay;

    private LayoutSO config;

    public void Setup(LayoutSO cfg, int index)
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

        if (lockOverlay != null) lockOverlay.SetActive(!unlocked);
        if (button != null) button.interactable = unlocked;
        if (levelNameLabel != null) levelNameLabel.gameObject.SetActive(unlocked);

        if (starsDisplay != null)
        {
            if (unlocked)
                starsDisplay.Show(LevelProgress.GetStars(levelIndex));
            else
                starsDisplay.Hide();
        }
    }
}
