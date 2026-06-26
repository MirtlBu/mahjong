using System.Collections.Generic;
using UnityEngine;

// Step pyramid viewed from above: each layer insets by 1 tile on all sides.
[CreateAssetMenu(fileName = "PyramidLayout", menuName = "Mahjong/Layouts/Pyramid")]
public class PyramidLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — 10x8 base (80 tiles)
        int[,] layer0 = {
            {1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1},
        };
        int[] y0 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — 8x6 (48 tiles)
        int[,] layer1 = {
            {0,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,0},
        };
        int[] y1 = { 5, 3, 1, -1, -3, -5 };
        AddLayer(positions, layer1, y1, 1);

        // Layer 2 — 6x4 (24 tiles)
        int[,] layer2 = {
            {0,0,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,0,0},
        };
        int[] y2 = { 3, 1, -1, -3 };
        AddLayer(positions, layer2, y2, 2);

        // Layer 3 — 4x2 peak (8 tiles)
        int[,] layer3 = {
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
        };
        int[] y3 = { 1, -1 };
        AddLayer(positions, layer3, y3, 3);

        return positions;  // 80 + 48 + 24 + 8 = 160 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
