using FriendCore;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using UnityEngine;

namespace BuzzbosLair
{
    internal class SmallBee : MonoBehaviour
    {

        private HealthManager _hm;
        private AlterHealthManager _alter_hm;
        private AlterInfectedEnemyEffects _alter_blood;
        private AlterEnemyDreamnailReaction _alter_dnail_reaction;

        private PlayMakerFSM _fsm;

        void Awake()
        {
            gameObject.GetComponent<Recoil>().enabled = false;

            _hm = gameObject.GetComponent<HealthManager>();
            _alter_hm = gameObject.AddComponent<AlterHealthManager>();
            _alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();
            _alter_dnail_reaction = gameObject.AddComponent<AlterEnemyDreamnailReaction>();
        }

        void Start()
        {
            //GameObject _corpse = gameObject.Find("Corpse Minimal(Clone)");
            //Destroy(_corpse.Find("Pt Death"));

            int naildmg = HeroController.instance.gameObject.transform.Find("Attacks/Slash").GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("damageDealt").Value;
            _hm.hp = 2 * naildmg;
            _alter_hm.SetMaxHp(_hm.hp);
            _alter_hm.SetRegen(0.25f, 0.1f, 1);
            _alter_hm.SetEnemyType((int)EnemyDeathTypes.Shade);
            _alter_blood.SetColor(Presets.Colors.lifeblood);
            _alter_dnail_reaction.SetNoSoul();

            if (gameObject.name.Contains("Bee Hatchling Ambient"))
            {
                _fsm = gameObject.LocateMyFSM("Bee");
                _fsm.RemoveTransition("Chase", "OUT OF RANGE");
                _fsm.GetAction<ChaseObject>("Chase", 1).speedMax = 15f;
            } else if (gameObject.name.Contains("Hiveling Spawner"))
            {
                _fsm = gameObject.LocateMyFSM("Control");
                _fsm.GetAction<ChaseObjectV2>("Chase", 2).speedMax = 15f;
                _fsm.GetState("Set Collider").AddMethod(() => {
                    gameObject.GetComponent<DamageHero>().damageDealt = 1;
                });
            }

        }

    }
}
