using System.Collections.Generic;
using UnityEngine;

// Dragon viewed from above: wide wings in the middle, body tapers to tail on the left,
// layers build up toward the head (right side).
[CreateAssetMenu(fileName = "DragonLayout", menuName = "Mahjong/Layouts/Dragon")]
public class DragonLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — body + wings (68 tiles)
        int[,] layer0 = {
            {0,0,0,0,0,0,1,1,1,1,0,0},  // y=  7  wing tips
            {0,0,0,1,1,1,1,1,1,1,1,0},  // y=  5  wings
            {0,0,1,1,1,1,1,1,1,1,1,1},  // y=  3  body widens
            {1,1,1,1,1,1,1,1,1,1,1,1},  // y=  1  full width
            {1,1,1,1,1,1,1,1,1,1,1,1},  // y= -1  full width
            {0,0,1,1,1,1,1,1,1,1,1,1},  // y= -3  body widens
            {0,0,0,1,1,1,1,1,1,1,1,0},  // y= -5  wings
            {0,0,0,0,0,0,1,1,1,1,0,0},  // y= -7  wing tips
        };
        int[] yValues0 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        AddLayer(positions, layer0, yValues0, 0);

        // Layer 1 — raised body, shifting toward head (48 tiles)
        int[,] layer1 = {
            {0,0,0,0,0,1,1,1,1,1,1,0},  // y=  5
            {0,0,0,0,1,1,1,1,1,1,1,1},  // y=  3
            {0,0,1,1,1,1,1,1,1,1,1,1},  // y=  1
            {0,0,1,1,1,1,1,1,1,1,1,1},  // y= -1
            {0,0,0,0,1,1,1,1,1,1,1,1},  // y= -3
            {0,0,0,0,0,1,1,1,1,1,1,0},  // y= -5
        };
        int[] yValues1 = { 5, 3, 1, -1, -3, -5 };
        AddLayer(positions, layer1, yValues1, 1);

        // Layer 2 — head area (28 tiles)
        int[,] layer2 = {
            {0,0,0,0,0,0,1,1,1,1,1,1},  // y=  3
            {0,0,0,0,1,1,1,1,1,1,1,1},  // y=  1
            {0,0,0,0,1,1,1,1,1,1,1,1},  // y= -1
            {0,0,0,0,0,0,1,1,1,1,1,1},  // y= -3
        };
        int[] yValues2 = { 3, 1, -1, -3 };
        AddLayer(positions, layer2, yValues2, 2);

        // Layer 3 — raised head (12 tiles)
        int[,] layer3 = {
            {0,0,0,0,0,0,1,1,1,1,1,1},  // y=  1
            {0,0,0,0,0,0,1,1,1,1,1,1},  // y= -1
        };
        int[] yValues3 = { 1, -1 };
        AddLayer(positions, layer3, yValues3, 3);

        return positions;  // 68 + 48 + 28 + 12 = 156 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 11, yValues[row], z));
    }
}
