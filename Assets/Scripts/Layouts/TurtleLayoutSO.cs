using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurtleLayout", menuName = "Mahjong/Layouts/Turtle")]
public class TurtleLayoutSO : LayoutSO
{
    public override List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Слой 0 — основание (классическое Turtle)
        int[,] layer0 = {
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,0},
        };
        int[] yValues0 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        AddLayer(positions, layer0, yValues0, 0);

        // Слой 1
        int[,] layer1 = {
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,0,1,1,1,1,1,1,1,1,0,0},
        };
        int[] yValues1 = { 5, 3, 1, -1, -3, -5 };
        AddLayer(positions, layer1, yValues1, 1);

        // Слой 2
        int[,] layer2 = {
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
        };
        int[] yValues2 = { 3, 1, -1, -3 };
        AddLayer(positions, layer2, yValues2, 2);

        // Слой 3
        int[,] layer3 = {
            {0,0,0,0,0,1,1,0,0,0,0,0},
            {0,0,0,0,0,1,1,0,0,0,0,0},
        };
        int[] yValues3 = { 1, -1 };
        AddLayer(positions, layer3, yValues3, 3);

        // Слой 4 — верхушка
        positions.Add(new Vector3Int(0, 0, 4));

        // Крылья
        positions.Add(new Vector3Int(-14, 0, 0));
        positions.Add(new Vector3Int( 14, 0, 0));

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
