using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class WebRequest : MonoBehaviour
{

	public static Int32 timeStart = 0;
	public static Int32 totalTime = 0;

	public static string playerName = "";

	public void setTime() {
		Reset();
		timeStart = getTime();
	}

	public static void Reset() {
		timeStart = 0;
		totalTime = 0;
		playerName = "";
	}

	private Int32 getTime() {
		return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
	}
	
    public void stopTime() {
		if (timeStart == 0) return;

		totalTime = getTime() - timeStart;
		timeStart = 0;
	}

	public void GoUpload() {
		StartCoroutine(Upload());
	}

	static IEnumerator Upload()
	{	
		if (totalTime == 0) yield return 0;
		if (playerName == "") yield return 0;

		WWWForm form = new WWWForm();
		form.AddField("key", "70cd531b-03a2-408f-ba82-956e382cd407");
		form.AddField("name", playerName);
		form.AddField("seconds", (int) totalTime);

		Reset();
		
		using (UnityWebRequest www = UnityWebRequest.Post("https://clyde.ducosebel.nl/api/submit.php", form))
		{
			yield return www.SendWebRequest();
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log("Form upload complete!");
			}
		}
	}
}
