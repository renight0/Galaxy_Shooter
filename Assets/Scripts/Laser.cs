using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    float _laserSpeed = 20;
    Vector3 initPosOffset = new Vector3(0, 1.05f, 0);

    GameObject _laserParent;
    Player _player;

   

    void Awake()
    {
        transform.position += initPosOffset;

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player.TripleShot == true)
        { 
        _laserParent = gameObject.transform.parent.gameObject;
        }
    }

   
    void Update()
    {
        transform.position += Vector3.up * _laserSpeed * Time.deltaTime;

        if (transform.position.y > 8)
        {           
            if (_laserParent != null)
            {
                Destroy(_laserParent);
            }

            Destroy(this.gameObject);
        }
        
    }



}

