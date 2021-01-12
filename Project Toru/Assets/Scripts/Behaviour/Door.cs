using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    [SerializeField]
    Collider2D doorCollider = null;

    public bool closed = true;
	
	public Room room = null;

    void Start()
    {
        if (!closed)
        {
            Open();
        }
		
		room = this.GetComponentInParent(typeof(Room)) as Room;
    }

    public bool Close()
    {
        closed = true;
        GetComponent<Animator>().SetBool("openDoor", false);
        doorCollider.enabled = true;

        return true;
    }

    public bool Open()
    {
        closed = false;
        GetComponent<Animator>().SetBool("openDoor", true);

        StartCoroutine(WaitForAnimationEndTimer());

        return true;
    }

    IEnumerator WaitForAnimationEndTimer()
    {
        yield return new WaitForSeconds(0f);
        doorCollider.enabled = false;
    }

    public bool IsOpen()
    {
        return !closed;
    }

    public bool IsClosed()
    {
        return closed;
    }
	
	public int isOpenOrHasTheRightKey(Item i)
	{
		Debug.LogWarning("This functions is not fully implemented");
		return 1;
	}
}
