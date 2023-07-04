using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public List<GameObject> colorSprites = new List<GameObject>();
    public BlockColor blockColor;
    public int currBlockRotation;
    public Transform blockParent;

    private GameObject colorSprite;
    private int[,,] blockArray;

    void Start()
    {
        colorSprite = colorSprites[(int)blockColor];

        switch (blockColor)
        {
            case BlockColor.LightBlue:
                blockArray = TetrominoBlock.IBlockArray;
                break;
            case BlockColor.Blue:
                blockArray = TetrominoBlock.JBlockArray;
                break;
            case BlockColor.Orange:
                blockArray = TetrominoBlock.LBlockArray;
                break;
            case BlockColor.Yellow:
                blockArray = TetrominoBlock.OBlockArray;
                break;
            case BlockColor.Green:
                blockArray = TetrominoBlock.SBlockArray;
                break;
            case BlockColor.Purple:
                blockArray = TetrominoBlock.TBlockArray;
                break;
            case BlockColor.Red:
                blockArray = TetrominoBlock.ZBlockArray;
                break;
            default:
                blockArray = TetrominoBlock.IBlockArray;
                break;
        }

        GenerateBlock();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currBlockRotation == 3)
            {
                currBlockRotation = 0;
            }
            else
            {
                currBlockRotation++;
            }
            ClearBlock();
            GenerateBlock();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currBlockRotation == 0)
            {
                currBlockRotation = 3;
            }
            else
            {
                currBlockRotation--;
            }
            ClearBlock();
            GenerateBlock();
        }
    }

    void ClearBlock()
    {
        foreach (Transform block in blockParent)
        {
            Destroy(block.gameObject);
        }
    }

    void GenerateBlock()
    {
        for (int i = 0; i < blockArray.GetLength(1); i++)
        {
            for (int j = 0; j < blockArray.GetLength(2); j++)
            {
                if (blockArray[currBlockRotation, i, j] == 1)
                {
                    GameObject colorTile = Instantiate(colorSprite, blockParent);
                    colorTile.transform.position = new Vector2(j + 0.5f, (3 - i) + 0.5f);
                }
            }
        }
    }
}
