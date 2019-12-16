using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public GameObject TotalScoreText;

    GameManager gameManager;

    void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        TotalScoreText.GetComponent<Text>().text = gameManager.Score.ToString();
    }
    
}
