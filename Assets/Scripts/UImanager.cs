using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _RestartLevel;
    private GameManager _gameManager;


    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _RestartLevel.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

   
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 0)
        {
            currentLives = 0;
        }

        if (currentLives >= _liveSprites.Length)
        {
            currentLives = _liveSprites.Length - 1;
        }

        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        _RestartLevel.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlickerRoutine(){

        while (true)
        {
            _gameOver.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOver.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
