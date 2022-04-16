using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the UI behaviour
/// Controls the Highlight of the Buttons
/// Controls the UI re-selection when loosing selection by keyboard
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField] private bool canPauseGame;
    [SerializeField] private GameObject loadGameBtn;
    [SerializeField] private GameObject _scoreBoardContents;
    [SerializeField] private GameObject _scoreBoardTemplate;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_InputField _scoreName;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text playerMoney;
    [SerializeField] private TMP_Text roundNumber;
    [SerializeField] private TMP_Text nextRoundTime;
    
    [SerializeField] private Sprite startMenuButtonImage;
    [SerializeField] private Sprite startMenuButtonHighlightImage;
    [SerializeField] private Sprite closeButtonImage;
    [SerializeField] private Sprite closeButtonHighlightImage;
    [SerializeField] private GameObject firstSelectedObj;
    [SerializeField] private GameObject[] selectables;
    [SerializeField] private GameObject[] closeBtnSelectables;
    [SerializeField] private TMP_Dropdown dropdownResolutions;
    [SerializeField] private TMP_Dropdown dropdownFullScreen;
    
    private EventSystem _eventSystem;
    private Resolution[] _resolutions;
    private int _currentResolution;
    private FullScreenMode[] _fullScreenModes;
    private int _currentFullScreenMode;
    private PlayerController _playerController;
    
    private void Start()
    {
        _eventSystem = EventSystem.current;
        _eventSystem.SetSelectedGameObject(firstSelectedObj);
        _playerController = FindObjectOfType<PlayerController>();


        if (dropdownResolutions != null) SetResolutions();
        if (dropdownFullScreen != null) SetFullScreenModes();

        if (GameController.Instance.CanLoadGame() && loadGameBtn != null)
        {
            loadGameBtn.GetComponent<Button>().interactable = true;
        }

        if (_scoreBoardContents != null)
        {
            LoadScoreBoard();
        }
        
        if (_score != null)
        {
            _score.text = GameController.Instance.GetCurrentScore().ToString();
        }
    }

    private void Update()
    {
        if (GameController.Instance.currentGameMode == GameMode.UI)
        {
            if (_eventSystem.currentSelectedGameObject == null &&
                Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            {
                _eventSystem.SetSelectedGameObject(firstSelectedObj);
            }

            foreach (GameObject selectableObj in selectables)
            {
                SelectDeselectButton(selectableObj);
            }
            foreach (GameObject selectableObj in closeBtnSelectables)
            {
                SelectDeselectCloseButton(selectableObj);
            }
            
            if (canPauseGame && Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
        }
        else if (GameController.Instance.currentGameMode == GameMode.Gameplay)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_playerController.isBuilding)
                {
                    Pause();
                }
            }

            if (roundNumber != null)
            {
                roundNumber.text = GameController.Instance.GetRoundNumber().ToString();
            }

            if (nextRoundTime != null)
            {
                nextRoundTime.text = GameController.Instance.GetTimeToNextRound().ToString("0.00");
            }
            
            if (_score != null)
            {
                _score.text = GameController.Instance.GetCurrentScore().ToString();
            }
        
            if (playerMoney != null)
            {
                playerMoney.text = _playerController.GetMoney().ToString();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameController.Instance.currentGameMode = GameMode.UI;
        if (pauseMenu != null)
        {
            pauseMenu.GetComponent<Animator>().SetTrigger("show");
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameController.Instance.currentGameMode = GameMode.Gameplay;
        if (pauseMenu != null)
        {
            pauseMenu.GetComponent<Animator>().SetTrigger("hide");
        }
    }

    public void NewGame()
    {
        GameController.Instance.NewGame();
    }

    public void LoadGame()
    {
        GameController.Instance.LoadGame();
    }

    public void EndGame()
    {
        GameController.Instance.EndGame();

    }
    
    public void GameOver()
    {
        GameController.Instance.GameOver(_scoreName.text);
    }

    private void SelectDeselectButton(GameObject selectableObj)
    {
        if (_eventSystem.currentSelectedGameObject == selectableObj)
        {
            SetSelectableMenuSprite(selectableObj, startMenuButtonHighlightImage);
        }
        else
        {
            SetSelectableMenuSprite(selectableObj, startMenuButtonImage);
        }
    }
    
    private void SelectDeselectCloseButton(GameObject selectableObj)
    {
        if (_eventSystem.currentSelectedGameObject == selectableObj)
        {
            SetSelectableMenuSprite(selectableObj, closeButtonHighlightImage);
        }
        else
        {
            SetSelectableMenuSprite(selectableObj, closeButtonImage);
        }
    }

    public void HoverSelect(GameObject obj)
    {
        _eventSystem.SetSelectedGameObject(obj);
    }

    private void SetSelectableMenuSprite(GameObject selectableObj, Sprite sprite)
    {
        Image image = selectableObj.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
        }
    }

    public void SetFirstSelectableObject(GameObject selectableObj)
    {
        firstSelectedObj = selectableObj;
    }

    public void LoadScene(String sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void FlipActiveObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    
    
    //===== Options =====s
    public void ApplyOptions()
    {
        int resolutionSelection = dropdownResolutions.value;
        int fullScreenSelection = dropdownFullScreen.value;
        Screen.SetResolution(_resolutions[resolutionSelection].width,
            _resolutions[resolutionSelection].height,
            _fullScreenModes[fullScreenSelection],
            _resolutions[resolutionSelection].refreshRate);
    }
    
    private void SetResolutions()
    {
        _resolutions = Screen.resolutions;
        Array.Reverse(_resolutions);
        _currentResolution = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (Screen.width == _resolutions[i].width &&
                Screen.height == _resolutions[i].height &&
                (Screen.currentResolution.refreshRate == _resolutions[i].refreshRate || Screen.currentResolution.refreshRate == _resolutions[i].refreshRate + 1))
            {
                _currentResolution = i;
            }

            dropdownResolutions.options.Add(new TMP_Dropdown.OptionData(_resolutions[i].ToString()));
        }

        dropdownResolutions.value = _currentResolution;
    }

    private void SetFullScreenModes()
    {
        _fullScreenModes = new[]
        {
            FullScreenMode.Windowed,
            FullScreenMode.MaximizedWindow,
            FullScreenMode.ExclusiveFullScreen,
            FullScreenMode.FullScreenWindow
        };
        _currentFullScreenMode = 0;
        
        
        for (int i = 0; i < _fullScreenModes.Length; i++)
        {
            if (Screen.fullScreenMode.Equals(_fullScreenModes[i]))
            {
                _currentFullScreenMode = i;
            }

            dropdownFullScreen.options.Add(new TMP_Dropdown.OptionData(_fullScreenModes[i].ToString()));
        }

        dropdownFullScreen.value = _currentFullScreenMode;
    }

    private void LoadScoreBoard()
    {
        int[] scoreBoard = GameController.Instance.GetSavedScoreBoard();
        string[] scoreBoardNames = GameController.Instance.GetSavedScoreBoardNames();
        for (int i = 0; i < scoreBoard.Length; i++)
        {
            if (scoreBoard[i] > 0)
            {
                GameObject go = Instantiate(_scoreBoardTemplate, _scoreBoardContents.transform);
                go.SetActive(true);
                go.GetComponent<TMP_Text>().text = $"{scoreBoardNames[i]} - {scoreBoard[i].ToString()}";
            }
        }
    }
}
