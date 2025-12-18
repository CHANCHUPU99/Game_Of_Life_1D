using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToPlayScene : MonoBehaviour
{
    public void goToPlayScene() {
        SceneManager.LoadScene("SampleScene");
    }

    public void exitGame() {
        Application.Quit();
    }

    public void goToMainMenu() {
        SceneManager.LoadScene("MainMenu");

    }
    public void goToInstructions() {
        SceneManager.LoadScene("Instructions");
    }
}
