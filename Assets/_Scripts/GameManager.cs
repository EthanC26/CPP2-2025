using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

   // public event Action<Player> OnPlayerSpawned;
    public event Action<int> OnLifeValueChanged;

    #region Game Properties
    [SerializeField] private int MaxLives = 10;
    private int _lives = 3;
    
    public int Lives
    {
        get => _lives;
        set
        {
            if (value < 0)
            {
                GameOver();
                return;
            }
           // if (_lives > value) Respawn();

            _lives = value;

            if(_lives > MaxLives) _lives = MaxLives;

            OnLifeValueChanged?.Invoke(_lives);

            Debug.Log($"{gameObject.name} lives: {_lives}");

        }
    }
    #endregion
    #region PlayerController Info
    [SerializeField] private Player playerPrefab;
    private Player _playerInstance;
    public Player PlayerInstance => _playerInstance;
    #endregion

    private MenuController CurrentMenuController;
    private Transform currentCheckpoint;


    public void SetMenuController(MenuController newMenuController) => CurrentMenuController = newMenuController;

    private void Awake()
    {
        Debug.Log($"Current Lives: {Lives}");

        if(!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            return;
        }
        Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(MaxLives <= 0) MaxLives = 10;
        _lives = MaxLives;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            string sceneName = (SceneManager.GetActiveScene().name.Contains("Game")) ? "Title" : "Game";
            SceneManager.LoadScene(sceneName);

            string sceneName_ = (SceneManager.GetActiveScene().name.Contains("GameOver")) ? "Title" : "GameOver";
            SceneManager.LoadScene(sceneName_);

            string _sceneName = (SceneManager.GetActiveScene().name.Contains("Title")) ? "Game" : "Title";
            SceneManager.LoadScene(_sceneName);

        }

        if (SceneManager.GetActiveScene().name.Contains("Game"))
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (CurrentMenuController.CurrentState.state == MenuStates.InGame)
                    CurrentMenuController.SetActiveState(MenuStates.Pause);
                else
                    CurrentMenuController.JumpBack();
            }
        }
    }
    void GameOver()
    {
        if(Lives <= 0)
        {
            string sceneName = (SceneManager.GetActiveScene().name.Contains("Game")) ? "GameOver" : "Game";
            SceneManager.LoadScene(sceneName);

            Debug.Log("Game Over gose here :(");
        }

        Lives = 3;
    }

    //void Respawn()
    //{
    //    _playerInstance.transform.position = currentCheckpoint.position;
    //}

}
