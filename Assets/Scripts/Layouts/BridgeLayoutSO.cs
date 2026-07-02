using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BridgeLayout", menuName = "Mahjong/Layouts/Bridge")]
public class BridgeLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — two towers connected by a bridge (46 tiles)
        int[,] layer0 = {
            {1,1,0,0,0,0,0,0,1,1},  // y= 6
            {1,1,0,0,0,0,0,0,1,1},  // y= 4
            {1,1,1,1,1,1,1,1,1,1},  // y= 2
            {1,1,1,1,1,1,1,1,1,1},  // y= 0
            {1,1,1,1,1,1,1,1,1,1},  // y=-2
            {1,1,0,0,0,0,0,0,1,1},  // y=-4
            {1,1,0,0,0,0,0,0,1,1},  // y=-6
        };
        int[] y0 = { 6, 4, 2, 0, -2, -4, -6 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — tower columns and bridge rows (24 tiles)
        int[,] layer1 = {
            {0,1,0,0,0,0,0,0,1,0},  // y= 6
            {0,1,0,0,0,0,0,0,1,0},  // y= 4
            {0,1,1,1,1,1,1,1,1,0},  // y= 2
            {0,1,1,1,1,1,1,1,1,0},  // y=-2
            {0,1,0,0,0,0,0,0,1,0},  // y=-4
            {0,1,0,0,0,0,0,0,1,0},  // y=-6
        };
        int[] y1 = { 6, 4, 2, -2, -4, -6 };
        AddLayer(positions, layer1, y1, 1);

        return positions;  // 46 + 24 = 70 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
