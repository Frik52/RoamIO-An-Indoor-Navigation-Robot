using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene loading

// This script handles Main Menu button actions
public class MainMenu : MonoBehaviour
{
    // Called when the "Play" button is clicked
    public void PlayGame()
    {
        // Load the game scene
        // Replace "interior" with the exact name of your scene in Build Settings
        SceneManager.LoadScene("interior");
    }

    // Called when the "Quit" button is clicked
    public void QuitGame()
    {
        // Exit the application
        // Note: This works only in a built executable, not in the editor
        Application.Quit();
    }
}
****************************************************************************


































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

