using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] float _rotateSpeed = 3.0f;

    Transform _explosionTransform;
    GameObject _explosionObject;
    Animator _explosionAnimator;

    Player _player;

    void Start()
    {
        _explosionTransform = gameObject.transform.Find("Explosion");
        _explosionObject = _explosionTransform.gameObject;
        _explosionAnimator = _explosionObject.GetComponent<Animator>();

        _explosionObject.SetActive(false);

        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Laser")
        {
            _explosionObject.SetActive(true);
            _explosionAnimator.SetTrigger("OnAsteroidDestruction");
            if (other.transform.tag == "Player")
            {
                _player.Damage();
            }
            Destroy(this.gameObject, 2.2f);
        }
    }
}   
