using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _fallSpeed;
    // 떨어지는 속도 (단위: sec/line)
    public static float fallSpeed { get; private set; }

    public int[,] boardArray = new int[10, 20];

    private void Awake()
    {
        fallSpeed = _fallSpeed;
    }

    private void Start()
    {
        
    }
}
