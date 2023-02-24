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

        public void Awake()
        {
            gameObject.AddComponent<Hiveblood>();
            gameObject.GetComponent<Recoil>().enabled = false;
            gameObject.AddComponent<Bloodchanger>();
        }

        public void Start()
        {
            GameObject _corpse = gameObject.Find("Corpse Minimal(Clone)");
            Destroy(_corpse.Find("Pt Death"));
        }

    }
}
