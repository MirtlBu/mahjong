using UnityEngine;

// Тип масти плитки
public enum TileSuit
{
    Characters, // Иероглифы (1-9)
    Bamboo,     // Бамбук (1-9)
    Circles,    // Круги (1-9)
    Winds,      // Ветра (4 штуки)
    Dragons     // Драконы (3 штуки)
}

[System.Serializable]
public class TileData
{
    public TileSuit suit;
    public int value; // 1-9 для Characters/Bamboo/Circles, 1-4 для Winds, 1-3 для Dragons

    public TileData(TileSuit suit, int value)
    {
        this.suit = suit;
        this.value = value;
    }

    // Две плитки совпадают если масть и значение одинаковые
    public bool Matches(TileData other)
    {
        return suit == other.suit && value == other.value;
    }
}
