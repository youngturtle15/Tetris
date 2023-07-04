using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // 생성되는 위치
    public Vector3 spawnPosition;
    [SerializeField]
    private Vector3 transformOffset;

    void Start()
    {
        transform.position = spawnPosition + transformOffset;
        StartCoroutine(TetrominoFall());
    }

    IEnumerator TetrominoFall()
    {
        while (transform.position.y > 0)
        {
            yield return new WaitForSeconds(GameManager.fallSpeed);
            transform.position += new Vector3(0f, -1f, 0f);
        }
    }
}
