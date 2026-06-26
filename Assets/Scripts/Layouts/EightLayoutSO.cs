using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EightLayout", menuName = "Mahjong/Layouts/Eight")]
public class EightLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — figure-eight, two hollow loops (44 tiles)
        int[,] layer0 = {
            {0,1,1,1,1,1,1,1,0},  // y= 8
            {1,1,0,0,0,0,0,1,1},  // y= 6
            {1,1,0,0,0,0,0,1,1},  // y= 4
            {0,1,1,1,1,1,1,1,0},  // y= 2
            {0,1,1,1,1,1,1,1,0},  // y= 0
            {1,1,0,0,0,0,0,1,1},  // y=-2
            {1,1,0,0,0,0,0,1,1},  // y=-4
            {0,1,1,1,1,1,1,1,0},  // y=-6
            {0,0,0,0,0,0,0,0,0},  // y=-8
        };
        int[] y0 = { 8, 6, 4, 2, 0, -2, -4, -6, -8 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — arc highlights (16 tiles)
        int[,] layer1 = {
            {0,0,1,1,1,0,0,0,0},  // y= 8
            {0,1,0,0,0,1,0,0,0},  // y= 6
            {0,0,0,0,0,0,0,0,0},  // y= 4
            {0,0,1,1,1,0,0,0,0},  // y= 2
            {0,0,1,1,1,0,0,0,0},  // y= 0
            {0,0,0,0,0,0,0,0,0},  // y=-2
            {0,1,0,0,0,1,0,0,0},  // y=-4
            {0,0,1,1,1,0,0,0,0},  // y=-6
            {0,0,0,0,0,0,0,0,0},  // y=-8
        };
        int[] y1 = { 8, 6, 4, 2, 0, -2, -4, -6, -8 };
        AddLayer(positions, layer1, y1, 1);

        return positions;  // 44 + 16 = 60 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 8, yValues[row], z));
    }
}
