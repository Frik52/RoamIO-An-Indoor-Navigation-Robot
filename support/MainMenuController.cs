using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;   // Assign your PauseMenu panel here

    private bool isPaused = false;

    void Update()
    {
        // Toggle menu with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Resume button
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume time
        isPaused = false;
    }

    // Play button (start new game session)
    public void PlayGame()
    {
        Time.timeScale = 1f;  // Ensure time is running
        isPaused = false;
        SceneManager.LoadScene("GameScene"); // Change to your game scene name
    }

    // Exit button
    public void QuitGame()
    {
        Debug.Log("Quit game"); // Works only in Editor log
        Application.Quit();      // Works in build
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Pause time
        isPaused = true;
    }
}
