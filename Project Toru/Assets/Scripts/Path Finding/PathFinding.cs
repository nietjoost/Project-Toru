using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
	private List<Node> pathN;

	public List<Vector3> CalculateTransforms(Room startRoom, Room endRoom)
	{
		List<Vector3> pathR = new List<Vector3>();

		List<Node> route = CalculateRoute(startRoom, endRoom);

		for (int i = 0; i < route.Count; i++)
		{
			List<Vector3> localTransforms = new List<Vector3>();

			if (i == route.Count - 1)
			{
				//Do Nothing
			}
			else if(!route[i].nodeRoom.isRoom() && !route[i].parent.nodeRoom.isRoom())
			{
				//Stair to Stair
				localTransforms = getStairTransform(route[i]);
			}
			pathR.AddRange(localTransforms);
		}
		return pathR;
	}

	private List<Node> CalculateRoute(Room startRoom, Room endRoom)
	{
		Node.vissited.Clear();
		Node.node.Clear();

		Node.endRoom = endRoom;

		new Node(null, startRoom);

		return GetPathToList(Node.endNode);
	}

	private List<Node> GetPathToList(Node n)
	{
		Node current = n;

		pathN = new List<Node>();

		pathN.Add(n);

		while (current.parent != null)
		{
			current = current.parent;
			pathN.Add(current);
		}
		return pathN;
	}

	private List<Vector3> getStairTransform(Node n)
	{
		List<Vector3> local = new List<Vector3>();

		if (n.nodeRoom.GetPosition().y < n.parent.nodeRoom.GetPosition().y)
		{
			//Go up
			local.Add(new Vector3(n.nodeRoom.GetPosition().x + 5, n.nodeRoom.GetPosition().y + 1));
			local.Add(new Vector3(n.nodeRoom.GetPosition().x + 5, n.nodeRoom.GetPosition().y + 2));
		}
		else
		{
			//Go down
			local.Add(new Vector3(n.nodeRoom.GetPosition().x + 2, n.nodeRoom.GetPosition().y + 1));
			local.Add(new Vector3(n.nodeRoom.GetPosition().x + 2, n.nodeRoom.GetPosition().y + 2));
		}
		return local;
	}
}