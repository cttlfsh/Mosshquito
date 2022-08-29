using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartCanvas : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button exitButton;

    public void OnPlayButtonClicked()
    {
        print("CLICKED PLAY");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void OnOptionsButtonClicked()
    {
        MenuManager.Instance.ActivateOptionsCanvas();
    }

    public void OnCreditsButtonClicked()
    {
        MenuManager.Instance.ActivateCreditsCanvas();
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
