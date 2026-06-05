using System.Collections.Generic;
using UnityEngine;

// Классический layout "Turtle" для маджонг-солитера
// Координаты: x, y — позиция на сетке (шаг 2), z — слой (0 = нижний)
// Плитка занимает 2x2 единицы на сетке
public static class TurtleLayout
{
    public static List<Vector3Int> GetPositions()
    {
        var positions = new List<Vector3Int>();

        // Слой 0 — основание (12x8 зона, классическое Turtle)
        int[,] layer0 = {
            // y=7
            {0,1,1,1,1,1,1,1,1,1,1,0},
            // y=5
            {0,1,1,1,1,1,1,1,1,1,1,0},
            // y=3
            {1,1,1,1,1,1,1,1,1,1,1,1},
            // y=1
            {1,1,1,1,1,1,1,1,1,1,1,1},
            // y=-1
            {1,1,1,1,1,1,1,1,1,1,1,1},
            // y=-3
            {1,1,1,1,1,1,1,1,1,1,1,1},
            // y=-5
            {0,1,1,1,1,1,1,1,1,1,1,0},
            // y=-7
            {0,1,1,1,1,1,1,1,1,1,1,0},
        };

        int[] yValues0 = { 7, 5, 3, 1, -1, -3, -5, -7 };
        for (int row = 0; row < layer0.GetLength(0); row++)
        {
            for (int col = 0; col < layer0.GetLength(1); col++)
            {
                if (layer0[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 11, yValues0[row], 0));
            }
        }

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
        for (int row = 0; row < layer1.GetLength(0); row++)
        {
            for (int col = 0; col < layer1.GetLength(1); col++)
            {
                if (layer1[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 11, yValues1[row], 1));
            }
        }

        // Слой 2
        int[,] layer2 = {
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
        };
        int[] yValues2 = { 3, 1, -1, -3 };
        for (int row = 0; row < layer2.GetLength(0); row++)
        {
            for (int col = 0; col < layer2.GetLength(1); col++)
            {
                if (layer2[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 11, yValues2[row], 2));
            }
        }

        // Слой 3
        int[,] layer3 = {
            {0,0,0,0,0,1,1,0,0,0,0,0},
            {0,0,0,0,0,1,1,0,0,0,0,0},
        };
        int[] yValues3 = { 1, -1 };
        for (int row = 0; row < layer3.GetLength(0); row++)
        {
            for (int col = 0; col < layer3.GetLength(1); col++)
            {
                if (layer3[row, col] == 1)
                    positions.Add(new Vector3Int(col * 2 - 11, yValues3[row], 3));
            }
        }

        // Слой 4 — верхушка (1 плитка)
        positions.Add(new Vector3Int(0, 0, 4));

        // Боковые одиночные плитки (крылья черепахи)
        positions.Add(new Vector3Int(-14, 0, 0)); // левое крыло
        positions.Add(new Vector3Int(14, 0, 0));  // правое крыло

        return positions;
    }
}
