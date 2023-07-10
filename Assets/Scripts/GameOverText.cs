using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    private TMP_Text gameOverText;

    void Start()
    {
        gameOverText = GetComponent<TMP_Text>();
        GameManager.GameOverEvent.AddListener(PrintGameOverText);
    }

    void PrintGameOverText()
    {
        gameOverText.text = "Game Over!";
    }
}
