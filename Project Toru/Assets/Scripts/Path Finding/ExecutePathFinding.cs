using Assets.Scripts.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExecutePathFinding : MonoBehaviour
{
	[NonSerialized]
	public PathFinding pf;

	[NonSerialized]
	public List<Vector3> path;

	[NonSerialized]
	public Animator animator;

	[NonSerialized]
	public int current = 0;

	[NonSerialized]
	public GameObject targetFurniture;

	Character character;
	Vector3 change;

	//Character stair variables
	float timer = 0;
	float stairsDuration = 1;

	[NonSerialized]
	public bool playerOnTheStairs = false;

	//[NonSerialized]
	public Room currentRoom;

	private Weapon weapon;

	public void Awake()
	{
		path = new List<Vector3>();

		currentRoom = GetEntranceOutsideRoom();
	}

	public void Start()
	{
		animator = GetComponent<Animator>();
		character = GetComponent<Character>();
		pf = new PathFinding();
	}

	public void AdjustOrderLayer()
	{
		GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.position.y * 1000);
	}

	public void WayPointsWalk()
	{
		if (path.Count > 0)
		{
			if (!playerOnTheStairs)
			{
				Vector3 newPosition = path[current];

				if (transform.position == newPosition)
				{
					current++;
				}

				change = Vector2.zero;

				change = newPosition - transform.position;

				if (tag.Contains("Player")) { 
					character.change = this.change;
				}
				
				transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * 4);
			}

			if (current == path.Count)
			{
				if (tag.Contains("Player"))
				{ 
					CallEventWindow();
				}
				StopPathFinding();
			}

			UpdateAnimations();
		}
	}

	private void CallEventWindow()
	{
		if (targetFurniture != null)
		{
			var e = targetFurniture.gameObject.GetComponent<Assets.Scripts.Options.Event>();
			if (e != null)
			{
				e.AddActor(GetComponent<Character>());
				CurrentEventWindow.Current.AddEvent(e);
			}
		}
	}

	public Room getCoRoom(Vector2 loc)
	{
		Room r;

		foreach (GameObject v in GameObject.FindGameObjectsWithTag("Room"))
		{
			r = v.GetComponent<Room>();

			if (loc.x >= r.transform.position.x && loc.x <= (Math.Round(r.transform.position.x) + (int)r.GetSize().x) && loc.y >= r.transform.position.y && loc.y <= (Math.Round(r.transform.position.y) + (int)r.GetSize().y))
			{
				return r;
			}
		}
		return null;
	}

	public void checkDoorClosed(Collider2D other)
	{
		try
		{
			if (other.gameObject.GetComponent<CardReader>())
			{
				if (other.gameObject.GetComponent<CardReader>().getDoor().IsClosed())
				{
					if (gameObject.GetComponent<Character>().HasKey(other.gameObject.GetComponent<CardReader>().GetColor()) || other.gameObject.GetComponent<CardReader>().GetColor() == CardreaderColor.Disabled)
					{
						other.gameObject.GetComponent<CardReader>().getDoor().Open();
						return;
					}
					StopPathFinding();
					return;
				}
			}
		}
		catch (NullReferenceException) {}

		try
		{
			if (other.gameObject.GetComponent<CardReader>())
			{
				if (other.gameObject.GetComponent<CardReader>().getDoor().IsClosed())
				{
					if (gameObject.tag.Equals("NPC"))
					{
						if (gameObject.GetComponent<NPC>().HasKey(other.gameObject.GetComponent<CardReader>().GetColor()) || other.gameObject.GetComponent<CardReader>().GetColor() == CardreaderColor.Disabled)
						{
							other.gameObject.GetComponent<CardReader>().getDoor().Open();
							return;
						}
						StopPathFinding();
						return;
					}
				}
			}
		}
		catch (NullReferenceException) { }
	}

	public void StopPathFinding()
	{
		current = 0;
		path.Clear();

		targetFurniture = null;
	}

	public void UpdateAnimations()
	{
		if(change != Vector3.zero)
		{	
			animator.SetFloat("moveX", change.x);
			animator.SetFloat("moveY", change.y);
			animator.SetBool("moving", true);
		}
		else
		{
			/*animator.SetFloat("moveX", 0);
			animator.SetFloat("moveY", 0);*/
			animator.SetBool("moving", false);
		}
		change = Vector2.zero;
	}

	public void PathFinding(Vector3 pos)
	{
		Room positionRoom = getCoRoom(pos);

		current = 0;
		path.Clear();

		if(currentRoom.name.ToLower().Contains("entrance") && positionRoom == null)
		{
			path.Add(new Vector2(pos.x, 1));
			return;
		}

		if (positionRoom != null)
		{
			pos = new Vector3(pos.x, positionRoom.transform.position.y + 1, -1);

			Room characterRoom;

			try
			{
				characterRoom = currentRoom;
			}
			catch (UnassignedReferenceException)
			{
				characterRoom = GetEntranceRoom();

				path.Add(new Vector3(characterRoom.transform.position.x - 1, characterRoom.transform.position.x + 1, -1));
			}

			if (!positionRoom.Equals(characterRoom))
			{
				path = pf.CalculateTransforms(positionRoom, characterRoom);
			}
			path.Add(pos);
		}
		else
		{
			Room entranceRoom = GetEntranceRoomToOutside(pos);

			//Code for inside to outside
			try
			{
				if (entranceRoom != null)
				{ 
					path = pf.CalculateTransforms(entranceRoom, currentRoom);
					path.Add(new Vector3(pos.x, entranceRoom.transform.position.y + 1, -1));
				}
			}
			catch (UnassignedReferenceException)
			{
				//Outside to outside code
				path.Add(new Vector3(pos.x, entranceRoom.transform.position.y + 1, -1));
			}
		}

		if (currentRoom.name.ToLower().Contains("entrance"))
		{
			path.Insert(0, new Vector2(transform.position.x, 1));
		}
	}

	private Room GetEntranceRoomToOutside(Vector2 pos)
	{
		foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
		{
			Room r = room.GetComponent<Room>();

			//TO-DO: Entrance room to other outside

			if (r.name.ToLower().Contains("entrance") && pos.y > (r.transform.position.y + 0.1) && pos.y < (r.transform.position.y + 2))
			{
				return r;
			}
		}
		return null;
	}

	public Room GetEntranceRoom()
	{
		foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
		{
			Room r = room.GetComponent<Room>();

			if (r.name.ToLower().Contains("entrance") && transform.position.y >= room.transform.position.y && transform.position.y <= (transform.position.y + r.GetSize().y))
			{
				return r;
			}
		}
		return null;
	}

	public Room GetEntranceOutsideRoom()
	{
		foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
		{
			Room r = room.GetComponent<Room>();

			if (r.name.ToLower().Contains("entrance"))
			{
				return r;
			}
		}
		return null;
	}

	//Character functions
	public void StairsTransistion()
	{
		playerOnTheStairs = true;
		GetComponent<Renderer>().enabled = false;

		if (tag.Contains("Player"))
		{
			weapon = GetComponent<Character>().weapon;
			if(weapon != null)
			{
				if (weapon.weaponOut)
				{
					weapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
			transform.Find("SelectedTriangle").gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}

		//Go to next transform in pathfinding
		current++;
	}

	public void HidePlayerOnStair()
	{
		if (playerOnTheStairs)
		{
			timer += Time.deltaTime;

			if (timer > stairsDuration)
			{
				playerOnTheStairs = false;
				timer = 0;
				this.GetComponent<Renderer>().enabled = true;

				if (tag.Contains("Player"))
				{
					if (weapon != null)
					{
						if (weapon.weaponOut)
						{
							weapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
						}
					}

					if (GetComponent<Character>().Equals(Character.selectedCharacter))
					{
						transform.Find("SelectedTriangle").gameObject.GetComponent<SpriteRenderer>().enabled = true;
					}
				}
			}
		}
	}
}