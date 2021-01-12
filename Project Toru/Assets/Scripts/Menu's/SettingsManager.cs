using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Button mainMenuButton;

    public GameSettings gameSettings;

    void OnEnable()
    {
        gameSettings = new GameSettings();
		
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFSToggle(); });
        mainMenuButton?.onClick.AddListener(delegate { OnCancelClick(); });
		
        LoadSettings();
    }

    //Full screen toggle
    public void OnFSToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        if(!File.Exists(Application.persistentDataPath + "/gamesettings.json"))
        {
            gameSettings.fullscreen = true;
			
            string jsonData = JsonUtility.ToJson(gameSettings, true);
            File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
        }

        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        fullscreenToggle.isOn = gameSettings.fullscreen;
    }

    public void OnCancelClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
