using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [HideInInspector] public static bool _isPaused = false;
    [HideInInspector] public static bool _isLoseGame = false;

    [SerializeField] GameObject _pauseButtoms;
    [SerializeField] GameObject _loseButtoms;
    [SerializeField] GameObject _hud;
    [SerializeField] Image _fadePanel;
    
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject creditsUI;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "MenuScene")
        {
            _fadePanel.gameObject.SetActive(false);
            _pauseButtoms.SetActive(false);
            _loseButtoms.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _isPaused = false;
            _isLoseGame = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1;
            creditsUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MenuScene")
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_isLoseGame) _isPaused = !_isPaused;

            if (_isPaused == true)
            {
                ViewUI();
                if(_hud != null)_hud.SetActive(false);
                
                _pauseButtoms.SetActive(true);
                if (Input.GetKeyDown(KeyCode.R)) Retry();
            }
            else Resume();
            if (_isLoseGame)
            {
                ViewUI();
                _loseButtoms.SetActive(true);
            }
        }
    }

    private void ViewUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        _fadePanel.gameObject.SetActive(true);
        _fadePanel.color = new Color(0, 0, 0, 0.5f); //opacidad
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _fadePanel.gameObject.SetActive(false);
        _pauseButtoms.SetActive(false);
        if (_hud != null) _hud.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenuScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isPaused = false;
        _isLoseGame = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
    public void GoToScene(string _nextScene)
    {
        int _isInt;
        if (int.TryParse(_nextScene, out _isInt)) //si es numero...
        {
            SceneManager.LoadScene(_isInt);
        }
        else SceneManager.LoadScene(_nextScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        if (menuUI != null) menuUI.SetActive(false);
        if (creditsUI != null) creditsUI.SetActive(true);
    }

    public void HideCredits()
    {
        if (creditsUI != null) creditsUI.SetActive(false);
        if (menuUI != null) menuUI.SetActive(true);
    }

    public void ShowMenu()
    {
        if (menuUI != null) menuUI.SetActive(true);
        if (creditsUI != null) creditsUI.SetActive(false);
    }

    public void HideMenu()
    {
        if (menuUI != null) menuUI.SetActive(false);
        if (creditsUI != null) creditsUI.SetActive(false);
    }
}
