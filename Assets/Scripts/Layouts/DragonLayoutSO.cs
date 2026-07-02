using System.Collections.Generic;
using UnityEngine;

// Dragon viewed from above: wide wings in the middle,
// layers build up toward the right (head side).
[CreateAssetMenu(fileName = "DragonLayout", menuName = "Mahjong/Layouts/Dragon")]
public class DragonLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — bottom (33 tiles)
        int[,] layer0 = {
            {1,1,1,0,0,0,0,0,0,0}, // y= 6
            {0,0,1,1,1,0,0,0,0,0}, // y= 4
            {0,0,0,1,1,1,1,1,1,1}, // y= 2
            {0,0,1,1,1,1,1,1,1,1}, // y= 0
            {1,0,0,0,0,1,1,1,0,0}, // y=-2
            {0,1,1,0,0,1,1,0,0,0}, // y=-4
            {0,0,1,1,1,1,0,0,0,0}, // y=-6
        };
        int[] y0 = { 6, 4, 2, 0, -2, -4, -6 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — middle (16 tiles)
        int[,] layer1 = {
            {0,1,0,0,0,0,0,0,0,0}, // y= 6
            {0,0,0,1,1,0,0,0,0,0}, // y= 4
            {0,0,0,0,1,1,1,0,0,0}, // y= 2
            {0,0,0,1,1,1,1,1,0,0}, // y= 0
            {0,0,0,0,0,0,1,0,0,0}, // y=-2
            {0,0,1,0,0,1,0,0,0,0}, // y=-4
            {0,0,0,1,1,0,0,0,0,0}, // y=-6
        };
        int[] y1 = { 6, 4, 2, 0, -2, -4, -6 };
        AddLayer(positions, layer1, y1, 1);

        // Layer 2 — top (5 tiles)
        int[,] layer2 = {
            {0,1,0,0,0,0,0,0,0,0}, // y= 6
            {0,0,0,0,0,1,0,0,0,0}, // y= 2
            {0,0,0,0,1,0,1,0,0,0}, // y= 0
            {0,0,0,0,1,0,0,0,0,0}, // y=-6
        };
        int[] y2 = { 6, 2, 0, -6 };
        AddLayer(positions, layer2, y2, 2);

        return positions;  // 33 + 16 + 5 = 54 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
