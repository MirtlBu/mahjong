using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrossLayout", menuName = "Mahjong/Layouts/Cross")]
public class CrossLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Слой 0 — форма креста (64 тайла)
        int[,] layer0 = {
            // y=7
            {0,0,0,0,1,1,1,1,0,0,0,0},
            // y=5
            {0,0,0,0,1,1,1,1,0,0,0,0},
            // y=3
            {1,1,1,1,1,1,1,1,1,1,1,1},
            // y=1
            {1,1,1,1,1,1,1,1,1,1,1,1},
            // y=-1
            {1,1,1,1,1,1,1,1,1,1,1,1},
            // y=-3
            {1,1,1,1,1,1,1,1,1,1,1,1},
            // y=-5
            {0,0,0,0,1,1,1,1,0,0,0,0},
            // y=-7
            {0,0,0,0,1,1,1,1,0,0,0,0},
        };
        int[] yValues0 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        AddLayer(positions, layer0, yValues0, 0);

        // Слой 1 — центральное возвышение (16 тайлов)
        int[,] layer1 = {
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
        };
        int[] yValues1 = { 3, 1, -1, -3 };
        AddLayer(positions, layer1, yValues1, 1);

        // Слой 2 — пик (4 тайла)
        int[,] layer2 = {
            {0,0,0,0,0,1,1,0,0,0,0,0},
            {0,0,0,0,0,1,1,0,0,0,0,0},
        };
        int[] yValues2 = { 1, -1 };
        AddLayer(positions, layer2, yValues2, 2);

        return positions;
    }

    static void AddLayer(List<Vector3Int> positions, int[,] mask, int[] yValues, int z)
    {
        for (int row = 0; row < mask.GetLength(0); row++)
            for (int col = 0; col < mask.GetLength(1); col++)
                if (mask[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 11, yValues[row], z));
    }
}
