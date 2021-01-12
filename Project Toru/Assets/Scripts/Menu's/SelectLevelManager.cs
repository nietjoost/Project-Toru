using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelManager : MonoBehaviour
{
    public Button mainMenuButton;
    public Button LevelOneButton;
    public Button LevelTwoButton;
    public Button LevelThreeButton;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(delegate 	{ OnLevelClick("MainMenu"); });
        LevelOneButton.onClick.AddListener(delegate 	{ OnLevelClick("Level 0 - Tutorial"); });
        LevelTwoButton.onClick.AddListener(delegate 	{ OnLevelClick("Level 1"); });
        LevelThreeButton.onClick.AddListener(delegate 	{ OnLevelClick("Level 2"); });
    }

    public void OnLevelClick(string scene)
    {
        Debug.Log("Loading" + scene);

        SceneManager.LoadScene(scene);
    }
}
