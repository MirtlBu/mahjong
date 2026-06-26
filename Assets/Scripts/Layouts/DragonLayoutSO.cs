using System.Collections.Generic;
using UnityEngine;

// Dragon viewed from above: wide wings in the middle,
// layers build up toward the right (head side).
[CreateAssetMenu(fileName = "DragonLayout", menuName = "Mahjong/Layouts/Dragon")]
public class DragonLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Layer 0 — body + wings (64 tiles)
        int[,] layer0 = {
            {0,0,0,1,1,1,1,0,0,0},  // y= 7  wing tips
            {0,1,1,1,1,1,1,1,1,0},  // y= 5  wings
            {1,1,1,1,1,1,1,1,1,1},  // y= 3  full body
            {1,1,1,1,1,1,1,1,1,1},  // y= 1
            {1,1,1,1,1,1,1,1,1,1},  // y=-1
            {1,1,1,1,1,1,1,1,1,1},  // y=-3  full body
            {0,1,1,1,1,1,1,1,1,0},  // y=-5  wings
            {0,0,0,1,1,1,1,0,0,0},  // y=-7  wing tips
        };
        int[] y0 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        AddLayer(positions, layer0, y0, 0);

        // Layer 1 — body raised, shifting toward head (right) (44 tiles)
        int[,] layer1 = {
            {0,0,0,1,1,1,1,1,1,0},  // y= 5
            {0,0,1,1,1,1,1,1,1,1},  // y= 3
            {0,0,1,1,1,1,1,1,1,1},  // y= 1
            {0,0,1,1,1,1,1,1,1,1},  // y=-1
            {0,0,1,1,1,1,1,1,1,1},  // y=-3
            {0,0,0,1,1,1,1,1,1,0},  // y=-5
        };
        int[] y1 = { 5, 3, 1, -1, -3, -5 };
        AddLayer(positions, layer1, y1, 1);

        // Layer 2 — head area, right side (24 tiles)
        int[,] layer2 = {
            {0,0,0,0,1,1,1,1,1,1},  // y= 3
            {0,0,0,0,1,1,1,1,1,1},  // y= 1
            {0,0,0,0,1,1,1,1,1,1},  // y=-1
            {0,0,0,0,1,1,1,1,1,1},  // y=-3
        };
        int[] y2 = { 3, 1, -1, -3 };
        AddLayer(positions, layer2, y2, 2);

        // Layer 3 — raised head (8 tiles)
        int[,] layer3 = {
            {0,0,0,0,0,0,1,1,1,1},  // y= 1
            {0,0,0,0,0,0,1,1,1,1},  // y=-1
        };
        int[] y3 = { 1, -1 };
        AddLayer(positions, layer3, y3, 3);

        return positions;  // 64 + 44 + 24 + 8 = 140 tiles
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 9, yValues[row], z));
    }
}
