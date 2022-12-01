using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _gameObjectSpeed = 3;
    [SerializeField] float _tripleShotDuration = 5f;
    [SerializeField] float _speedBoostDuration = 6f;


    int _powerUpID;
    public int PowerUpID
    {
        get { return _powerUpID; }
    }

    string _powerUpName;

    [SerializeField] AudioClip _powerUpSoundClip;
    AudioSource _audioSource;

    void Awake()
    {
        transform.position = new Vector3(Random.Range(-10.6f, 10f), 7.5f, 0);

        _powerUpName = gameObject.name;
        Debug.Log(_powerUpName);
        switch (_powerUpName)
        {
            case "Triple_Shot_PowerUp(Clone)":
                _powerUpID = 0;
                //Debug.Log("Spawned Triple Shot PowerUp");
                break;
            case "Speed_PowerUp(Clone)":
                _powerUpID = 1;
                //Debug.Log("Spawned Speed PowerUp");
                break;
            case "Shield_PowerUp(Clone)":
                _powerUpID = 2;
                break;
        }

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _powerUpSoundClip;

    }

    void Update()
    {
        transform.position += Vector3.down * _gameObjectSpeed * Time.deltaTime;

        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.transform.tag == "Player")
        {
            Player player = collider.gameObject.GetComponent<Player>();
            switch (PowerUpID)
            {
                case 0:
                    player.ActivateTripleShot(_tripleShotDuration); ;
                    Debug.Log("Got Triple Shot PowerUp");
                    break;
                case 1:
                    player.ActivateSpeed(1.3f , _speedBoostDuration);
                    Debug.Log("Got Speed PowerUp");
                    break;
                case 2:
                    if (player.Lives > 0)
                    {
                        player.ActivateShield();
                    }                
                    break;
            }

            _audioSource.Play();
            Destroy(this.gameObject);          
        }
    }
}
