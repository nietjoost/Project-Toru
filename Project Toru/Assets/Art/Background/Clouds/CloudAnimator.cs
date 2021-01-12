using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloudAnimator : MonoBehaviour
{
    [SerializeField]
    Tilemap sky = null;

    [SerializeField]
    [Range(0, 100)]
    int totalCloudsRendered = 10;

    [SerializeField]
    List<GameObject> clouds = new List<GameObject>();

    List<Cloud> renderedClouds = new List<Cloud>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < totalCloudsRendered; i++)
        {
            GameObject cloud = Instantiate(clouds[Random.Range(0, clouds.Count)], new Vector3(Random.Range(sky.origin.x, 50), Random.Range(5, 13), 0), Quaternion.identity);
            renderedClouds.Add(new Cloud(cloud));
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cloud in renderedClouds)
        {
            if (cloud.cloud.transform.position.x > 50)
            {
                cloud.cloud.transform.position = new Vector3(sky.origin.x, Random.Range(5, 13), 0);
            }
            cloud.cloud.transform.Translate(cloud.speed * Time.deltaTime, 0, 0);
        }
    }
}

public struct Cloud
{
    public GameObject cloud;
    public float speed;

    public Cloud(GameObject cloud)
    {
        this.cloud = cloud;
        this.speed = Random.Range(0.1f, 1f);
    }
}