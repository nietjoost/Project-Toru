using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public Text nameText, dialogueText;
	
	public bool DebugDisableDialogue = false;
	
	[SerializeField]
	public Animator animator = null;

	private Queue<string> sentences = new Queue<string>();
	private Queue<DialogueText> dialogues = new Queue<DialogueText>();
	
	private DialogueText currentDialogue = null;

	void Start()
	{
		if (DebugDisableDialogue) {
			Debug.LogWarning("!!!!! Dialogue is disabled for debugging !!!!!");
		}
		animator = GetComponent<Animator>();
	}

	void Update() {
		if(animator.GetBool("IsOpen") == false)
			return;

		if (Input.GetKeyDown(KeyCode.Space)) {
			DisplayNextSentence();
		}
	}
	
	public void QueueDialogue(DialogueText dialogue) {
		
		dialogues.Enqueue(dialogue);
		
		if (currentDialogue == null) {
			currentDialogue = dialogue;
			StartDialogue();
		}
	}
	
	public void StartDialogue()
	{
		
		if (dialogues.Count == 0) {
			return;
		}
		
		Time.timeScale = 0;
		
		animator.SetBool("IsOpen", true);
		
		DialogueText dialogue = dialogues.Dequeue();
		nameText.text = dialogue.name;	
	
		if (!DebugDisableDialogue) {
			foreach(string s in dialogue.sentences)
			{
				sentences.Enqueue(s);
			}
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if(sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";

		foreach (char l in sentence.ToCharArray())
		{
			dialogueText.text += l;
			yield return null;
		}
	}

	private void EndDialogue()
	{
		sentences.Clear();
		animator.SetBool("IsOpen", false);
		LevelManager.Delay(0.2f, () => {
			StartDialogue();
			DialogueText oldCurrentDialogue = currentDialogue;
			currentDialogue = null;
			oldCurrentDialogue?.callback?.Invoke();
		});
		
		Time.timeScale = 1.0f;
	}
}
