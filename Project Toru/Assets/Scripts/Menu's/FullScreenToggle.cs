using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class FullScreenToggle : MonoBehaviour
{	
	[SerializeField]
	TextMesh text = null;
	
	public GameSettings gameSettings;

	void OnEnable()
    {
        gameSettings = new GameSettings();

		
        LoadSettings();
    }
	
    void OnMouseDown()
    {
		text.fontStyle = FontStyle.Normal;
		
		gameSettings.fullscreen = Screen.fullScreen = (text.text == "Enable fullscreen");
		text.text = gameSettings.fullscreen ? "Disable fullscreen" : "Enable fullscreen";
		Debug.Log(gameSettings.fullscreen);
		SaveSettings();
    }
	
	void OnMouseEnter() {
		text.fontStyle = FontStyle.Bold;
	}
	
	void OnMouseExit() {
		text.fontStyle = FontStyle.Normal;
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

        text.text = gameSettings.fullscreen ? "Disable fullscreen" : "Enable fullscreen";
    }
	
	public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }
}