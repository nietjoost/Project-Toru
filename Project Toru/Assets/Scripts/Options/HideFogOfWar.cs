using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Options
{
    public class HideFogOfWar : Option
    {
        BuildingBehaviour building;

        public void Start()
        {
            building = GameObject.Find("Building").GetComponent<BuildingBehaviour>();
        }

        public override string Activate(Character c)
        {
            building.DiscoverAllRooms();
            return null;
        }
    }
}