using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Item : MonoBehaviour
{
	public new string name;
	public Sprite UIIcon;
	public float Weight;
	public bool isStackable = false;
	public float value;
}

