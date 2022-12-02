using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] float _rotateSpeed = 3.0f;
    [SerializeField] float _speed = 2.5f;
    [SerializeField] int _moveSide = 0;

    Animator _asteroidAnimator;
    Player _player;

    bool _isPlayerDead;

    bool _isAsteroidExploding;

    [SerializeField] AudioClip _explosion;
    AudioSource _audioSource;

    void Start()
    {

        _isPlayerDead = false;

        _asteroidAnimator = transform.GetComponent<Animator>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        _moveSide = Random.Range(-1, 2);
        Debug.Log(_moveSide);
        gameObject.transform.localScale = Random.Range(0.4f, 1.1f)*Vector3.one;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explosion;

        _isAsteroidExploding = false;
    }

    
    void Update()
    {
        AsteroidMovement();

        AsteroidMovementLoop();


    }
    void AsteroidMovement()
    {
        if (_asteroidAnimator.GetBool("OnAsteroidDestruction") == false)
        {
            transform.Rotate(Vector3.forward * _rotateSpeed * Random.Range(1f, 3.5f) * Time.deltaTime);
            transform.Translate((Vector3.down + (Vector3.right * _moveSide)) * _speed * Time.deltaTime);
        }
        else if (_asteroidAnimator.GetBool("OnAsteroidDestruction") == true)
        {
            _speed = 0f;
        }
    }

    void AsteroidMovementLoop()
    {

        if (transform.position.y < -6 && _isPlayerDead == false)
        {
            transform.position = new Vector3(Random.Range(-10.5f, 10.1f), 7.5f, 0);
        }
        else if (transform.position.y < -6 && _isPlayerDead == true)
        {
            Destroy(this.gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Laser" || other.transform.tag == "Enemy")
        {
          
            _asteroidAnimator.SetTrigger("OnAsteroidDestruction");
            if (other.transform.tag == "Player")
            {
                _player.Damage();
            }
            else if (other.transform.tag == "Laser" && _isAsteroidExploding == false)
            {
                Destroy(other.gameObject);
            }
            else if (other.transform.tag == "Enemy" && _isAsteroidExploding == false)
            {
                Enemy enemyScript = other.gameObject.GetComponent<Enemy>();
                enemyScript.EnemyDeath();
            }

            if (_isAsteroidExploding == false)
            {
                _audioSource.Play();
                _isAsteroidExploding = true;
            }
            
            Destroy(this.gameObject, 2.2f);
        }    
    }
}   
