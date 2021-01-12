using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Options
{
    public class DisableCameraOption : Option
    {
        private SecurityDesk desk;

        public void Start()
        {
            desk = GetComponentInParent<SecurityDesk>();
        }

        public override string Activate(Character c)
        {
			LevelManager.emit("CamerasDisabled");
            return null;
        }
    }
}
