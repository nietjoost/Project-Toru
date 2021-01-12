using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ExecutePathFindingNPC))]
public abstract class NPC : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> bag = null;

    [SerializeField]
    private GameObject TextBox = null;

    // [NonSerialized]
    public Room currentRoom = null;

    [NonSerialized]
    public StateMachine statemachine = new StateMachine();

    [NonSerialized]
    public Vector3 startingPosition = Vector3.zero;

    [NonSerialized]
    public Animator animator = null;

	[NonSerialized]
    public ExecutePathFindingNPC pathfinder = null;

    [NonSerialized]
    public CharacterStats stats;

	protected Weapon weapon;
	protected bool showWeapon = false;
	protected GameObject firePoint;

	public bool surrender = false;
	protected bool fleeTrue = false;

	DialogueManager dialogueManager;

	public bool fleeIfPossible = false;

	Vector3 currentpos;
    Vector3 lastpos;
    Vector3 change;

	protected virtual void Awake() {

		startingPosition = transform.position;;
        stats = GetComponent<CharacterStats>();
		animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();
		pathfinder = GetComponent<ExecutePathFindingNPC>();
		dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();

		if(weapon != null)
        {
            firePoint = weapon.gameObject;
			weapon.weaponHolder = gameObject;
        }
	}

	protected virtual void Start() {

	}

	protected virtual void Update() {
		this.statemachine.ExecuteStateUpdate();
		AdjustOrderLayer();

		lastpos = currentpos;
        currentpos = transform.position;
        change = currentpos - lastpos;

		if (fleeIfPossible) FleeIfPossible();

		if(change != Vector3.zero && showWeapon)
        {
            FlipFirePoint();
        }
	}

    public void dropBag()
    {
        foreach (GameObject g in bag)
        {
            Instantiate(g, new Vector3(transform.position.x + UnityEngine.Random.Range(-1.5f, 1.5f), currentRoom.transform.position.y + 0.5f, 0), Quaternion.identity);
        }
		bag.Clear();
    }

    public void Say(string text)
    {
        TextBox.GetComponent<TextMesh>().text = text;
        TextBox.SetActive(true);
		TextBox.GetComponent<Renderer>().sortingLayerName = "UI";
        Invoke("disableTextBox", 3);
    }

    private void disableTextBox()
    {
        TextBox.SetActive(false);
    }

    public void AdjustOrderLayer()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.position.y * 1000);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Room"))
        {
            currentRoom = other.gameObject.GetComponent<Room>();
        }
    }

    private void OnMouseOver()
    {
        if (currentRoom.SelectedPlayerInRoom())
        {
            Tint.Apply(this.gameObject);
        }
    }

    public void PingPong()
    {
        this.statemachine.ChangeState(new PingPong(this.startingPosition, this.gameObject, this.animator));
    }

    private void OnMouseExit()
    {
        Tint.Reset(this.gameObject);
    }

    public bool HasKey(CardreaderColor color)
    {
        foreach (GameObject i in bag)
        {
            if (i.GetComponent<Key>())
            {
                if (i.GetComponent<Key>().color == color)
                {
                    return true;
                }
            }
        }
        return false;
    }

	public virtual void ShootAt(Character character) {
		showWeapon = true;
		this.statemachine.ChangeState(new Combat(this, weapon, gameObject, firePoint, animator, character.gameObject));
	}

	public virtual void StopShooting() {
		this.statemachine.ChangeState(new Idle(animator));
		weapon.HideGun();
		showWeapon = false;
	}

	protected virtual void FlipFirePoint()
    {
        if (animator.GetFloat("moveX") > 0)
        {
            firePoint.transform.rotation = Quaternion.Euler(0, 0, 0);
            firePoint.transform.position = transform.position + new Vector3(.3f, -.3f);
            firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
        }
        else
        {
            firePoint.transform.rotation = Quaternion.Euler(0, 180, 0);
            firePoint.transform.position = transform.position + new Vector3(-.3f, -.3f);
            firePoint.GetComponent<SpriteRenderer>().sortingLayerName = "Guns";
        }

        if (animator.GetFloat("moveY") > 0.1)
        {
            weapon.HideGun();
        }
        else
        {

            weapon.RevealGun();
        }
    }

	protected virtual void OnMouseDown() {
		if (dialogueManager.animator.GetBool("IsOpen") == true) {
			return;
		}
		
		Surrender();
	}

	public virtual void Surrender()
    {
        if (currentRoom.SelectedPlayerInRoom() && !surrender)
        {
			Character.selectedCharacter.weapon.RevealGun();

            this.surrender = true;
			animator.SetFloat("moveX", 0);
            this.statemachine.ChangeState(new Surrender(this.animator));
			
			LevelManager.emit("Surrendered", gameObject);
        }
    }


	protected virtual void FleeIfPossible()
    {
        if (currentRoom == null) return;

        if (!currentRoom.AnyCharacterInRoom() && surrender)
        {
            this.surrender = false;
            this.statemachine.ChangeState(new Idle(this.animator));
			gameObject.GetComponent<ExecutePathFindingNPC>().setPosTarget(-30, 1);
            fleeTrue = true;
        }
    }
}
