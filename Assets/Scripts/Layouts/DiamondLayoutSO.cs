using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiamondLayout", menuName = "Mahjong/Layouts/Diamond")]
public class DiamondLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — diamond base (60 tiles)
        int[,] layer0 = {
            {0,0,0,0,1,1,0,0,0,0},  // y= 9
            {0,0,0,1,1,1,1,0,0,0},  // y= 7
            {0,0,1,1,1,1,1,1,0,0},  // y= 5
            {0,1,1,1,1,1,1,1,1,0},  // y= 3
            {1,1,1,1,1,1,1,1,1,1},  // y= 1
            {1,1,1,1,1,1,1,1,1,1},  // y=-1
            {0,1,1,1,1,1,1,1,1,0},  // y=-3
            {0,0,1,1,1,1,1,1,0,0},  // y=-5
            {0,0,0,1,1,1,1,0,0,0},  // y=-7
            {0,0,0,0,1,1,0,0,0,0},  // y=-9
        };
        int[] y0 = { 9, 7, 5, 3, 1, -1, -3, -5, -7, -9 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — smaller diamond (24 tiles)
        int[,] layer1 = {
            {0,0,0,0,1,1,0,0,0,0},  // y= 5
            {0,0,0,1,1,1,1,0,0,0},  // y= 3
            {0,0,1,1,1,1,1,1,0,0},  // y= 1
            {0,0,1,1,1,1,1,1,0,0},  // y=-1
            {0,0,0,1,1,1,1,0,0,0},  // y=-3
            {0,0,0,0,1,1,0,0,0,0},  // y=-5
        };
        int[] y1 = { 5, 3, 1, -1, -3, -5 };
        AddLayer(positions, layer1, y1, 1);

        // Layer 2 — peak (8 tiles)
        int[,] layer2 = {
            {0,0,0,1,1,1,1,0,0,0},  // y= 1
            {0,0,0,1,1,1,1,0,0,0},  // y=-1
        };
        int[] y2 = { 1, -1 };
        AddLayer(positions, layer2, y2, 2);

        return positions;  // 60 + 24 + 8 = 92 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
