using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiamondLayout", menuName = "Mahjong/Layouts/Diamond")]
public class DiamondLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — bottom (34 tiles)
        int[,] layer0 = {
            {0,0,0,0,1,1,0,0,0,0}, // y= 6
            {0,0,0,1,1,1,1,0,0,0}, // y= 4
            {0,0,1,1,1,1,1,1,0,0}, // y= 2
            {1,1,1,1,1,1,1,1,1,1}, // y= 0
            {0,0,1,1,1,1,1,1,0,0}, // y=-2
            {0,0,0,1,1,1,1,0,0,0}, // y=-4
            {0,0,0,0,1,1,0,0,0,0}, // y=-6
        };
        int[] y0 = { 6, 4, 2, 0, -2, -4, -6 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — middle (20 tiles)
        int[,] layer1 = {
            {0,0,0,0,1,1,0,0,0,0}, // y= 4
            {0,0,0,1,1,1,1,0,0,0}, // y= 2
            {0,1,1,1,1,1,1,1,1,0}, // y= 0
            {0,0,0,1,1,1,1,0,0,0}, // y=-2
            {0,0,0,0,1,1,0,0,0,0}, // y=-4
        };
        int[] y1 = { 4, 2, 0, -2, -4 };
        AddLayer(positions, layer1, y1, 1);

        // Layer 2 — top (8 tiles)
        int[,] layer2 = {
            {0,0,0,0,1,1,0,0,0,0}, // y= 2
            {0,0,0,1,1,1,1,0,0,0}, // y= 0
            {0,0,0,0,1,1,0,0,0,0}, // y=-2
        };
        int[] y2 = { 2, 0, -2 };
        AddLayer(positions, layer2, y2, 2);

        return positions;  // 34 + 20 + 8 = 62 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
