using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueText
{
	public delegate void DialogueTextCallback();
	
	public string name;

	[TextArea(4,10)]
	public List<string> sentences = new List<string>();
	
	public DialogueTextCallback callback = null;
}
