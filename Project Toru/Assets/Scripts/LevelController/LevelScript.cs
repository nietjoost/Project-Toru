using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
	protected DialogueManager dialogueManager = null;
	public PoliceSirenOverlay PoliceSiren = null;
	private Queue<PoliceCar> _PoliceCars = new Queue<PoliceCar>();
	public List<PoliceCar> PoliceCars = new List<PoliceCar>();
	
	protected void SpawnPoliceCar() {
		if (_PoliceCars.Count == 0) {
			return;
		}
		
		PoliceSiren.Activate();
		
		LevelManager.Delay(Random.Range(5, 10), () => {
			try {
				_PoliceCars.Dequeue().Drive();
			} catch {
				// Queue is empty
			}
		});
		
		LevelManager.Condition("CopsTriggered")?.Fullfill();
	}
	
	protected virtual void Awake() {
		LevelManager.setLevel();
		dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
		
		foreach(var policecar in PoliceCars) {
			_PoliceCars.Enqueue(policecar);
		}
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.O))
        {
			WebRequest.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
		
		if (Input.GetKeyDown(KeyCode.I))
        {
			WebRequest.Reset();
            dialogueManager.DebugDisableDialogue = true;
        }

		if (Input.GetKeyDown(KeyCode.Alpha0))
        {
			WebRequest.Reset();
            SceneManager.LoadScene("Level 0 - Tutorial", LoadSceneMode.Single);
        }

		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			WebRequest.Reset();
            SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
        }

		if (Input.GetKeyDown(KeyCode.Alpha2))
        {
			WebRequest.Reset();
            SceneManager.LoadScene("Level 2", LoadSceneMode.Single);
        }
	}
}
