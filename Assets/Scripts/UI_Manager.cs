using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] Text _scoreText;

    Player _player;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();       
    }


    void Update()
    {      
        _scoreText.text = "Score: " + _player.Score;
    }
}
