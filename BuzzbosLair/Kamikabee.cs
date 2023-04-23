using FriendCore;
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

        private AlterHealthManager alter_hm;

        public void Awake()
        {
            _control = gameObject.LocateMyFSM("Big Bee");

            alter_hm = gameObject.AddComponent<AlterHealthManager>();
        }

        public void Start()
        {
            //_control.GetAction<SetIntValue>("Charge Antic", 0).intValue = 100;

            _control.GetState("Hit Vertical").AddMethod(() => {
                _control.GetFloatVariable("Angle").Value += (UnityEngine.Random.Range(0, 15) - UnityEngine.Random.Range(0, 15));
            });
            _control.GetState("Hit Horizontal").AddMethod(() => {
                _control.GetFloatVariable("Angle").Value += (UnityEngine.Random.Range(0, 15) - UnityEngine.Random.Range(0, 15));
            });

            //_control.RemoveAction("Hit Vertical", 1);
            //_control.RemoveAction("Hit Horizontal", 1);
            //_control.RemoveTransition("Hit Vertical", "END");
            //_control.RemoveTransition("Hit Horizontal", "END");

            alter_hm.SetRegen(1f, 1f, 5);
        }

    }
}
