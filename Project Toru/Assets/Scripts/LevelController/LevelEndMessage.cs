using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is static, so the content remains even when we have a scene change.
/// Because of this, we can pass data to other scenes
/// </summary>
public static class LevelEndMessage
{
    public static string title;
    public static string message;
	public static string nextLevel;

    public static bool LevelSuccessfull = false;

    public static int money;

	public static int lastLevel = -1;

    public static void Reset()
    {
        title = "";
        message = "";
		nextLevel = "";
        LevelSuccessfull = false;
        money = 0;
    }
}
