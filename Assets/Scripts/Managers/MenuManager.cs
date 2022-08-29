using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject startCanvas;
    public GameObject optionsCanvas;
    public GameObject creditsCanvas;

    public void ActivateStartCanvas()
    {
        startCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void ActivateOptionsCanvas()
    {
        startCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }
    public void ActivateCreditsCanvas()
    {
        startCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
    }


}
