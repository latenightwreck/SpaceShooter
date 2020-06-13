﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = 0f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    void Start()
    {
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null) {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire ) {
            ShootLaser();
        }
    }

    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput);


        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));

        //To allow for screen wrapping
        if (transform.position.x >= 11.3f)
        {
        transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
        transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void ShootLaser() {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
    }

    public void Damage() {
        this._lives--;

        if (_lives <= 0) {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
}
