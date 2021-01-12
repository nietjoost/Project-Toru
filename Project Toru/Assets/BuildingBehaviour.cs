using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts.Enums;

public class BuildingBehaviour : MonoBehaviour
{
	[SerializeField]
	Grid grid = null;

	/// <summary>
	/// List of all rooms in this building
	/// </summary>
	Room[] rooms = null;

	/// <summary>
	/// Size of building
	/// </summary>
	Vector2Int size = new Vector2Int(0, 0);

	/// <summary>
	/// Center of building in canvas
	/// </summary>
	Vector2Int center = new Vector2Int(0, 0);

	/// <summary>
	/// Bottom left of building
	/// </summary>
	Vector2Int bottomLeft = new Vector2Int(0, 0);

	// Start is called before the first frame update
	void Start()
	{
		CalculateBuilding();
        GetAllRoomNeighbours();
        SetupAllRooms();
    }

	/// <summary>
	/// Calculates building dimentions
	/// must be called every time the room is changed dynamicly.
	/// </summary>
	void CalculateBuilding()
	{

        // Get rooms
        Room[] tmpRooms = grid.GetComponentsInChildren<Room>();
        StairsBehaviour[] tmpStairs = grid.GetComponentsInChildren<StairsBehaviour>();
        rooms = new Room[tmpRooms.Length + tmpStairs.Length];
        tmpRooms.CopyTo(rooms, 0);
        tmpStairs.CopyTo(rooms, tmpRooms.Length);

		// Calculate size of building
		int left = int.MaxValue;
		int right = int.MinValue;
		int top = int.MinValue;
		int bottom = int.MaxValue;

		foreach (Room room in rooms)
		{

			Vector3Int position = room.GetPosition();
			if (position.y < bottom)
				bottom = position.y;

			if (position.x < left)
				left = position.x;

			if (position.x + room.GetSize().x > right)
				right = position.x + room.GetSize().x;

			if (position.y + room.GetSize().y > top)
				top = position.y + room.GetSize().y;
		}

		size = new Vector2Int(right - left, top - bottom);

		// Calculate center of building (For camera eg)
		center = new Vector2Int((left + right) / 2, (bottom + top) / 2);

		// Set bottom left
		bottomLeft = new Vector2Int(left, bottom);
	}

	/// <summary>
	/// Returns all rooms in a list (Unordered)
	/// Must be updated with CalculateBuilding() after building has changed
	/// </summary>
	/// <returns>Unordered list of rooms in this building</returns>
	public Room[] GetRooms()
	{
		return rooms;
	}

	/// <summary>
	/// Returns total of rooms
	/// </summary>
	/// <returns>Total of rooms in building</returns>
	public int GetTotalRooms()
	{
		return rooms.Length;
	}

	/// <summary>
	/// Returns size of building
	/// This is not dependend on canvas layout. Just an absolute value of the size
	/// </summary>
	/// <returns>Absolute value of the size of the building</returns>
	public Vector2Int GetSize()
	{
		return size;
	}

	/// <summary>
	/// Returns center of building dependent of the canvas
	/// </summary>
	/// <returns>Center of building depending on canvas</returns>
	public Vector2Int GetCenter()
	{
		return center;

	}

	/// <summary>
	/// Returns bottomleft of building depentend of the canvas 
	/// </summary>
	/// <returns></returns>
	public Vector2Int GetBottomLeft()
	{
		return bottomLeft;
	}

    /// <summary>
    /// 
    /// </summary>
    public void DiscoverAllRooms()
    {
        foreach (Room room in rooms)
        {
            room.HideFogOfWar();
        }
    }

    private void GetAllRoomNeighbours()
    {
        foreach (Room room in rooms)
        {
            foreach (Room possibleNeighbour in rooms)
            {
                if ((possibleNeighbour.GetPosition().x <= room.GetPosition().x + room.GetSize().x + 1) 
                    && (possibleNeighbour.GetPosition().x + possibleNeighbour.GetSize().x >= room.GetPosition().x + room.GetSize().x + 1) 
                    && (possibleNeighbour.GetPosition().y <= room.GetPosition().y + 1) 
                    && (possibleNeighbour.GetPosition().y + possibleNeighbour.GetSize().y >= room.GetPosition().y + 1)) 
                { 
                    possibleNeighbour.AddNeighbour(Direction.Left, room); 
                } 
                if (room.getStairScript() != null 
                    && (possibleNeighbour.GetPosition().x <= room.GetPosition().x + 1) 
                    && (possibleNeighbour.GetPosition().x + possibleNeighbour.GetSize().x >= room.GetPosition().x + 1) 
                    && (possibleNeighbour.GetPosition().y <= room.GetPosition().y + room.GetSize().y + 1) 
                    && (possibleNeighbour.GetPosition().y + possibleNeighbour.GetSize().y >= room.GetPosition().y + room.GetSize().y + 1)) 
                { 
                    possibleNeighbour.AddNeighbour(Direction.Down, room); 
                } 
            }
        }
    }

    private void SetupAllRooms()
    {
        foreach (Room room in rooms)
        {
            room.SetupRoom();
        }
    }
}
