using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   
    [SerializeField] float _speed = 2.5f;
    Player _player;
    
    bool _isPlayerDead = false;

    SpawnManager spawnManager;

    void Start()
    {

        spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        transform.position = new Vector3(Random.Range(-10.6f, 10f), 7.5f, 0);

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
     
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
            _player.EarnScorePoints(10);

            Destroy(this.gameObject);
            Destroy(other.gameObject);

        }
        else if (other.transform.tag == "Player")
        {
            //Debug.Log("Player loses health");
            spawnManager.DecreaseEnemyCount();

            Player player = other.GetComponent<Player>();
            if (player != null) { player.Damage(); }
            Destroy(this.gameObject);
            
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
}
