using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tint
{
    private static Color defaultColor = new Color(0.7f, 0.7f, 0.7f, 1);
    private static Color transparent = new Color(1, 1, 1, 0.3f);

    public static void Apply(GameObject obj, Color color)
    {
        obj.GetComponent<SpriteRenderer>().color = color;
    }

    public static void Apply(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().color = defaultColor;
    }

	public static void Transparent(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().color = transparent;
    }

	public static void Transparent(Image obj)
	{
		obj.color = transparent;
	}

	public static void Reset(GameObject obj)
	{
		obj.GetComponent<SpriteRenderer>().color = Color.white;
	}

	public static void Reset(Image obj)
	{
		obj.color = Color.white;
	}
}
