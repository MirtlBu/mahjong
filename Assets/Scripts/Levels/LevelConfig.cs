using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Mahjong/Level Config")]
public class LevelConfig : ScriptableObject
{
    public string levelName = "Level 1";
    public Sprite previewSprite;
    public Vector2 mapPosition;
}
