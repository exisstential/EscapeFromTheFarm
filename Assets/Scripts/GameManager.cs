using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas PauseMenuCanvas = null;
    float _currentTimeScale = 1f;

    DataManager dataManagerScript;

    private void Start()
    {
        dataManagerScript = FindObjectOfType<DataManager>();
    }
    public void PauseGame()
    {
        _currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        PauseMenuCanvas.enabled = true;
        dataManagerScript.SaveGame();
    }

    public void ResumeGame()
    {
        Time.timeScale = _currentTimeScale;
        PauseMenuCanvas.enabled = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        int GameSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(GameSceneIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
