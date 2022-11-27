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
    GameObject _restartText;
    GameManager _gameManager;

    
    void Start()
    {     
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        _gameOverText = GameObject.Find("Game_Over_Text");
        if (_gameOverText != null)
        {
            _gameOverText.SetActive(false);
        }

        

        _restartText = GameObject.Find("Restart_Text");
        if (_restartText != null)
        {
            _restartText.SetActive(false);
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        
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
                _gameOverText.transform.Translate(Vector3.down * 80f * Time.deltaTime);
            }
            else if (gameOverYPos <= 240f && _gameManager.GameOver == false)
            {
                _gameManager.GameOver = true;
                StartCoroutine(BlinkRoutine(_gameOverText, 0.15f, false));
            }
            else if (gameOverYPos <= 240f && _gameManager.GameOver == true)
            {
                _restartText.SetActive(true);
                return;
            }
        }
    }

    IEnumerator BlinkRoutine(GameObject blinkObject, float blinkRate, bool hideAfterBlink)
    {
        blinkObject.SetActive(false);
        yield return new WaitForSeconds(blinkRate);
        blinkObject.SetActive(true);
        yield return new WaitForSeconds(blinkRate);
        blinkObject.SetActive(false);
        yield return new WaitForSeconds(blinkRate);
        blinkObject.SetActive(true);
        yield return new WaitForSeconds(blinkRate);
  
        if (hideAfterBlink == true)
        {
            blinkObject.SetActive(false);
        }     
    }

}
