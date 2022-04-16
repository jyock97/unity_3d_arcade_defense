using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum GameMode
{
    UI,
    Gameplay
}

[Serializable]
public class GameSaveData
{
    // Game Info
    public int roundNumber;
    public bool isRoundCooldown;
    public int[] scoreboard;
    public string[] scoreboardNames;
    public int currentScore;
    
    // Player Info
    public int playerLife;
    public int playerMoney;
    public float[] playerPosition;
    
    // Structures Info
    public int nexoLife;
    public float[][,] structuresPosition;
    
    // Enemies Info
    public float[][,] enemiesPosition;
}

public class GameController : MonoBehaviour
{
    private enum GameAction
    {
        NewGame,
        LoadGame,
        None
    }
    
    public GameMode currentGameMode;
    
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] structurePrefabs;
    [SerializeField] private float roundCooldownTime;
    [SerializeField] private int enemiesSpawnOffset;

    private static GameAction _gameAction;
    private int _roundNumber;
    [SerializeField] private int _currentScore;
    private int[] _scoreBoard;
    private string[] _scoreBoardNames;
    [SerializeField] private int _totalEnemies;
    private bool _isGameStarted;
    private bool _isRoundCooldown;

    private float _currentRoundCooldownTime;

    private int[] _enemySpawnAmount;
    private readonly Dictionary<int, int> _fibonacci = new Dictionary<int, int>();

    private GameSaveData _saveData;

    public static GameController Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SceneManager.activeSceneChanged += ChangedActiveScene;
        _gameAction = GameAction.None;
        _saveData = SaveController.LoadGameData();
        
        DontDestroyOnLoad(gameObject);
    }
    
    public void Start()
    {
        Time.timeScale = 0;
        _isGameStarted = false;
        _isRoundCooldown = false;
        _enemySpawnAmount = new int[3];
        enemiesSpawnOffset = 2; // THIS can be a config value

        _scoreBoard = _saveData?.scoreboard ?? new int[10];
        _scoreBoardNames = _saveData?.scoreboardNames ?? new string[10];
    }
    
    //test
    [SerializeField] private bool newGame; 
    [SerializeField] private bool saveGame; 
    [SerializeField] private bool loadGame;
    [SerializeField] private bool gameOver;
    public void Update()
    {
        if (newGame)
        {
            NewGame();
            newGame = false;
        }
        if (saveGame)
        {
            SaveGameData();
            saveGame = false;
        }
        if (loadGame)
        {
            LoadGame();
            loadGame = false;
        }

        if (gameOver)
        {
            GameOver("test");
            gameOver = false;
        }
        
        
        if (_isGameStarted)
        {
            if (_totalEnemies == 0 && !_isRoundCooldown)
            {
                _currentRoundCooldownTime = Time.time + roundCooldownTime;
                _isRoundCooldown = true;
            }

            if (_isRoundCooldown && Time.time > _currentRoundCooldownTime)
            {
                _roundNumber++;
                StartNewRound(_roundNumber);
            }
        }
    }

    public bool CanLoadGame()
    {
        if (_saveData != null)
        {
            return _saveData.currentScore > 0;
        }

        return false;
    }

    public void NewGame()
    {
        _gameAction = GameAction.NewGame;
        currentGameMode = GameMode.Gameplay;
        SceneManager.LoadScene("Main");
    }
    
    public void LoadGame()
    {
        _gameAction = GameAction.LoadGame;
        currentGameMode = GameMode.Gameplay;
        SceneManager.LoadScene("Main");
    }
    
    public void EndGame()
    {
        // Clear all data and save game
        StopScore();
        _gameAction = GameAction.None;
        currentGameMode = GameMode.UI;
        SaveGameData();
        SceneManager.LoadScene("MainMenu");
    }
    
    public void GameOver(string name)
    {
        SaveNewScore(name);
        _currentScore = 0;
        SaveGameData();
        currentGameMode = GameMode.UI;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReduceTotalEnemies()
    {
        _totalEnemies--;
    }

    public void StopScore()
    {
        StopAllCoroutines();
    }
    
    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (next.name.Equals("Main"))
        {
            if (_gameAction == GameAction.NewGame)
            {
                _roundNumber = 0;
                Time.timeScale = 1;
                _isGameStarted = true;
                StartNewRound(_roundNumber);
                StartCoroutine(IncreaseScore());
                
                PlayerController playerController = FindObjectOfType<PlayerController>();
                playerController.ResetLife();
                GameObject.FindGameObjectWithTag(TagsLayers.NexoTag).GetComponent<EntityController>().ResetLife();

            } 
            else if (_gameAction == GameAction.LoadGame)
            {
                _roundNumber = _saveData.roundNumber;
                _isRoundCooldown = _saveData.isRoundCooldown;
                _currentScore = _saveData.currentScore;
            
                PlayerController playerController = FindObjectOfType<PlayerController>();

                playerController.SetLife(_saveData.playerLife);
                playerController.SetMoney(_saveData.playerMoney);
                playerController.transform.position = 
                    new Vector3(_saveData.playerPosition[0], _saveData.playerPosition[1], _saveData.playerPosition[2]);
            
                GameObject.FindGameObjectWithTag(TagsLayers.NexoTag).GetComponent<EntityController>().SetLife(_saveData.nexoLife);
            
                // Restore Structures
                RestoreGameObjects(structurePrefabs, _saveData.structuresPosition);
            
                // Restore Enemies
                RestoreGameObjects(enemyPrefabs, _saveData.enemiesPosition);
                for (int i = 0; i < _saveData.enemiesPosition.Length; i++)
                {
                    if (_saveData.enemiesPosition[i] != null)
                    {
                        _totalEnemies += _saveData.enemiesPosition[i].GetLength(0);
                    }
                }

                Time.timeScale = 1;
                _isGameStarted = true;
            }
        }
    }

    private void SaveGameData()
    {
        _saveData = new GameSaveData();
        _saveData.roundNumber = _roundNumber;
        _saveData.isRoundCooldown = _isRoundCooldown;
        _saveData.scoreboard = _scoreBoard;
        _saveData.scoreboardNames = _scoreBoardNames;
        _saveData.currentScore = _currentScore;

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            Vector3 playerPosition = playerController.transform.position;
            _saveData.playerLife = playerController.GetLife();
            _saveData.playerMoney = playerController.GetMoney();
            _saveData.playerPosition = new float[3] {playerPosition.x, playerPosition.y, playerPosition.z};
        }

        GameObject nexo = GameObject.FindGameObjectWithTag(TagsLayers.NexoTag);
        if (nexo != null)
        {
            _saveData.nexoLife = nexo.GetComponent<EntityController>().GetLife();
        }
        
        GameObject[] spikes = GameObject.FindGameObjectsWithTag(TagsLayers.SpikesTag);
        GameObject[] torrets = GameObject.FindGameObjectsWithTag(TagsLayers.TorretTag);
        _saveData.structuresPosition = new float[][,]
        {
            GetConvertedPositions(spikes),
            GetConvertedPositions(torrets)
        };

        GameObject[] enemies01 = GameObject.FindGameObjectsWithTag(TagsLayers.Enemy01Tag);
        GameObject[] enemies02 = GameObject.FindGameObjectsWithTag(TagsLayers.Enemy02Tag);
        GameObject[] enemies03 = GameObject.FindGameObjectsWithTag(TagsLayers.Enemy03Tag);
        _saveData.enemiesPosition = new float[][,]
        {
            GetConvertedPositions(enemies01),
            GetConvertedPositions(enemies02),
            GetConvertedPositions(enemies03)
        };

        SaveController.SaveGameData(_saveData);
    }

    private void StartNewRound(int round)
    {
        for (int i = 0; i < _enemySpawnAmount.Length; i++)
        {
            _enemySpawnAmount[i] = Fibonacci(round);
            round -= enemiesSpawnOffset;
            if (round < 0)
            {
                break;
            }
        }
        
        Debug.Log($"Round {_roundNumber}");
        
        for (int i = 0; i < _enemySpawnAmount.Length; i++)
        {
            int enemiesToSpawn = _enemySpawnAmount[i];
            while (enemiesToSpawn > 0)
            {
                Vector2 randomPosition = Random.insideUnitCircle * 60;
                Vector3 position = new Vector3(randomPosition.x, 0, randomPosition.y);
                GameObject go = Instantiate(enemyPrefabs[i]);
                go.transform.position = position;
                go.GetComponent<EnemyController>().ResetLife();
                _totalEnemies++;
                
                enemiesToSpawn--;
            }
        }
        _isRoundCooldown = false;
    }

    private IEnumerator IncreaseScore()
    {
        while (_isGameStarted)
        {
            yield return new WaitForSeconds(1);
            _currentScore++;
        }
    }

    private float[,] GetConvertedPositions(GameObject[] objs)
    {
        if (objs.Length == 0)
        {
            return null;
        }
        
        float[,] arr = new float[objs.Length, 3];
        for (int i = 0; i < objs.Length; i++)
        {
            Vector3 structurePosition = objs[i].transform.position;
            arr[i, 0] = structurePosition.x;
            arr[i, 1] = structurePosition.y;
            arr[i, 2] = structurePosition.z;
        }

        return arr;
    }

    private void RestoreGameObjects(GameObject[] objectPrefabs, float[][,] objsPosition)
    {
        for (int i = 0; i < objsPosition.Length; i++)
        {
            if (objsPosition[i] != null)
            {
                for (int j = 0; j < objsPosition[i].GetLength(0); j++)
                {
                    GameObject go = Instantiate(objectPrefabs[i]);
                    float[,] enemiesPosition = objsPosition[i];
                    go.transform.position = new Vector3(enemiesPosition[j,0], enemiesPosition[j,1],enemiesPosition[j,2]);
                }
            }
        }
    }

    private void SaveNewScore(string scoreName)
    {
        if (scoreName.Equals(""))
        {
            scoreName = "AAA";
        }
        
        int scoreToMatch = _currentScore;
        string nameToMatch = scoreName.ToUpper();
        
        if (nameToMatch.Length > 3)
        {
            nameToMatch = nameToMatch.Substring(0, 3);
        }
        
        for (int i = 0; i < _scoreBoard.Length; i++)
        {
            if (scoreToMatch > _scoreBoard[i])
            {
                int tempScore = _scoreBoard[i];
                _scoreBoard[i] = scoreToMatch;
                scoreToMatch = tempScore;

                string tempName = _scoreBoardNames[i];
                _scoreBoardNames[i] = nameToMatch;
                nameToMatch = tempName;
            }
        }
    }

    private int Fibonacci(int n)
    {
        if (n == 0) return 3; // Skip first 4 fibonacci numbers
        if(n == 1) return 5;

        if (_fibonacci.ContainsKey(n))
        {
            return _fibonacci[n];
        }

        int f = Fibonacci(n - 1) + Fibonacci(n - 2);
        _fibonacci[n] = f;
        return f;
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }

    public int[] GetSavedScoreBoard()
    {
        return _saveData.scoreboard;
    }
    
    public string[] GetSavedScoreBoardNames()
    {
        return _saveData.scoreboardNames;
    }

    public int GetRoundNumber()
    {
        return _roundNumber + 1;
    }

    public float GetTimeToNextRound()
    {
        float time = _currentRoundCooldownTime - Time.time;
        if (time < 0)
        {
            return 0;
        }

        return time;
    }
}
