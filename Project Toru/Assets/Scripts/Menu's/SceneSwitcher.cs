using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
	public string scene = "";
	
	[SerializeField]
	TextMesh text = null;

    void OnMouseDown()
    {
		text.fontStyle = FontStyle.Normal;
		
		if (scene != "")
			SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
	
	void OnMouseEnter() {
		text.fontStyle = FontStyle.Bold;
	}
	
	void OnMouseExit() {
		text.fontStyle = FontStyle.Normal;
	}
}