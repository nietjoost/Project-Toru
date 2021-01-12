using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Outline
{

    public static void SetOutline(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Sprite-Outline");
    }

    public static void SetOutline(GameObject obj, Material material)
    {
        obj.GetComponent<SpriteRenderer>().material = material;
    }

    public static void RemoveOutline(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Sprite-Default");
    }

}
