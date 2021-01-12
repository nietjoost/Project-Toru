using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuPrefab;
    public GameObject settingsPrefab;
    private bool paused = false;
    public Button resumeButton;
    public Button howtoplayButton;
    public Button quitButton;
    public Button backButton;

    public void Start()
    {
        pauseMenuPrefab.SetActive(false);
        settingsPrefab.SetActive(false);

        resumeButton.onClick.AddListener(delegate { OnResumeClick(); });
        howtoplayButton.onClick.AddListener(delegate { OnHowToPlayClick(); });
        quitButton.onClick.AddListener(delegate { OnQuitClick(); });
        backButton.onClick.AddListener(delegate { OnBackClick(); });
    }

    public void OpenMenu()
    {
        if(paused)
        {
            Time.timeScale = 0;
            pauseMenuPrefab.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuPrefab.SetActive(false);
        }
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!settingsPrefab.activeInHierarchy)
            {
                if (paused)
                {
                    paused = false;
                }
                else
                {
                    paused = true;
                }
                OpenMenu();
            }
            else
            {
                OnBackClick();
            }
        }

    }

    public void OnResumeClick()
    {
        paused = false;
        OpenMenu();
    }

    public void OnHowToPlayClick()
    {
        pauseMenuPrefab.SetActive(false);

        settingsPrefab.SetActive(true);
    }

    public void OnQuitClick()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnBackClick()
    {
        settingsPrefab.SetActive(false);
        pauseMenuPrefab.SetActive(true);
    }

}
