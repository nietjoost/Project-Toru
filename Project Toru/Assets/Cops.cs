using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cops : MonoBehaviour
{
    [SerializeField]
    GameObject copsSpawnPoint = null;

    [SerializeField]
    GameObject copsPrefab = null;

    private List<GameObject> copsList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject move in copsList)
        {
            move.GetComponent<Rigidbody2D>().MovePosition(new Vector2(move.transform.position.x + 0.1f, move.transform.position.y));
        }
    }

    public void Spawn()
    {
        GameObject obj = Instantiate(copsPrefab, new Vector3(copsSpawnPoint.transform.position.x - 0.5f, copsSpawnPoint.transform.position.y - 0.5f, 0), Quaternion.identity);
        copsList.Add(obj);
    }
}
