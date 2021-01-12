using System;
using Assets.Scripts.Options;
using System.Collections.Generic;
using UnityEngine;

public enum Skills
{
	hacker
}

public class Character : MonoBehaviour
{
	[SerializeField]
	public float speed;
	
	private Rigidbody2D myRigidbody;

	[NonSerialized]
	public Vector3 change;

    [NonSerialized]
	public Animator animator;

	[NonSerialized]
	public Inventory inventory;
	
	public Room currentRoom;

	public static Character selectedCharacter;

	[SerializeField]
	public float MaxWeight;

	[SerializeField]
	private GameObject textBox = null;

	[SerializeField]
	public Weapon weapon;

	bool weaponKeyRelease = true;

	public List<Skills> skills = new List<Skills>();

	private bool outline = false;

	[NonSerialized]
	public bool surrendering = false;
	// Start is called before the first frame update
	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		inventory = new Inventory(MaxWeight);

		weapon = GetComponentInChildren<Weapon>();
		if (weapon != null)
		{
			weapon.weaponHolder = this.gameObject;
			weapon.gameObject.transform.position = transform.position + new Vector3(.3f, -.3f);
		}
	}

	// Update is called once per frame
	void Update()
	{	
		if (surrendering) return;

		if (this.Equals(selectedCharacter) && !surrendering)
		{
			if(weapon != null) {
				if (Input.GetKey(KeyCode.F))
				{
					if (weaponKeyRelease)
						if (currentRoom != null) {
							LevelManager.emit("PlayerHasUsedGun", currentRoom.gameObject);
						}
						
					
					weaponKeyRelease = false;
					weapon?.Shoot();
				} else {
					weaponKeyRelease = true;
				}
                if (Input.GetKeyDown(KeyCode.H))
                {
					if (weapon.weaponOut)
					{
						weapon.HideGun();
					}
					else
					{
						weapon.RevealGun();
					}
                }
			}
		}

		if(weapon != null)
		{
			FlipFirePoint();
		}
	}

	public bool HasKey(CardreaderColor color)
	{
		foreach (Item i in inventory.getItemsList())
		{
			if (i is Key && ((Key)i).color == color)
				return true;
		}
		return false;
	}

	void OnMouseDown()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (selectedCharacter != null)
			{
				selectedCharacter.transform.Find("SelectedTriangle").gameObject.GetComponent<SpriteRenderer>().enabled = false;
			}

			selectedCharacter = this;
			
			LevelManager.emit("CharacterHasBeenSelected");

			inventory.UpdateUI();

			transform.Find("SelectedTriangle").gameObject.GetComponent<SpriteRenderer>().enabled = true;
		}
	}

	protected virtual void FlipFirePoint()
	{
		GameObject firePoint = weapon.gameObject;

		if (animator.GetFloat("moveX") > 0.1)
		{
			firePoint.transform.rotation = Quaternion.Euler(0, 0, 0);
			firePoint.transform.position = transform.position + new Vector3(.3f, -.3f);
			firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
		}

		if (animator.GetFloat("moveX") < -0.1)
		{
			firePoint.transform.rotation = Quaternion.Euler(0, 180, 0);
			firePoint.transform.position = transform.position + new Vector3(-.3f, -.4f);
			firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Room"))

		{	
			Room oldRoom = currentRoom;
			currentRoom = other.gameObject.GetComponent<Room>();
			if (currentRoom != oldRoom)
				LevelManager.emit("CharacterIsInRoom", this.gameObject);
		}
	}

	public void Say(string text)
    {
        textBox.GetComponent<TextMesh>().text = text;
        textBox.SetActive(true);
		textBox.GetComponent<Renderer>().sortingLayerName = "UI";
        Invoke("disableTextBox", 3);
    }

	private void disableTextBox()
    {
        textBox.SetActive(false);
	}

	public void Surrender() {
		weapon.HideGun();
		surrendering = true;
		animator.SetBool("Surrendering", true);
		animator.SetFloat("moveX", 0);
		animator.SetFloat("moveY", 0);
		animator.SetBool("moving", false);
		gameObject.GetComponent<ExecutePathFindingPlayable>().StopPathFinding();
		gameObject.GetComponent<ExecutePathFindingPlayable>().disabled = true;

		LevelManager.emit("Surrendered", gameObject);
	}

	public void StopSurrender() {
		surrendering = false;
		animator.SetBool("Surrendering", false);
		gameObject.GetComponent<ExecutePathFindingPlayable>().disabled = false;
	}
}