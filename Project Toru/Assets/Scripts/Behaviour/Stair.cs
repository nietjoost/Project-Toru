using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    // Assign target for this transistion. The character moves to this spot.
    [SerializeField]
    private Transform target = null;

    private float timer = 0;
    private float stairsDuration = 1;
    private bool playerOnTheStairs = false;
    private Collider2D player = null;

    void Update()
    {
        if (playerOnTheStairs)
        {
            timer += Time.deltaTime;
        }

        if (timer > stairsDuration)
        {
            playerOnTheStairs = false;
            timer = 0;
            PlayerUsesStairs();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other;
            playerOnTheStairs = true;
            player.gameObject.SetActive(false);
        }
    }

    private void PlayerUsesStairs()
    {
        // Move character
        player.gameObject.SetActive(true);
        player.transform.position = target.position;

        // Let character know it is using a stairs
        // Get GameObject from collider
        GameObject gameobject = player.gameObject;

		// Check if this gameobject has an script Character
		ExecutePathFinding character = (ExecutePathFinding)gameobject.GetComponent(typeof(ExecutePathFinding));

        if (character != null)
        {
            character.StairsTransistion();
        }
    }


}
