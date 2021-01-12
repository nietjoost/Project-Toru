using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{

    [SerializeField]
    GameObject LeftWall = null;

    public void EnableLeftWall(bool enable)
    {
        LeftWall?.SetActive(enable);
    }
}
