using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    float _laserSpeed = 20;
    Vector3 initPosOffset = new Vector3(0, 1.05f, 0);

    GameObject _laserParent;
    Player _player;

    bool _isEnemylaser = false;

    public bool EnemyLaser
    {
        get { return _isEnemylaser; }

        set { _isEnemylaser = value; }
    }


    void Awake()
    {
        transform.position += initPosOffset;

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player != null)
        {
            if (_player.TripleShot == true)
            {
                _laserParent = gameObject.transform.parent.gameObject;
            }
        }
        else { Debug.Log("Player is NULL"); }

  
    }

   
    void Update()
    {
        LaserMovement();

        DestroyOutOfBoundsLaser();

    }

    public void AssignEnemyLaser()
    { 
        _isEnemylaser = true;
    }

    void LaserMovement()
    {
        if (EnemyLaser == false)
        {
            transform.position += Vector3.up * _laserSpeed * Time.deltaTime;
        }
        else if (EnemyLaser == true)
        {
            transform.position -= Vector3.up * _laserSpeed * Time.deltaTime;
        }
    }

    void DestroyOutOfBoundsLaser()
    {
        if (transform.position.y > 8 || transform.position.y < -5.9f)
        {
            if (_laserParent != null)
            {
                Destroy(_laserParent);
            }

            if(_isEnemylaser == true)
            {
                GameObject enemyLaserParent = transform.parent.gameObject;
                if (enemyLaserParent != null)
                {
                    Destroy(enemyLaserParent);
                }
            }

            Destroy(this.gameObject);
        }
    }

}

