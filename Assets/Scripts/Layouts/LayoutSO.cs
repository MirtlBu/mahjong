using System.Collections.Generic;
using UnityEngine;

public abstract class LayoutSO : ScriptableObject
{
    [Header("Level Info")]
    public string levelName = "Level";
    public Sprite previewSprite;
    public Vector2 mapPosition;

    public abstract List<Vector3Int> GetPositions();
}
