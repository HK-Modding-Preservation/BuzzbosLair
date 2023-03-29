using FriendCore;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuzzbosLair
{
    internal class SmallBee : MonoBehaviour
    {

        private AlterHealthManager alter_hm;

        public void Awake()
        {
            alter_hm = gameObject.AddComponent<AlterHealthManager>();
        }

        public void Start()
        {
            //GameObject _corpse = gameObject.Find("Corpse Minimal(Clone)");
            //Destroy(_corpse.Find("Pt Death"));

            alter_hm.SetRegen(0.25f, 0.1f, 1);
            alter_hm.SetEnemyType((int)EnemyDeathTypes.Shade);
        }

    }
}
