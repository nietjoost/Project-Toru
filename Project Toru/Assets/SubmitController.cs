using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SubmitController : MonoBehaviour
{
   
    [SerializeField]
    TextMesh Message = null;
	
	[SerializeField]
    SceneSwitcherSubmit ContinueButtonScript = null;

	[SerializeField]
	InputField inputField;

	void Start() {

		ContinueButtonScript.scene = "MainMenu";

		if (WebRequest.totalTime == 0) {
			Message.text = "You finished the game";
			inputField.gameObject.SetActive(false);
			return;
		}

		Message.text = "You finished the game in " + WebRequest.totalTime + " seconds";

		ContinueButtonScript.webRequest = GetComponent<WebRequest>();
	}
}
