using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurtleLayout", menuName = "Mahjong/Layouts/Turtle")]
public class TurtleLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — base body, wide middle (72 tiles)
        int[,] layer0 = {
            {0,1,1,1,1,1,1,1,1,0},  // y= 7
            {0,1,1,1,1,1,1,1,1,0},  // y= 5
            {1,1,1,1,1,1,1,1,1,1},  // y= 3
            {1,1,1,1,1,1,1,1,1,1},  // y= 1
            {1,1,1,1,1,1,1,1,1,1},  // y=-1
            {1,1,1,1,1,1,1,1,1,1},  // y=-3
            {0,1,1,1,1,1,1,1,1,0},  // y=-5
            {0,1,1,1,1,1,1,1,1,0},  // y=-7
        };
        int[] y0 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — 6 wide (36 tiles)
        int[,] layer1 = {
            {0,0,1,1,1,1,1,1,0,0},  // y= 5
            {0,0,1,1,1,1,1,1,0,0},  // y= 3
            {0,0,1,1,1,1,1,1,0,0},  // y= 1
            {0,0,1,1,1,1,1,1,0,0},  // y=-1
            {0,0,1,1,1,1,1,1,0,0},  // y=-3
            {0,0,1,1,1,1,1,1,0,0},  // y=-5
        };
        int[] y1 = { 5, 3, 1, -1, -3, -5 };
        AddLayer(positions, layer1, y1, 1);

        // Layer 2 — 4 wide (16 tiles)
        int[,] layer2 = {
            {0,0,0,1,1,1,1,0,0,0},  // y= 3
            {0,0,0,1,1,1,1,0,0,0},  // y= 1
            {0,0,0,1,1,1,1,0,0,0},  // y=-1
            {0,0,0,1,1,1,1,0,0,0},  // y=-3
        };
        int[] y2 = { 3, 1, -1, -3 };
        AddLayer(positions, layer2, y2, 2);

        // Layer 3 — peak 2 wide (4 tiles)
        int[,] layer3 = {
            {0,0,0,0,1,1,0,0,0,0},  // y= 1
            {0,0,0,0,1,1,0,0,0,0},  // y=-1
        };
        int[] y3 = { 1, -1 };
        AddLayer(positions, layer3, y3, 3);

        return positions;  // 72 + 36 + 16 + 4 = 128 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
