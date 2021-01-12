using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReader : MonoBehaviour
{

	[SerializeField]
	bool activated = true;
	bool currentActivated = true;

	[SerializeField]
	CardreaderColor color = CardreaderColor.Disabled;
	CardreaderColor currentColor = CardreaderColor.Disabled;

	[SerializeField]
	SpriteRenderer StatusIndicator = null;

	[SerializeField]
	SpriteRenderer ColorIndicator = null;

	Door door = null;

	void Start()
	{
		UpdateColor();
	}

	void Update()
	{
		if (door != null) {
			if (door.IsOpen()) {
				activated = true;
			} else {
				activated = false;
			}
		}
		
		if ((activated != currentActivated) || (color != currentColor))
		{
			UpdateColor();
		}
	}


	/// <summary>
	/// For now, when a reader is hit, check door
	/// </summary>
	/// <param name="collision"></param>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (activated == true) {
			return;
		}
		
		if (collision.CompareTag("Player"))
		{
			if (color == CardreaderColor.Disabled)
			{
				door.Open();
				this.SetStatus(true);
			}
			else if (collision.gameObject.GetComponent<Character>().HasKey(color))
			{
				LevelManager.emit("PlayerTriedOpeningDoorSuccesfull");
				door.Open();
				this.SetStatus(true);
			} else {
				LevelManager.emit("PlayerTriedOpeningDoorButWasLocked", door?.room?.name);
			}
		}
	}

	/// <summary>
	/// Let parent assign a door to this reader
	/// </summary>
	/// <param name="door">The door that this reader must control</param>
	public void AssignDoor(Door door)
	{
		this.door = door;
	}

	/// <summary>
	/// Update the visual appearance of the cardreader
	/// Gets called by Update() when needed
	/// </summary>
	void UpdateColor()
	{
		// Update Status indicator
		if (activated)
		{
			StatusIndicator.color = ColorZughy.green;
			currentActivated = activated;
		}
		else
		{
			StatusIndicator.color = ColorZughy.red;
		}
		currentActivated = activated;


		// Update Color Indicator
		switch (color)
		{
			case CardreaderColor.Blue:
				ColorIndicator.color = ColorZughy.cyan;
				break;
			case CardreaderColor.Purple:
				ColorIndicator.color = ColorZughy.purple;
				break;
			case CardreaderColor.Yellow:
				ColorIndicator.color = ColorZughy.yellow;
				break;
			default:
				ColorIndicator.color = ColorZughy.grey;
				break;
		}
		currentColor = color;
	}

	/// <summary>
	/// Sets color of card by CardreaderColor
	/// </summary>
	/// <param name="color">The color to set to, will be updated directly</param>
	public void SetColor(CardreaderColor color)
	{
		this.color = color;
	}

	/// <summary>
	/// Get current color of reader
	/// </summary>
	/// <returns>Returns current color of reader</returns>
	public CardreaderColor GetColor()
	{
		return color;
	}

	/// <summary>
	/// Sets status (on or off)
	/// </summary>
	/// <param name="status">The status, true = green/active, false = red/active</param>
	public void SetStatus(bool status)
	{
		activated = status;
	}

	/// <summary>
	/// Returns current activation status
	/// </summary>
	/// <returns>Returns current activation status</returns>
	public bool GetStatus()
	{
		return activated;
	}

	public bool OpenDoor(Key key)
	{

		return false;
	}

	public void Hide()
	{
		this.GetComponent<SpriteRenderer>().enabled = false;
		StatusIndicator.enabled = false;
		ColorIndicator.enabled = false;
	}

	public Door getDoor()
	{
		return door;
	}
}

/// <summary>
/// The colors of the cards and readers
/// </summary>
public enum CardreaderColor
{
    Disabled,
    Blue,
    Purple,
    Yellow
}
