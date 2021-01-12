using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Options
{
	static class CurrentEventWindow
	{
		public static EventWindow Current;
	}

	public enum EventTextType
	{
		options,
		characters,
		extraOption,
		result
	}

	[RequireComponent(typeof(TextMeshProUGUI))]
	[RequireComponent(typeof(BoxCollider2D))]
	public class EventWindow : MonoBehaviour, IPointerClickHandler
	{
		private string ResultMessage, DoAnotherActionString = "Do you want to do anything else?" + Environment.NewLine + " <link>yes</link>" + Environment.NewLine + " <link>no</link>";

		List<Event> EventQueue;
		TextMeshProUGUI TMP;
		EventTextType TextType;
		int OptionIndex, ActorCount, HoveringOver = -1;
		GameObject Panel;
		Color UnSelected = Color.black, Selected = Color.grey;

		public void Start()
		{
			EventQueue = new List<Event>();
			gameObject.SetActive(false);

			Panel = gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject;
			Panel.SetActive(false);
			TMP = GetComponent<TextMeshProUGUI>();
			CurrentEventWindow.Current = this;
		}

		public void LateUpdate()
		{
			int LinkIndex = TMP_TextUtilities.FindIntersectingLink(TMP, Input.mousePosition, null); //Camera.main);
			//if (LinkIndex == HoveringOver)
			//	return;

			if(HoveringOver != -1)
				ChangeLinkColor(HoveringOver, UnSelected);
			if(LinkIndex != -1)
				ChangeLinkColor(LinkIndex, Selected);
			HoveringOver = LinkIndex;
		}

		public void AddEvent(Event NewEvent)
		{
			foreach (var e in EventQueue)
				if (e.Merge(NewEvent))
				{
					DisplayNextOptions();
					return;
				}
			EventQueue.Add(NewEvent);
			DisplayNextOptions();
		}

		public void RemoveEvent(GameObject g, Character c)
		{
			int temp;
			for (int i = 0; i < EventQueue.Count; i++) {
				temp = EventQueue[i].Remove(g, c);
				if (temp > 0)
					break;
				if (temp == 0)
					EventQueue.RemoveAt(i);
			}
			DisplayNextOptions();
		}

		public void RemoveEvent(Event E)
		{
			EventQueue.Remove(E);
			DisplayNextOptions();
		}

		public void RemoveEvent()
		{
			EventQueue.RemoveAt(0);
			DisplayNextOptions();
		}

		void DisplayNextOptions()
		{
			if (EventQueue.Count == 0)
			{
				gameObject.SetActive(false);
				Panel.SetActive(false);
				Time.timeScale = 1.0f;
				return;
			}

			gameObject.SetActive(true);
			Panel.SetActive(true);
			EventQueue = EventQueue.OrderBy(o => o.priority).ToList();
			
			// slows down time depending on priority in 4 steps from full speed to 20 percent speed
			Time.timeScale = 0.2f + (float)(0.8f * Math.Floor(Math.Min(EventQueue[0].priority, 4) / 4f));

			TMP.text = EventQueue[0].GetOptionText();
			if (TMP.text == null)
			{
				EventQueue.RemoveAt(0);
				DisplayNextOptions();
			}
			TextType = EventTextType.options;
		}

		private void BuildResult()
		{
			TMP.text = ResultMessage + Environment.NewLine + " <link>continue</link>";
		}
		public void ChangeLinkColor(int LinkIndex, Color colorForLinkAndVert)
		{
			var linkInfo = TMP.textInfo.linkInfo[LinkIndex];

			for (int i = 0; i < linkInfo.linkTextLength; i++)
			{ // for each character in the link string
				int characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // the character index into the entire text
				var charInfo = TMP.textInfo.characterInfo[characterIndex];
				int meshIndex = charInfo.materialReferenceIndex; // Get the index of the material / sub text object used by this character.
				int vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

				Color32[] vertexColors = TMP.textInfo.meshInfo[meshIndex].colors32; // the colors for this character

				if (charInfo.isVisible)
				{
					vertexColors[vertexIndex + 0] = colorForLinkAndVert;
					vertexColors[vertexIndex + 1] = colorForLinkAndVert;
					vertexColors[vertexIndex + 2] = colorForLinkAndVert;
					vertexColors[vertexIndex + 3] = colorForLinkAndVert;
				}
			}

			// Update Geometry
			TMP.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			int LinkIndex = TMP_TextUtilities.FindIntersectingLink(TMP, Input.mousePosition, null); //Camera.main);
			if (LinkIndex == -1)
			{
				return;
			}

			HoveringOver = -1;

			switch (TextType)
			{
				case EventTextType.options:
					ActorCount = EventQueue[0].ActivateOption(LinkIndex, ref ResultMessage);
					if (ActorCount == 0)
						goto default;

					TMP.text = EventQueue[0].GetActorText();
					TextType = EventTextType.characters;
					OptionIndex = LinkIndex;
					break;
				case EventTextType.characters:
					EventQueue[0].ActivateOption(OptionIndex, LinkIndex, ref ResultMessage);
					ActorCount--;
					if(ActorCount == 0)
						goto default;

					TMP.text = DoAnotherActionString;
					TextType = EventTextType.extraOption;
					break;
				case EventTextType.extraOption:
					if (LinkIndex == 1)
						goto default;

					DisplayNextOptions();
					break;
				case EventTextType.result:
					ResultMessage = null;
					goto default;

				default:
					if (ResultMessage != null)
					{
						BuildResult();
						TextType = EventTextType.result;
						break;
					}

					EventQueue.RemoveAt(0);

					DisplayNextOptions();

					break;
			}
		}
	}
}
