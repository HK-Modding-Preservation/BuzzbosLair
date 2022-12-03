using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuzzbosLair
{
    internal class Kamikabee : MonoBehaviour
    {

        private PlayMakerFSM _control;

        public void Awake()
        {
            _control = gameObject.LocateMyFSM("Big Bee");
        }

        public void Start()
        {
            _control.GetAction<SetIntValue>("Charge Antic", 0).intValue = 10;
        }

    }
}
