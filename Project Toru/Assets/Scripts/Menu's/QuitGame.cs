using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
	
	void Start() {
		if (Application.platform == RuntimePlatform.WebGLPlayer) {
			this.gameObject.SetActive(false);
		}
	}
	
	void OnMouseDown()
	{
		Application.Quit();
	}
}
