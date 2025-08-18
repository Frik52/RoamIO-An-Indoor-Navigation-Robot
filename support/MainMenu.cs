using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called when Play button is clicked
    public void PlayGame()
    {
        // Load your game scene (replace "GameScene" with actual name)
        SceneManager.LoadScene("interior");
    }

    // Optional: quit button function
    public void QuitGame()
    {
        Application.Quit();
    }
}
