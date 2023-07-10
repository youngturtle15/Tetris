using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public int[,] boardArray = new int[22, 10];

    private Tetromino tetromino;
    private int[,] blockArray = new int[4, 4];

    private List<int> rowsToClear = new List<int>();

    [SerializeField]
    private Vector2 initialPos;
    private Vector2 potentialNewPos = Vector2.zero;

    private Coroutine fallCoroutine;
    private bool isInputAllowed;

    [SerializeField]
    private List<Sprite> tileSprites = new List<Sprite>();

    public static UnityEvent spawnShapeEvent = new UnityEvent();
    public static UnityEvent GameOverEvent = new UnityEvent();

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
        if (!isInputAllowed)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int potentialRot = currBlockRotation;

            if (potentialRot == 3)
            {
                potentialRot = 0;
            }
            else
            {
                potentialRot++;
            }

            if (!CheckIfValidRot(potentialRot))
            {
                return;
            }

            currBlockRotation = potentialRot;
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

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StopCoroutine(fallCoroutine);
            potentialNewPos += new Vector2(1f, 0f);
            if (!CheckIfValidMove())
            // 확인 작업 들어가고
            {
                PieceLockedIn();
                return;
            }
            // 움직일 수 있으면 움직인다
            currShape.position += new Vector3(0f, -1f, 0f);
            fallCoroutine = StartCoroutine(TetrominoFall());
        }

        if (Input.GetKey(KeyCode.DownArrow))
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
        isInputAllowed = true;

        while (currShape.position.y > 0)
        {
            yield return new WaitForSeconds(fallSpeed * speedUpMultiplier);

            potentialNewPos += new Vector2(1f, 0f);
            if (!CheckIfValidMove())
            // 확인 작업 들어가고
            {
                PieceLockedIn();
                yield break;
            }
            // 움직일 수 있으면 움직인다
            currShape.position += new Vector3(0f, -1f, 0f);
        }
    }

    void SpawnNewPiece()
    {
        spawnShapeEvent.Invoke();
        GenerateBlock();
        currShape.position = new Vector3(initialPos.x, initialPos.y, 0f);
        potentialNewPos = new Vector2(22 - currShape.position.y, currShape.position.x);
        fallCoroutine = StartCoroutine(TetrominoFall());
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
                    else if ((int)potentialNewPos.x + i > 21 || boardArray[(int)potentialNewPos.x + i, (int)potentialNewPos.y + j] != 0)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    bool CheckIfValidRot(int rotation)
    {
        int[,] potentialRot = tetromino.GetShapeInRot(rotation);

        for (int i = 0; i < potentialRot.GetLength(0); i++)
        {
            for (int j = 0; j < potentialRot.GetLength(1); j++)
            {
                if (potentialRot[i, j] != 0)
                {
                    if ((int)potentialNewPos.y + j < 0 || (int)potentialNewPos.y + j > 9)
                    {
                        return false;
                    }
                    else if ((int)potentialNewPos.x + i > 21 || boardArray[(int)potentialNewPos.x + i, (int)potentialNewPos.y + j] != 0)
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
        isInputAllowed = false;
        StopCoroutine(fallCoroutine);

        currBlockRotation = 0;

        HashSet<int> rowsToCheck = new HashSet<int>();

        for (int i = 0; i < blockArray.GetLength(0); i++)
        {
            for (int j = 0; j < blockArray.GetLength(1); j++)
            {
                if (blockArray[i, j] != 0)
                {
                    boardArray[(int)potentialNewPos.x + i - 1, (int)potentialNewPos.y + j] = blockArray[i, j];
                    Debug.Log((int)potentialNewPos.x + i - 1);
                    rowsToCheck.Add((int)potentialNewPos.x + i - 1);
                }
            }
        }

        for (int i = currShape.childCount - 1; i >= 0; i--)
        {
            Transform blockObj = currShape.GetChild(i);
            blockObj.GetComponent<Tile>().boardArrayLoc = new Vector2(21 - (blockObj.position.y - 0.5f), blockObj.position.x - 0.5f);
            currShape.GetChild(i).SetParent(tetrisBoard);
        }

        CheckForCompleteLines(rowsToCheck);

        float invokeTime = 0f;

        if (rowsToClear.Count > 0)
        {
            ClearCompletedLines();
            invokeTime = 1f;
        }

        if (CheckIfGameOver())
        {
            StopAllCoroutines();
            return;
        }

        Invoke("SpawnNewPiece", invokeTime);
    }

    bool CheckIfGameOver()
    {
        for (int i = 0; i < 10; i++)
        {
            if (boardArray[1, i] != 0 || boardArray[0, i] != 0)
            {
                Debug.Log("Game Over");
                GameOverEvent.Invoke();
                return true;
            }
        }
        return false;
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
        //int[,] newBoard = new int[20, 10];
        int[,] newBoard = (int[,])boardArray.Clone();
        int[,] tempBoard = (int[,])boardArray.Clone();

        rowsToClear.Sort();

        PrintBoard(boardArray);
        // array copy down logic
        foreach (int row in rowsToClear)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < tempBoard.GetLength(1); j++)
                {
                    newBoard[i + 1, j] = tempBoard[i, j];
                }
            }
            tempBoard = (int[,])newBoard.Clone();
        }
        PrintBoard(newBoard);

        boardArray = newBoard;

        // actual tile move down logic
        // 1. clear tiles equal to the num of non zero numbers deleted in array
        StartCoroutine(DeleteTilesInRow());

        //for (int i = 0; i < tetrisBoard.childCount; i++)
        //{
        //    Transform blockTr = tetrisBoard.GetChild(i);
        //    Tile block = blockTr.GetComponent<Tile>();
        //    int blockRow = (int)block.boardArrayLoc.x;
        //    if (rowsToClear.Contains(blockRow))
        //    {
        //        Destroy(blockTr.gameObject);
        //    }
        //    else
        //    {
        //        blockTr.position += new Vector3(0f, -(rowsToClear.Count), 0f);
        //        block.boardArrayLoc += new Vector2((rowsToClear.Count), 0);
        //    }
        //}
        
        rowsToClear.Clear();
    }

    void PrintBoard(int[,] boardArray)
    {
        string boardString = "";

        for (int i = 0; i < boardArray.GetLength(0); i++)
        {
            for (int j = 0; j < boardArray.GetLength(1); j++)
            {
                boardString += $"{boardArray[i, j]}";
            }
            boardString += "\n";
        }

        Debug.Log(boardString);
    }

    IEnumerator DeleteTilesInRow()
    {
        for (int i = tetrisBoard.childCount - 1; i >= 0; i--)
        {
            if (rowsToClear.Contains((int)tetrisBoard.GetChild(i).GetComponent<Tile>().boardArrayLoc.x))
            {
                Destroy(tetrisBoard.GetChild(i).gameObject);
            }
        }
        yield return new WaitForSeconds(0.5f);

        int tileObjIndex = 0;

        for (int i = 0; i < boardArray.GetLength(0); i++)
        {
            for (int j = 0; j < boardArray.GetLength(1); j++)
            {
                if (boardArray[i, j] != 0)
                {
                    Transform tileObj = tetrisBoard.GetChild(tileObjIndex);
                    tileObj.position = new Vector3(j + 0.5f, (21 - i) + 0.5f, 0f);
                    tileObj.GetComponent<Tile>().boardArrayLoc = new Vector2(i, j);
                    tileObj.GetComponent<SpriteRenderer>().sprite = tileSprites[boardArray[i, j] - 1];
                    tileObjIndex++;
                }
            }
        }
    }
}
