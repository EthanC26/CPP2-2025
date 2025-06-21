using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private LoadSaveManager saveManager;

    // public event Action<Player> OnPlayerSpawned;
    public event Action<int> OnLifeValueChanged;

    #region Game Properties
    [SerializeField] private int MaxLives = 10;
    private int _lives = 5;
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

           // if (_lives > value) DamageTaken();

            _lives = value;

            if (_lives > MaxLives) _lives = MaxLives;

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

    public void SetMenuController(MenuController newMenuController) => CurrentMenuController = newMenuController;

    private void Awake()
    {
        Debug.Log($"Current Lives: {Lives}");

        if (!_instance)
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
        if (MaxLives <= 0) MaxLives = 10;

        saveManager = GetComponent<LoadSaveManager>();
        if (_playerInstance == null)
        {
            _playerInstance = FindAnyObjectByType<Player>();
        }

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

            string scenename = (SceneManager.GetActiveScene().name.Contains("Victory")) ? "Title" : "Victory";
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

        if (SceneManager.GetActiveScene().name.Contains("Game"))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (CurrentMenuController.CurrentState.state == MenuStates.InGame)
                    CurrentMenuController.SetActiveState(MenuStates.Continue);
                else
                    CurrentMenuController.JumpBack();
            }



        }
    }

    void DamageTaken()
    {
        if (CurrentMenuController.CurrentState.state == MenuStates.InGame)
            CurrentMenuController.SetActiveState(MenuStates.Continue);
        else
            CurrentMenuController.JumpBack();
    }
   

    void GameOver()
    {
        if (Lives <= 0)
        {

            string sceneName = (SceneManager.GetActiveScene().name.Contains("Game")) ? "GameOver" : "Game";
            SceneManager.LoadScene(sceneName);

            if (CurrentMenuController.CurrentState.state == MenuStates.InGame)
                CurrentMenuController.SetActiveState(MenuStates.GameOver);
            else
                CurrentMenuController.JumpBack();
        }

        Lives = 5;
    }

    public void SaveGame()
    {
        if (_playerInstance == null)
        {
            Debug.LogError("SaveGame failed: _playerInstance is null!");
            return;
        }

        if (saveManager == null)
        {
            saveManager = GetComponent<LoadSaveManager>();
            if (saveManager == null)
            {
                Debug.LogError("SaveGame failed: saveManager component is missing!");
                return;
            }
        }


        var data = new LoadSaveManager.GameStateData.DataTransform
        {
            posx = _playerInstance.transform.position.x,
            posy = _playerInstance.transform.position.y,
            posz = _playerInstance.transform.position.z,
            rotx = _playerInstance.transform.rotation.x,
            roty = _playerInstance.transform.rotation.y,
            rotz = _playerInstance.transform.rotation.z,
            scaleX = _playerInstance.transform.localScale.x,
            scaleY = _playerInstance.transform.localScale.y,
            scaleZ = _playerInstance.transform.localScale.z
        };

        saveManager.gameState.playerData.transformData = data;
        saveManager.gameState.playerData.health = _lives;

        // Print the path where the XML is save
        Debug.Log(Application.persistentDataPath);

        // Call save game functionality
        saveManager.Save(Application.persistentDataPath + "/SaveGame.xml");
    }

    public void LoadGame()
    {
        if (_playerInstance == null)
        {
            Debug.LogError("LoadGame failed: _playerInstance is null!");
            return;
        }

        if (saveManager == null)
        {
            saveManager = GetComponent<LoadSaveManager>();
            if (saveManager == null)
            {
                Debug.LogError("LoadGame failed: saveManager component is missing!");
                return;
            }
        }

        if (saveManager == null) saveManager = GetComponent<LoadSaveManager>();

        saveManager.Load(Application.persistentDataPath + "/SaveGame.xml");

        var data = saveManager.gameState.playerData.transformData;

        Vector3 pos = new Vector3(data.posx, data.posy, data.posz);
        Quaternion rot = Quaternion.Euler(data.rotx, data.roty, data.rotz);
        _playerInstance.transform.localScale = new Vector3(data.scaleX, data.scaleY, data.scaleZ);

        _playerInstance.ResetPlayerState(pos, rot);

        Lives = saveManager.gameState.playerData.health;
    }



    //void Respawn()
    //{
    //    _playerInstance.transform.position = currentCheckpoint.position;
    //}

}