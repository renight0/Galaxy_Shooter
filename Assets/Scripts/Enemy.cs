using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   
    [SerializeField] float _speed = 2.5f;
    Player _player;
    
    bool _isPlayerDead = false;

    SpawnManager spawnManager;

    Animator _animator;

    Transform _leftThruster, _rightThruster;

    bool _isEnemyDying;

    void Start()
    {

        spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        transform.position = new Vector3(Random.Range(-10.6f, 10f), 7.5f, 0);

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null) { Debug.LogError("Player is NULL"); }

        _animator = GetComponent<Animator>();
        if (_animator == null) { Debug.LogError("Animator is NULL"); }

        _leftThruster = transform.Find("Thruster_L");
        _rightThruster = transform.Find("Thruster_R");

        _isEnemyDying = false;
    }

  
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * _speed;

        IsPlayerDead();

        EnemyMovementLoop();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hit: " + other.transform.name);
        if (other.transform.tag == "Laser")
        {
            
            spawnManager.DecreaseEnemyCount();
            if (_player != null && _isEnemyDying == false) 
            { 
                _player.EarnScorePoints(10);
                _isEnemyDying = true;
            }

            Destroy(other.gameObject);
            EnemyDeath();         
        }
        else if (other.transform.tag == "Player")
        {
            //Debug.Log("Player loses health");
            spawnManager.DecreaseEnemyCount();
 
            if (_player != null) { _player.Damage(); }
            EnemyDeath();

        }
        
    }

    void IsPlayerDead() 
    {
        if (_player.Lives == 0)
        {
            _isPlayerDead = true;
        }
    }

    void EnemyMovementLoop()
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

    void EnemyDeath()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0f;
        _leftThruster.gameObject.SetActive(false);
        _rightThruster.gameObject.SetActive(false);
        Destroy(this.gameObject, 2.8f);
    }
}
