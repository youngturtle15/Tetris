using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public GameObject colorSprite;
    public int[,,] blockArray;

    public int[,] GetShapeInRot(int rotation)
    {
        int x = blockArray.GetLength(1);
        int y = blockArray.GetLength(2);

        int[,] shapeArray = new int[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                shapeArray[i, j] = blockArray[rotation, i, j];
            }
        }

        return shapeArray;
    }
}
