using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private Animator _destroyed;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _destroyed = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CalculateMovement();
        FireLaser();
    }

    void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            float RandomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(RandomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            DestroyEnemy();
        }
        if (other.tag == "Laser")
        {   
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            DestroyEnemy();
            Destroy(GetComponent<Collider2D>());
        }
    }

    void DestroyEnemy()
    {
        _destroyed.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _audioSource.Play();
        Destroy(this.gameObject, 2.5f);
    }
}