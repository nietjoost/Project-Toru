using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 20;
	public Rigidbody2D rb;
    public bulletOwner owner;
    public Weapon weapon;

    void Start()
    {
		rb.velocity = transform.right * speed;
		Invoke("DestroyObject", 1);
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("NPC") && owner != bulletOwner.NPC)
		{
			other.GetComponent<CharacterStats>().TakeDamage(weapon.damage);
			DestroyObject();
		}
        else if (other.CompareTag("Player") && owner != bulletOwner.Player)
        {
            other.GetComponent<CharacterStats>().TakeDamage(weapon.damage);
            DestroyObject();
        }
		else if (other.CompareTag("Door"))
        {
            if (other.GetComponent<Door>().IsClosed()) {
				DestroyObject();
			}

        }
	}
	
	private void DestroyObject()
	{
		Destroy(this.gameObject);
	}
}

public enum bulletOwner
{
    Player,
    NPC
}
