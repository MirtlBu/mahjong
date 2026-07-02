using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EightLayout", menuName = "Mahjong/Layouts/Eight")]
public class EightLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — bottom (40 tiles)
        int[,] layer0 = {
            {0,0,1,1,1,1,1,1,0,0}, // y= 7
            {0,1,0,0,0,0,0,0,1,0}, // y= 5
            {0,1,0,0,0,0,0,0,1,0}, // y= 3
            {0,0,1,1,1,1,1,1,0,0}, // y= 1
            {0,1,1,1,1,1,1,1,1,0}, // y=-1
            {1,1,0,0,0,0,0,0,1,1}, // y=-3
            {1,1,0,0,0,0,0,0,1,1}, // y=-5
            {0,1,1,1,1,1,1,1,1,0}, // y=-7
        };
        int[] y0 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — middle (18 tiles)
        int[,] layer1 = {
            {0,0,0,0,1,1,0,0,0,0}, // y= 7
            {0,1,0,0,0,0,0,0,1,0}, // y= 5
            {0,1,0,0,0,0,0,0,1,0}, // y= 3
            {0,0,0,0,1,1,0,0,0,0}, // y= 1
            {0,0,0,1,1,1,1,0,0,0}, // y=-1
            {0,1,0,0,0,0,0,0,1,0}, // y=-3
            {0,1,0,0,0,0,0,0,1,0}, // y=-5
            {0,0,0,0,1,1,0,0,0,0}, // y=-7
        };
        int[] y1 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        AddLayer(positions, layer1, y1, 1);

        // Layer 2 — top (6 tiles)
        int[,] layer2 = {
            {0,0,0,0,1,1,0,0,0,0}, // y= 7
            {0,0,0,0,1,1,0,0,0,0}, // y=-1
            {0,0,0,0,1,1,0,0,0,0}, // y=-7
        };
        int[] y2 = { 7, -1, -7 };
        AddLayer(positions, layer2, y2, 2);

        return positions;  // 40 + 18 + 6 = 64 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
