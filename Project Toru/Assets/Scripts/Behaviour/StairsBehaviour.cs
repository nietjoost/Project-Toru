using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using Assets.Scripts.Enums;

public class StairsBehaviour : Room
{
	public StairsBehaviour Upstairs = null;
	public StairsBehaviour Downstairs = null;

	[SerializeField]
	StairsCollission GoUpStairs = null;

	[SerializeField]
	StairsCollission GoDownStairs = null;

	void Start()
	{
        //SetupRoom();
	}

    public override void SetupRoom()
    {
        base.SetupRoom();
        // Hide stairs when floor is not connected
        if (Upstairs == null && GoUpStairs != null)
        {
            GoUpStairs?.Disable();
        }

        if (Downstairs == null && GoUpStairs != null)
        {
            GoDownStairs?.Disable();
        }
    }

	/// <summary>
	/// Returns target for Go Up movement
	/// </summary>
	/// <returns>Target set by GoDown</returns>
	public Transform GetUpTarget()
	{
		// If GoUpstairs is not set, return null
		if (GoDownStairs == null) return null;

		// Find PositionTarget
		// We need to get the Downstairs target, because the character must end there.
		return GoDownStairs?.GetTarget();
	}

	/// <summary>
	/// Returns target for Go Down movement
	/// </summary>
	/// <returns>Target set by GoUp</returns>
	public Transform GetDownTarget()
	{
		// If GoUpstairs is not set, return null
		if (GoUpStairs == null) return null;

		// Find PositionTarget
		// We need to get the Downstairs target, because the character must end there.
		return GoUpStairs?.GetTarget();
	}

	/// <summary>
	/// Receives message from collider script in stairs
	/// Gets target and sends information to character
	/// </summary>
	/// <param name="GoUp">True when movement is up direction, false when down direction</param>
	/// <param name="collider">The character collider</param>
	public void UseStairs(bool GoUp, Collider2D collider)
	{
		Transform target = (GoUp ? Upstairs?.GetUpTarget() : Downstairs?.GetDownTarget());

		if (target == null)
		{
			return;
		}

		// Move character
		collider.transform.position = target.position;

		// Let character know it is using a stairs
		// Get GameObject from collider
		GameObject gameobject = collider.gameObject;

		// Check if this gameobject has an script Character

		// Check if this gameobject has an script Character
		ExecutePathFinding character = (ExecutePathFinding)gameobject.GetComponent(typeof(ExecutePathFinding));

		if (character != null)
		{
			character.StairsTransistion();
		}
	}

    public override void AddNeighbour(Direction direction, Room neighbour, bool callback = true)
    {
        base.AddNeighbour(direction, neighbour, callback);
        switch (direction)
        {
            case Direction.Up:
                Upstairs = neighbour.getStairScript();

                break;
            case Direction.Down:
                Downstairs = neighbour.getStairScript();
                break;

            default:
                break;
        }
    }
}