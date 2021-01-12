using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimator : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera = null;

    [SerializeField]
    GameObject SkyscraperFront = null;

    [SerializeField]
    GameObject SkyscraperBack = null;

    void Start()
    {
        if (mainCamera == null)
        {
            this.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        SkyscraperFront.transform.position = new Vector3(mainCamera.gameObject.transform.position.x * 0.2f, SkyscraperFront.transform.position.y, SkyscraperFront.transform.position.z);
        SkyscraperBack.transform.position = new Vector3(mainCamera.gameObject.transform.position.x * 0.1f, SkyscraperBack.transform.position.y, SkyscraperBack.transform.position.z);
    }
}
