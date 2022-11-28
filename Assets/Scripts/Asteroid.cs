using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] float _rotateSpeed = 3.0f;
    
    Animator _asteroidAnimator;
    Player _player;

    void Start()
    {

        _asteroidAnimator = transform.GetComponent<Animator>();

        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    
    void Update()
    {
        if (_asteroidAnimator.GetBool("OnAsteroidDestruction") == false)
        {
            transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Laser")
        {
          
            _asteroidAnimator.SetTrigger("OnAsteroidDestruction");
            if (other.transform.tag == "Player")
            {
                _player.Damage();
            }
            else if (other.transform.tag == "Laser")
            {
                Destroy(other.gameObject);
            }
            Destroy(this.gameObject, 2.2f);
        }
    }
}   
