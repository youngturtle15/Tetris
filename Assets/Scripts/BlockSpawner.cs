using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public List<Sprite> colorSprites = new List<Sprite>();
    public Transform blockParent;

    [SerializeField]
    private GameObject colorSprite;
    private Tetromino tetromino;

    private void OnEnable()
    {
        GameManager.spawnShapeEvent.AddListener(SpawnTetromino);
    }

    void SpawnTetromino()
    {
        int randShape = Random.Range(0, 7);
        tetromino = blockParent.GetComponent<Tetromino>();
        tetromino.colorSprite.GetComponent<SpriteRenderer>().sprite = colorSprites[randShape];

        switch (randShape)
        {
            case 0:
                tetromino.blockArray = TetrominoData.IBlockArray;
                break;
            case 1:
                tetromino.blockArray = TetrominoData.JBlockArray;
                break;
            case 2:
                tetromino.blockArray = TetrominoData.LBlockArray;
                break;
            case 3:
                tetromino.blockArray = TetrominoData.OBlockArray;
                break;
            case 4:
                tetromino.blockArray = TetrominoData.SBlockArray;
                break;
            case 5:
                tetromino.blockArray = TetrominoData.TBlockArray;
                break;
            case 6:
                tetromino.blockArray = TetrominoData.ZBlockArray;
                break;
            default:
                tetromino.blockArray = TetrominoData.IBlockArray;
                break;
        }
    }
}
