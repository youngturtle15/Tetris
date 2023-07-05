using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _fallSpeed;
    // 떨어지는 속도 (단위: sec/line)
    public static float fallSpeed { get; private set; }

    private float speedUpMultiplier = 1f;

    [SerializeField]
    private int currBlockRotation;

    public Transform currShape;
    [SerializeField]
    private Transform tetrisBoard;

    public int[,] boardArray = new int[20, 10];

    private Tetromino tetromino;
    private int[,] blockArray = new int[4, 4];

    private List<int> rowsToClear = new List<int>();

    [SerializeField]
    private Vector2 initialPos;
    private Vector2 potentialNewPos = Vector2.zero;

    public static UnityEvent spawnShapeEvent = new UnityEvent();

    private void Awake()
    {
        fallSpeed = _fallSpeed;
    }

    private void Start()
    {
        tetromino = currShape.GetComponent<Tetromino>();
        SpawnNewPiece();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
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
        if (Input.GetKeyDown(KeyCode.DownArrow))
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            potentialNewPos -= new Vector2(0f, 1f);
            if (CheckIfValidMove())
            {
                currShape.position += new Vector3(-1f, 0f, 0f);
            }
            else
            {
                potentialNewPos += new Vector2(0f, 1f);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            potentialNewPos += new Vector2(0f, 1f);
            if (CheckIfValidMove())
            {
                currShape.position += new Vector3(1f, 0f, 0f);
            }
            else
            {
                potentialNewPos -= new Vector2(0f, 1f);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            speedUpMultiplier = 0.1f;
        }
        else
        {
            speedUpMultiplier = 1f;
        }
    }

    IEnumerator TetrominoFall()
    {
        while (currShape.position.y > 0)
        {
            yield return new WaitForSeconds(fallSpeed * speedUpMultiplier);
            potentialNewPos += new Vector2(1f, 0f);
            if (!CheckIfValidMove())
            {
                PieceLockedIn();
                break;
            }
            currShape.position += new Vector3(0f, -1f, 0f);
        }
    }

    void SpawnNewPiece()
    {
        spawnShapeEvent.Invoke();
        GenerateBlock();
        currShape.position = new Vector3(initialPos.x, initialPos.y, 0f);
        potentialNewPos = new Vector2(20 - currShape.position.y, currShape.position.x);
        StartCoroutine(TetrominoFall());
    }

    bool CheckIfValidMove()
    {
        for (int i = 0; i < blockArray.GetLength(0); i++)
        {
            for (int j = 0; j < blockArray.GetLength(1); j++)
            {
                if (blockArray[i, j] != 0)
                {
                    if ((int)potentialNewPos.y + j < 0 || (int)potentialNewPos.y + j > 9)
                    {
                        return false;
                    }
                    else if ((int)potentialNewPos.x + i > 19 || boardArray[(int)potentialNewPos.x + i, (int)potentialNewPos.y + j] == 1)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    void GenerateBlock()
    {
        blockArray = tetromino.GetShapeInRot(currBlockRotation);

        for (int i = 0; i < blockArray.GetLength(0); i++)
        {
            for (int j = 0; j < blockArray.GetLength(1); j++)
            {
                if (blockArray[i, j] != 0)
                {
                    GameObject colorTile = Instantiate(tetromino.colorSprite, currShape);
                    colorTile.transform.position = currShape.position + new Vector3(j + 0.5f, -i - 0.5f, 0f);
                }
            }
        }
    }

    void ClearBlock()
    {
        foreach (Transform block in currShape)
        {
            Destroy(block.gameObject);
        }
    }

    void PieceLockedIn()
    {
        HashSet<int> rowsToCheck = new HashSet<int>();

        for (int i = 0; i < blockArray.GetLength(0); i++)
        {
            for (int j = 0; j < blockArray.GetLength(1); j++)
            {
                if (blockArray[i, j] != 0)
                {
                    boardArray[(int)potentialNewPos.x + i - 1, (int)potentialNewPos.y + j] = 1;
                    Debug.Log((int)potentialNewPos.x + i - 1);
                    rowsToCheck.Add((int)potentialNewPos.x + i - 1);
                }
            }
        }

        for (int i = currShape.childCount - 1; i >= 0; i--)
        {
            Transform blockObj = currShape.GetChild(i);
            blockObj.GetComponent<Block>().boardArrayLoc = new Vector2(19 - (blockObj.position.y - 0.5f), blockObj.position.x - 0.5f);
            currShape.GetChild(i).SetParent(tetrisBoard);
        }

        CheckForCompleteLines(rowsToCheck);

        if (rowsToClear.Count > 0)
        {
            ClearCompletedLines();
        }

        SpawnNewPiece();
    }

    void CheckForCompleteLines(HashSet<int> rowSetToCheck)
    {
        List<int> rowsToCheck = rowSetToCheck.ToList();
        for (int i = 0; i < rowsToCheck.Count; i++)
        {
            bool rowComplete = true;

            for (int j = 0; j < boardArray.GetLength(1); j++)
            {
                if (boardArray[rowsToCheck[i], j] == 0)
                {
                    rowComplete = false;
                }
            }
            if (rowComplete)
            {
                rowsToClear.Add(rowsToCheck[i]);
            }
        }
    }

    void ClearCompletedLines()
    {
        int[,] newBoard = new int[20, 10];

        for (int i = 0; i < boardArray.GetLength(0) - rowsToClear.Count; i++)
        {
            for (int j = 0; j < boardArray.GetLength(1); j++)
            {
                newBoard[i + rowsToClear.Count, j] = boardArray[i, j];
            }
        }

        for (int i = 0; i < tetrisBoard.childCount; i++)
        {
            Transform blockTr = tetrisBoard.GetChild(i);
            Block block = blockTr.GetComponent<Block>();
            int blockRow = (int)block.boardArrayLoc.x;
            if (rowsToClear.Contains(blockRow))
            {
                Destroy(blockTr.gameObject);
            }
            else
            {
                blockTr.position += new Vector3(0f, -(rowsToClear.Count), 0f);
                block.boardArrayLoc += new Vector2((rowsToClear.Count), 0);
            }
        }

        boardArray = newBoard;
        rowsToClear.Clear();
    }
}
