using System.Collections.Generic;
using UnityEngine;

public abstract class LayoutSO : ScriptableObject
{
    [Header("Level Info")]
    public string levelName = "Level";
    public Sprite previewSprite;
    public Vector2 mapPosition;

    [Header("Difficulty")]
    [Tooltip("Сколько уникальных типов тайлов использовать. 0 = все (34). Меньше = легче найти пары.")]
    public int maxTileTypes = 0;

    [Header("Special Tiles")]
    [Tooltip("Каждый элемент добавляет пару специальных тайлов на доску. При матче выдаётся награда.")]
    public SpecialTileRewardSO[] specialTileRewards;

    public abstract List<Vector3Int> GetPositions();
}
