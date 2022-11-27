using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 7f;
    public float Speed
    {
        get { return _speed; }

        set { _speed = value; }
    }


    [SerializeField] bool _isTripleShotActive = false;

    public bool TripleShot
    {
        get { return _isTripleShotActive; }

        set { _isTripleShotActive = value; }
    }

    [SerializeField] GameObject _shieldObject;

    [SerializeField] GameObject _laserPrefab;
    [SerializeField] GameObject _tripleShot;

    [SerializeField] float _fireRate = 0.5f;
    float _timeUntilCanFireAgain = -1f;


    [SerializeField] int _lives = 3;
    public int Lives
    {
        get { return _lives; }
    }

    [SerializeField] int _shield = 0;
    public int Shield
    {
        get { return _shield; }
    }


    SpawnManager _spawnManager;

    [SerializeField] int _score;

    public int Score
    {
        get { return _score; }

        set { _score = value; }
    }

    UI_Manager _UIManager;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null!");
        }

        _shieldObject.GetComponent<Renderer>().enabled = false;

        _UIManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
 
    }


    void Update()
    {

        PlayerMovement();
        Fire();


    }



    public void Damage()
    {
        if (_shield > 0)
        {
            _shield--;
            Blink(_shieldObject, 0.1f, true);

        }
        else if (_shield == 0)
        {
            _shieldObject.GetComponent<Renderer>().enabled = false;

            _lives--;
            _UIManager.UpdateLives(_lives);

            if (_lives == 0)
            {

                _spawnManager.OnPlayerDeathStopSpawning();

                Destroy(this.gameObject);

            }
            else if (this.gameObject != null && _lives>0)
            {
                Blink(this.gameObject, 0.1f, false);
            }
        }


    }

    IEnumerator DamageBlinkRoutine(GameObject blinkObject, float blinkRate, bool hideAfterBlink)
    {
        blinkObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(blinkRate);
        blinkObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(blinkRate);
        blinkObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(blinkRate);
        blinkObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(blinkRate);
        blinkObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(blinkRate);
        blinkObject.GetComponent<Renderer>().enabled = true;
        if (hideAfterBlink == true)
        {
            blinkObject.GetComponent<Renderer>().enabled = false;
        }

    }
  
    IEnumerator ActivateTripleShotRoutine(float tripleShotDuration)
    {
        TripleShot = true;
        yield return new WaitForSeconds(tripleShotDuration);
        TripleShot = false;
    }

    public void ActivateTripleShot(float tripleShotDuration)
    {
        StartCoroutine(ActivateTripleShotRoutine(tripleShotDuration));
    }

    IEnumerator ActivateSpeedRoutine(float speedMultiplier, float speedBostDuration)
    {
        float originalSpeed = Speed;
        Speed *= speedMultiplier;
        yield return new WaitForSeconds(speedBostDuration);
        Speed = originalSpeed;
    }

    public void ActivateSpeed(float speedMultiplier, float speedBostDuration)
    {
        StartCoroutine(ActivateSpeedRoutine(speedMultiplier, speedBostDuration));
    }

    public void ActivateShield()
    {
        _shield = 1;
        _shieldObject.GetComponent<Renderer>().enabled = true;
        
    }

    void Blink(GameObject blinkObject, float blinkRate, bool hideAfterBlink)
    {
        StartCoroutine(DamageBlinkRoutine(blinkObject, blinkRate, hideAfterBlink));
    }

    void Fire() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timeUntilCanFireAgain)
        {
            _timeUntilCanFireAgain = Time.time + _fireRate;

            switch (_isTripleShotActive)
            {
                case false:
                    Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                    break;
                case true:
                    Instantiate(_tripleShot, transform.position, Quaternion.identity);
                    break;
            }           
        }
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        MovementBoundaries();
    }

    void MovementBoundaries()
    {
        if (transform.position.y >= 5.99f)
        {
            transform.position = new Vector3(transform.position.x, 5.99f, 0);

        }
        else if (transform.position.y <= -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);

        }


        if (transform.position.x >= 12.2f)
        {
            transform.position = new Vector3(-12.5f, transform.position.y, 0);
        }
        else if (transform.position.x <= -12.6f)
        {
            transform.position = new Vector3(12.1f, transform.position.y, 0);
        }
    }

    public void EarnScorePoints(int points)
    {
        Score += points;
    
    }
}

