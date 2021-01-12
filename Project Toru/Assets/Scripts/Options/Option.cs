using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Options
{
    public abstract class Option : MonoBehaviour
    {
        [SerializeField]
        private string Description = null;
		[SerializeField]
		public bool once = false;

		public Skills? Prerequisite = null;

        public virtual string getInfo()
        {
            return Description;
        }

        /// <summary>
        /// activates this action
        /// </summary>
        /// <param name="c"></param>
        /// <returns>should return a result message if neccesary</returns>
        public abstract string Activate(Character c);
    }
}
