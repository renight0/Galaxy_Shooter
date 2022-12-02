using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    Transform _leftDamageTransform, _rightDamageTransform;
    Transform _thrusterTransform;

    int _damageSideRandomNumber;
    
    UI_Manager _UIManager;

    Animator _animator;

    [SerializeField] AudioClip _laserSoundClip, _explosionSoundClip;
    AudioSource _audioSource;

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

        _leftDamageTransform = gameObject.transform.Find("Damage_Effect_L");
        _rightDamageTransform = gameObject.transform.Find("Damage_Effect_R");

        _leftDamageTransform.gameObject.SetActive(false);
        _rightDamageTransform.gameObject.SetActive(false);

        _damageSideRandomNumber = Random.Range(0, 2);
        Debug.Log("_damageSideRandomNumber =" + _damageSideRandomNumber);

        _thrusterTransform = gameObject.transform.Find("Thruster");
        _animator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _laserSoundClip;
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
            Blink(_shieldObject, 0.1f, true, 1);

        }
        else if (_shield == 0)
        {
            _shieldObject.GetComponent<Renderer>().enabled = false;

            _lives--;
            DamageVisualEffect();
            _UIManager.UpdateLives(_lives);

            if (_lives == 0)
            {

                _spawnManager.OnPlayerDeathStopSpawning();
                _animator.SetTrigger("OnPlayerDeath");
                Destroy(_leftDamageTransform.gameObject);
                Destroy(_rightDamageTransform.gameObject);
                Destroy(_thrusterTransform.gameObject);
                _audioSource.clip = _explosionSoundClip;
                _audioSource.Play();
                Destroy(this.gameObject, 2.8f);

            }
            else if (this.gameObject != null && _lives>0)
            {
                Blink(this.gameObject, 0.1f, false, 1);
            }
        }
    }

    void DamageVisualEffect()
    {
        switch (Lives)
        {
            case 0:              
                break;
            case 1:
                
                if (_damageSideRandomNumber == 0)
                {
                    _leftDamageTransform.gameObject.SetActive(true);
                }
                else if (_damageSideRandomNumber == 1)
                {
                    _rightDamageTransform.gameObject.SetActive(true);
                }

                break;
            case 2:
                if (_damageSideRandomNumber == 0)
                {
                    _rightDamageTransform.gameObject.SetActive(true);
                }
                else if (_damageSideRandomNumber == 1)
                {
                    _leftDamageTransform.gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }


    }

    IEnumerator DamageBlinkWithChildrenRoutine(GameObject blinkObject, float blinkRate, bool hideAfterBlink, int skipUntilChildNumber)
    {

        int numberOfChildren = blinkObject.transform.childCount;

        BlinkChildren(blinkObject, numberOfChildren, skipUntilChildNumber, false);
        blinkObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(blinkRate);

        BlinkChildren(blinkObject, numberOfChildren, skipUntilChildNumber, true);
        blinkObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(blinkRate);

        BlinkChildren(blinkObject, numberOfChildren, skipUntilChildNumber, false);
        blinkObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(blinkRate);

        BlinkChildren(blinkObject, numberOfChildren, skipUntilChildNumber, true);
        blinkObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(blinkRate);

        BlinkChildren(blinkObject, numberOfChildren, skipUntilChildNumber, false);
        blinkObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(blinkRate);

        BlinkChildren(blinkObject, numberOfChildren, skipUntilChildNumber, true);
        blinkObject.GetComponent<Renderer>().enabled = true;

        if (hideAfterBlink == true)
        {
            blinkObject.GetComponent<Renderer>().enabled = false;
        }

    }

    void BlinkChildren(GameObject parentObject, int numberOfChildren, int skipUntilChildNumber, bool isRendererEnabled)
    {
        for (int i = skipUntilChildNumber; i < numberOfChildren; i++)
        {
            parentObject.transform.GetChild(i).GetComponent<Renderer>().enabled = isRendererEnabled;
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

    void Blink(GameObject blinkObject, float blinkRate, bool hideAfterBlink, int skipUntilChildNumber)
    {
        StartCoroutine(DamageBlinkWithChildrenRoutine(blinkObject, blinkRate, hideAfterBlink, skipUntilChildNumber));
    }



    void Fire() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timeUntilCanFireAgain && Lives>0)
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

            _audioSource.Play();
        }
        
    }

    void PlayerMovement()
    {
        if (Lives > 0)
        { 
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            transform.Translate(direction * _speed * Time.deltaTime);
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Laser enemyLaserScript = other.GetComponent<Laser>();
        if (other.transform.tag == "Laser" & enemyLaserScript.EnemyLaser == true)
        {
            Damage();
        }
    }

}

