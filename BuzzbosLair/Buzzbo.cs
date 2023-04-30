using FriendCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuzzbosLair
{
    internal class Buzzbo : MonoBehaviour
    {

        private bool awakened = false;

        private AlterHealthManager _alter_hm;
        private AlterInfectedEnemyEffects _alter_blood;

        private PlayMakerFSM _control;
        private PlayMakerFSM _stun_control;


    }
}
