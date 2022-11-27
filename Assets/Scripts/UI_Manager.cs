using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] Text _scoreText;

    Player _player;

    [SerializeField] Image _livesImage;
    [SerializeField] Sprite[] _liveSprites;

    GameObject _gameOverText;

    void Start()
    {     
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        _gameOverText = GameObject.Find("Game_Over_Text");
        _gameOverText.SetActive(false);
    }


    void Update()
    {      
        _scoreText.text = "Score: " + _player.Score;

        gameOverFallDownOnScreenAfterDeath();
        
    }

    public void UpdateLives(int currentLives)
    { 
        _livesImage.sprite = _liveSprites[currentLives];

        if (_gameOverText != null)
        {
            
            if (_player.Lives == 0)
            {
                _gameOverText.SetActive(true);
            }
        }
    }

    void gameOverFallDownOnScreenAfterDeath()
    {  
        if (_player.Lives == 0)
        {
            float gameOverYPos = _gameOverText.transform.position.y;
            if (gameOverYPos > 240f)
            {
                _gameOverText.transform.Translate(Vector3.down * 40f * Time.deltaTime);
            }
        }
    }
}
