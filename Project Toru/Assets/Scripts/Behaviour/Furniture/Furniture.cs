using Assets.Scripts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Furniture : MonoBehaviour
{
	[SerializeField]
	bool Passable;

	[SerializeField]
	string Description = string.Empty;

	List<Item> items;
	Room Parent;

	void Start()
	{
		items = GetComponentsInChildren<Item>().ToList();
		foreach (Item i in items)
			i.gameObject.SetActive(false);
	}

	
	public void drop()
	{
		foreach (Item i in items)
			i.gameObject.SetActive(true);
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.isTrigger)
		{
			//Known bug: Eventwindow is gone fast, will not click either
			CurrentEventWindow.Current.RemoveEvent(gameObject, collision.GetComponent<Character>());
		}
	}

	public void OnMouseOver()
	{
		if (Character.selectedCharacter != null)
		{
			if (Input.GetMouseButtonDown(1))
			{
				Invoke("setFurnitureTarget", 0.1f);
				return;
			}
		}

		Tint.Apply(this.gameObject);
	}

	public void OnMouseExit()
	{
		Tint.Reset(this.gameObject);
	}

	private void setFurnitureTarget()
	{
		Character.selectedCharacter.GetComponent<ExecutePathFindingPlayable>().targetFurniture = gameObject;
	}
}