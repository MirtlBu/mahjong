using System.Collections.Generic;
using UnityEngine;

// Five clusters: top-left, top-right, center, bottom-left, bottom-right.
// Upper layer has a diamond on each cluster.
[CreateAssetMenu(fileName = "ClustersLayout", menuName = "Mahjong/Layouts/Clusters")]
public class ClustersLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — five 3×3 clusters (48 tiles)
        // Left block = cols 0-2, Center block = cols 3-6, Right block = cols 7-9
        int[,] layer0 = {
            {1,1,1,0,0,0,0,1,1,1},  // y= 8
            {1,1,1,0,0,0,0,1,1,1},  // y= 6
            {1,1,1,0,0,0,0,1,1,1},  // y= 4
            {0,0,0,1,1,1,1,0,0,0},  // y= 2
            {0,0,0,1,1,1,1,0,0,0},  // y= 0
            {0,0,0,1,1,1,1,0,0,0},  // y=-2
            {1,1,1,0,0,0,0,1,1,1},  // y=-4
            {1,1,1,0,0,0,0,1,1,1},  // y=-6
            {1,1,1,0,0,0,0,1,1,1},  // y=-8
        };
        int[] y0 = { 8, 6, 4, 2, 0, -2, -4, -6, -8 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — diamond on each cluster (22 tiles)
        int[,] layer1 = {
            {0,1,0,0,0,0,0,0,1,0},  // y= 8
            {1,0,1,0,0,0,0,1,0,1},  // y= 6
            {0,1,0,0,0,0,0,0,1,0},  // y= 4
            {0,0,0,0,1,1,0,0,0,0},  // y= 2
            {0,0,0,1,0,0,1,0,0,0},  // y= 0
            {0,0,0,0,1,1,0,0,0,0},  // y=-2
            {0,1,0,0,0,0,0,0,1,0},  // y=-4
            {1,0,1,0,0,0,0,1,0,1},  // y=-6
            {0,1,0,0,0,0,0,0,1,0},  // y=-8
        };
        int[] y1 = { 8, 6, 4, 2, 0, -2, -4, -6, -8 };
        AddLayer(positions, layer1, y1, 1);

        return positions;  // 48 + 22 = 70 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
