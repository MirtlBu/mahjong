using System.Collections.Generic;
using UnityEngine;

// Layout "Turtle" для маджонг-солитера
// Координаты: x, y — позиция на сетке (шаг 2), z — слой (0 = нижний)
public static class TurtleLayout
{
    public static List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Слой 0 — нижний (10x7, 50 тайлов)
        int[,] layer0 = {
            {1,1,1,1,1,1,1,1,1,1}, // y= 6
            {0,0,0,1,1,1,1,0,0,0}, // y= 4
            {0,0,1,1,1,1,1,1,0,0}, // y= 2
            {1,1,1,1,1,1,1,1,1,1}, // y= 0
            {0,0,1,1,1,1,1,1,0,0}, // y=-2
            {0,0,0,1,1,1,1,0,0,0}, // y=-4
            {1,1,1,1,1,1,1,1,1,1}, // y=-6
        };
        int[] yValues0 = { 6, 4, 2, 0, -2, -4, -6 };
        AddLayer(positions, layer0, yValues0, 0);

        // Слой 1 — средний (10x5, 28 тайлов)
        int[,] layer1 = {
            {0,0,0,1,1,1,1,0,0,0}, // y= 4
            {0,0,1,1,1,1,1,1,0,0}, // y= 2
            {0,1,1,1,1,1,1,1,1,0}, // y= 0
            {0,0,1,1,1,1,1,1,0,0}, // y=-2
            {0,0,0,1,1,1,1,0,0,0}, // y=-4
        };
        int[] yValues1 = { 4, 2, 0, -2, -4 };
        AddLayer(positions, layer1, yValues1, 1);

        // Слой 2 — верхний (10x3, 8 тайлов)
        int[,] layer2 = {
            {0,0,0,0,1,1,0,0,0,0}, // y= 2
            {0,0,0,1,1,1,1,0,0,0}, // y= 0
            {0,0,0,0,1,1,0,0,0,0}, // y=-2
        };
        int[] yValues2 = { 2, 0, -2 };
        AddLayer(positions, layer2, yValues2, 2);

        return positions;
    }

    static void AddLayer(List<Vector3Int> positions, int[,] layer, int[] yValues, int z)
    {
        int cols = layer.GetLength(1);
        int xOffset = cols - 1; // центрирование: col*2 - (cols-1)
        for (int row = 0; row < layer.GetLength(0); row++)
            for (int col = 0; col < cols; col++)
                if (layer[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - xOffset, yValues[row], z));
    }
}
