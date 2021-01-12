using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcherSubmit : MonoBehaviour
{
	public string scene = "";
	
	[SerializeField]
	TextMesh text = null;

	public WebRequest webRequest = null;

	public InputField input;

    void OnMouseDown()
    {
		text.fontStyle = FontStyle.Normal;
		WebRequest.playerName = input.text;
		
		if (input.IsActive()) { 	
			webRequest.GoUpload();
		}

		StartCoroutine(Segue());		
    }

	private IEnumerator Segue() {
		
		yield return new WaitForSeconds(1f);
		
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}
	
	void OnMouseEnter() {
		text.fontStyle = FontStyle.Bold;
	}
	
	void OnMouseExit() {
		text.fontStyle = FontStyle.Normal;
	}
}