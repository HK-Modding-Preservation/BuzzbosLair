using FriendCore;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using UnityEngine;

namespace BuzzbosLair
{
    internal class SmallBee : MonoBehaviour
    {

        private AlterHealthManager alter_hm;
        private AlterInfectedEnemyEffects alter_blood;
        private AlterEnemyDreamnailReaction alter_dnail_reaction;

        private PlayMakerFSM _fsm;

        void Awake()
        {
            gameObject.GetComponent<Recoil>().enabled = false;

            alter_hm = gameObject.AddComponent<AlterHealthManager>();
            alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();
            alter_dnail_reaction = gameObject.AddComponent<AlterEnemyDreamnailReaction>();
        }

        void Start()
        {
            //GameObject _corpse = gameObject.Find("Corpse Minimal(Clone)");
            //Destroy(_corpse.Find("Pt Death"));

            alter_hm.SetRegen(0.25f, 0.1f, 1);
            alter_hm.SetEnemyType((int)EnemyDeathTypes.Shade);
            alter_blood.SetColor(Presets.Colors.hiveblood);
            alter_dnail_reaction.SetNoSoul();

            if (gameObject.name.Contains("Bee Hatchling Ambient"))
            {
                _fsm = gameObject.LocateMyFSM("Bee");
                _fsm.RemoveTransition("Chase", "OUT OF RANGE");
                _fsm.GetAction<ChaseObject>("Chase", 1).speedMax = 15f;
            } else if (gameObject.name.Contains("Hiveling Spawner"))
            {
                _fsm = gameObject.LocateMyFSM("Control");
                _fsm.GetAction<ChaseObjectV2>("Chase", 2).speedMax = 15f;
            }

        }

    }
}
