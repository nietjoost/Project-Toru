using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Van : MonoBehaviour
{

    List<Character> characters = new List<Character>();

    public Collider2D carCollider = null;

    public bool drive = false;
	
	public LevelScript levelScript = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (drive)
        {
			carCollider?.gameObject.SetActive(false);
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x + -0.1f, transform.position.y));
        }
    }
	
	public void Drive() {
		drive = true;
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        Character ch = collision.GetComponent<Character>();

        if (collision.isTrigger && ch != null)
        {
			LevelManager.emit("CharacterEntersVan", ch.gameObject);
			
            this.EnterCharacter(ch);
        }
    }

    public void EnterCharacter(Character character)
    {
        characters.Add(character);
        character.gameObject.SetActive(false);
    }

    public void ExitCharacter(Character character)
    {
        characters.Remove(character);
        character.gameObject.SetActive(true);
    }
}
