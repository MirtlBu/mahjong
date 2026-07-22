using UnityEngine;

[CreateAssetMenu(fileName = "SpecialTileReward", menuName = "Mahjong/Special Tile Reward")]
public class SpecialTileRewardSO : ScriptableObject
{
    [Header("Reward")]
    public PowerUpType powerUpType;

    [Header("Message")]
    [TextArea] public string messageText;
    public Sprite icon;

    [Header("Visual")]
    [Tooltip("Ячейка в атласе 6×6 (0–35, слева-направо, сверху-вниз). Укажи ячейку с картинкой этого тайла.")]
    public int atlasIndex = 0;
}
