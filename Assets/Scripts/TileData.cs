using UnityEngine;

public enum TileSuit
{
    Characters, // 1-9
    Bamboo,     // 1-9
    Circles,    // 1-9
    Winds,      // 1-4
    Dragons     // 1-3
}

[System.Serializable]
public class TileData
{
    public TileSuit suit;
    public int value; // 1-9 for Characters/Bamboo/Circles, 1-4 for Winds, 1-3 for Dragons

    public TileData(TileSuit suit, int value)
    {
        this.suit = suit;
        this.value = value;
    }

    public bool Matches(TileData other)
    {
        return suit == other.suit && value == other.value;
    }
}
