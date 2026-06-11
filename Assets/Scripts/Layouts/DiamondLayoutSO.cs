using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiamondLayout", menuName = "Mahjong/Layouts/Diamond")]
public class DiamondLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — diamond base (72 tiles)
        int[,] layer0 = {
            {0,0,0,0,0,1,1,0,0,0,0,0},  // y= 10
            {0,0,0,0,1,1,1,1,0,0,0,0},  // y=  8
            {0,0,0,1,1,1,1,1,1,0,0,0},  // y=  6
            {0,0,1,1,1,1,1,1,1,1,0,0},  // y=  4
            {0,1,1,1,1,1,1,1,1,1,1,0},  // y=  2
            {1,1,1,1,1,1,1,1,1,1,1,1},  // y=  0
            {0,1,1,1,1,1,1,1,1,1,1,0},  // y= -2
            {0,0,1,1,1,1,1,1,1,1,0,0},  // y= -4
            {0,0,0,1,1,1,1,1,1,0,0,0},  // y= -6
            {0,0,0,0,1,1,1,1,0,0,0,0},  // y= -8
            {0,0,0,0,0,1,1,0,0,0,0,0},  // y=-10
        };
        int[] yValues0 = { 10, 8, 6, 4, 2, 0, -2, -4, -6, -8, -10 };
        AddLayer(positions, layer0, yValues0, 0);

        // Layer 1 — smaller diamond (32 tiles)
        int[,] layer1 = {
            {0,0,0,0,0,1,1,0,0,0,0,0},  // y=  6
            {0,0,0,0,1,1,1,1,0,0,0,0},  // y=  4
            {0,0,0,1,1,1,1,1,1,0,0,0},  // y=  2
            {0,0,1,1,1,1,1,1,1,1,0,0},  // y=  0
            {0,0,0,1,1,1,1,1,1,0,0,0},  // y= -2
            {0,0,0,0,1,1,1,1,0,0,0,0},  // y= -4
            {0,0,0,0,0,1,1,0,0,0,0,0},  // y= -6
        };
        int[] yValues1 = { 6, 4, 2, 0, -2, -4, -6 };
        AddLayer(positions, layer1, yValues1, 1);

        // Layer 2 — peak (8 tiles)
        int[,] layer2 = {
            {0,0,0,0,0,1,1,0,0,0,0,0},  // y=  2
            {0,0,0,0,1,1,1,1,0,0,0,0},  // y=  0
            {0,0,0,0,0,1,1,0,0,0,0,0},  // y= -2
        };
        int[] yValues2 = { 2, 0, -2 };
        AddLayer(positions, layer2, yValues2, 2);

        return positions;  // 72 + 32 + 8 = 112 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 11, yValues[row], z));
    }
}
