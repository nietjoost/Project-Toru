using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enums
{
    public enum Direction
    {
        Up = 1,
        Down = ~Up,
        Left = 2,
        Right = ~Left
    }
}