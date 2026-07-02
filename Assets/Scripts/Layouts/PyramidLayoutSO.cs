using System.Collections.Generic;
using UnityEngine;

// Step pyramid viewed from above: each layer insets by 1 tile on all sides.
[CreateAssetMenu(fileName = "PyramidLayout", menuName = "Mahjong/Layouts/Pyramid")]
public class PyramidLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — bottom (52 tiles)
        int[,] layer0 = {
            {1,1,1,1,1,1,1,1,1,1}, // y= 6
            {0,1,1,1,1,1,1,1,1,0}, // y= 4
            {0,0,1,1,1,1,1,1,0,0}, // y= 2
            {0,0,0,1,1,1,1,0,0,0}, // y= 0
            {0,0,1,1,1,1,1,1,0,0}, // y=-2
            {0,1,1,1,1,1,1,1,1,0}, // y=-4
            {1,1,1,1,1,1,1,1,1,1}, // y=-6
        };
        int[] y0 = { 6, 4, 2, 0, -2, -4, -6 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — middle (26 tiles)
        int[,] layer1 = {
            {0,0,1,1,1,1,1,1,0,0}, // y= 6
            {0,0,0,1,1,1,1,0,0,0}, // y= 4
            {0,0,0,0,1,1,0,0,0,0}, // y= 2
            {0,0,0,0,1,1,0,0,0,0}, // y= 0
            {0,0,0,0,1,1,0,0,0,0}, // y=-2
            {0,0,0,1,1,1,1,0,0,0}, // y=-4
            {0,0,1,1,1,1,1,1,0,0}, // y=-6
        };
        int[] y1 = { 6, 4, 2, 0, -2, -4, -6 };
        AddLayer(positions, layer1, y1, 1);

        // Layer 2 — top (2 tiles)
        int[,] layer2 = {
            {0,0,0,0,1,1,0,0,0,0}, // y= 0
        };
        int[] y2 = { 0 };
        AddLayer(positions, layer2, y2, 2);

        return positions;  // 52 + 26 + 2 = 80 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
