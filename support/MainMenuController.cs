using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.UI;              // Required if you use UI components (optional here)

// This script handles main menu, pause menu, and game flow
public class MainMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;   // Assign your Pause Menu panel from the Inspector

    private bool isPaused = false;   // Tracks if the game is currently paused

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)   // If game is paused, resume it
            {
                ResumeGame();
            }
            else            // If game is running, pause it
            {
                PauseGame();
            }
        }
    }

    // Resume button function
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f;          // Resume game time
        isPaused = false;             // Update pause state
    }

    // Play button function for starting a new game
    public void PlayGame()
    {
        Time.timeScale = 1f;          // Ensure time is running
        isPaused = false;             // Update pause state
        SceneManager.LoadScene("GameScene"); // Load the main game scene (replace with your scene name)
    }

    // Quit button function
    public void QuitGame()
    {
        Debug.Log("Quit game"); // Logs message in editor
        Application.Quit();      // Closes the game in a build
    }

    // Pause game function
    void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f;         // Freeze game time
        isPaused = true;             // Update pause state
    }
}
***************************************************************





























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

