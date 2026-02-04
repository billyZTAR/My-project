using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UImanager _uiManager;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private AudioSource _laserAudio;
    private bool _tripleShot = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _canTakeDamage = true;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UImanager>();
        _laserAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        } 
       // Instantiate(_EnemyPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 3.5f)
        {
            transform.position = new Vector3(transform.position.x, 3.5f, 0);
        }
        else if (transform.position.y <= -3.5f)
        {
            transform.position = new Vector3(transform.position.x, -3.5f, 0);
        }

        if (transform.position.x >= 10)
        {
            transform.position = new Vector3(-10, transform.position.y, 0);
        }
        else if (transform.position.x <= -10)
        {
            transform.position = new Vector3(10, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_tripleShot == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, -3.5f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
        _laserAudio.Play();
    }

    public void Damage()
    {
        // Prevent instant multiple hits
        if (_canTakeDamage == false)
            return;

        _canTakeDamage = false;

        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            StartCoroutine(DamageCooldown());
            return;
        }
        _lives--;
        if (_lives < 0)
            _lives = 0;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            return;
        }
        StartCoroutine(DamageCooldown());
    }

    public void TripleSHotActive()
    {
        _tripleShot = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _tripleShot = false;
    }

    public void SpeedBoostActive()
    {
        if (_isSpeedBoostActive == false)
        {
            _isSpeedBoostActive = true;
            _speed *= _speedMultiplier;
            StartCoroutine(SpeedBoostPowerDown());
        }
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.6f); 
        _canTakeDamage = true;
    }
}