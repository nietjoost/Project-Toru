using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class securitydesk : MonoBehaviour
{
    BuildingBehaviour building;

    // Start is called before the first frame update
    void Start()
    {
        building = GameObject.Find("Building").GetComponent<BuildingBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
        {
            if (other.CompareTag("Player"))
            {
                building.DiscoverAllRooms();
            }
        }
    }

}
