using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenSwitch : MonoBehaviour
{
	void Start()
	{
		Invoke("Switch", 5);
	}

	void Switch()
	{
		Application.Quit();
	}
}
