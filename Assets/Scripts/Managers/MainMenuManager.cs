using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [Header("General")]
    public GameObject gameUI;
    public GameObject victoryUI;
    public GameObject controllerDisconnectedUI;
    public GameObject silenceText;
    public List<GameObject> roundsText = new List<GameObject>();

    [Header("VictoryUI")]
    public TMP_Text roundText;

    [Header("Mosquito")]
    public Image mosquitosLifeImg;
    public Image mosquitovisibilityImg;
    public List<Image> mosquitoRounds = new List<Image>();

    [Header("Player")]
    public Image playerMadnessImg;
    public List<Image> playerRounds = new List<Image>();

    public bool isGamePaused;
    private bool isGameOver = false;

    #region GENERAL_UI

    public void ContinueButtonClicked()
    {
        victoryUI.SetActive(false);
        if (!isGameOver)
        {
            isGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            gameUI.SetActive(true);
            SceneManager.LoadScene(0);
        }
    }

    public void ResetRoundsImages()
    {
        foreach (Image round in playerRounds)
        {
            round.fillAmount = 0;
        }

        foreach (Image round in mosquitoRounds)
        {
            round.fillAmount = 0;
        }
    }

    public void HideRoundTexts()
    {
        foreach (GameObject round in roundsText)
        {
            round.SetActive(false);
        }
        silenceText.SetActive(false);

    }


    public void AssignRound(string winner, int roundNumber)
    {
        //gameUI.SetActive(false);
        if (winner == "Player")
        {
            //print(roundNumber);
            playerRounds[roundNumber].fillAmount = 1;
            GameManager.Instance.playerRoundWon += 1;
            CheckEndGame("Player", GameManager.Instance.player.roundWon);

        }
        else
        {
            mosquitoRounds[roundNumber].fillAmount = 1;
            GameManager.Instance.mosquitoRoundWon += 1;
            CheckEndGame("Mosquito", GameManager.Instance.mosquito.roundWon);
        }
        victoryUI.SetActive(true);
    }

    public void CheckEndGame(string player, int roundsWon)
    {
        if (roundsWon == 3)
        {
            roundText.text = $"The WINNER is {player}";
            isGameOver = true;
        }
        else
        {
            roundText.text = $"Round {roundsWon}: {player}";
        }
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
        print(Time.timeScale);
        isGamePaused = true;
    }

    private void PauseTime()
    {
        Time.timeScale = 0f;
        print(Time.timeScale);
        isGamePaused = true;
    }

    public void ToggleResumePause()
    {
        if (isGamePaused)
        {
            ResumeTime();
        }
        else
        {
            PauseTime();
        }
    }

    public void OnControllerLost()
    {
        PauseTime();
        gameUI.SetActive(false);
        controllerDisconnectedUI.SetActive(false);
    }

    public void OnControllerRejoined()
    {
        ResumeTime();
        gameUI.SetActive(true);
        controllerDisconnectedUI.SetActive(false);
    }
    #endregion

    #region MOSQUITO_UI

    public void SetMosquitosLife()
    {
        mosquitosLifeImg.fillAmount = GameManager.Instance.mosquito.life;
    }

    public void UpdateMosquitosLife(float life)
    {
        mosquitosLifeImg.fillAmount = life / 100f;
    }

    public void SetMosquitoVisibility(float totalVisibility)
    {
        mosquitovisibilityImg.fillAmount = totalVisibility / GameManager.Instance.mosquito.visibilityDuration;
    }

    public void UpdateMosquitoVisibility(float remainedVisibility)
    {
        mosquitovisibilityImg.fillAmount = remainedVisibility / GameManager.Instance.mosquito.visibilityDuration;
    }
    #endregion

    #region PLAYER_UI

    public void SetPlayerMadness()
    {
        playerMadnessImg.fillAmount = GameManager.Instance.player.maxMadness;
    }
    public void UpdatePlayerMadnessSlider(float madness)
    {
        playerMadnessImg.fillAmount = (GameManager.Instance.player.maxMadness - madness)/ GameManager.Instance.player.maxMadness;
    }

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        
    }

    private void Start()
    {
        ResetRoundsImages();
        HideRoundTexts();
    }


}
