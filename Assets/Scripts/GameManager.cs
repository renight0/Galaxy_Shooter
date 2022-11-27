using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool _isGameOver;

    public bool GameOver
    {
        get { return _isGameOver; }

        set { _isGameOver = value; }
    }


    void Start()
    {
        GameOver = false;
    }


    void Update()
    {
        RestartGame();
    }

    void RestartGame()
    {
        if (GameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restart Level");
            //SceneManager.
            
        }
    }
}

