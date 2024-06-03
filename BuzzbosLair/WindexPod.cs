using FriendCore;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using System.Collections;
using UnityEngine;

namespace BuzzbosLair
{
    internal class WindexPod : MonoBehaviour
    {

        private GameObject little_bee;

        private AlterHealthManager _alter_hm;
        private AlterInfectedEnemyEffects _alter_blood;
        private AlterEnemyDreamnailReaction _alter_dreamnail;

        private PlayMakerFSM fsm;

        private bool isStationary = false;

        void Awake()
        {
            little_bee = Instantiate(BuzzbosLair._gameObjects["Ambient Bee"], transform);
            little_bee.LocateMyFSM("Bee").enabled = false;
            little_bee.LocateMyFSM("flyer_receive_direction_msg").enabled = false;
            Destroy(little_bee.GetComponent<Rigidbody2D>());
            Destroy(little_bee.GetComponent<HealthManager>());
            Destroy(little_bee.GetComponent<DamageHero>());
            Destroy(little_bee.GetComponent<SetZ>());
            Destroy(little_bee.GetComponent<BoxCollider2D>());
            Destroy(little_bee.GetComponent<Recoil>());
            Destroy(little_bee.GetComponent<EnemyDreamnailReaction>());
            little_bee.transform.localPosition = new Vector3(0, 0, 0.01f);
            little_bee.SetActive(true);

            _alter_hm = gameObject.AddComponent<AlterHealthManager>();
            _alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();
            _alter_dreamnail = gameObject.AddComponent<AlterEnemyDreamnailReaction>();

            fsm = gameObject.GetComponent<PlayMakerFSM>();
        }

        void Start()
        {
            _alter_hm.SetGeo(0, 0, 0);
            _alter_blood.SetColor(Presets.Colors.lifeblood);
            _alter_dreamnail.SetNoReaction();
        }

        public void MakeStationary()
        {
            if (!isStationary)
            {
                IEnumerator SetStateToInit()
                {
                    yield return new WaitForEndOfFrame();
                    fsm.SetState("Init");
                }

                gameObject.GetComponent<Walker>().enabled = false;
                gameObject.GetComponent<Recoil>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

                fsm.ChangeTransition("Friendly?", "TOOK DAMAGE", "Idle");
                fsm.RemoveAction("Idle", 0);
                fsm.AddAction("Idle", fsm.GetAction<WaitRandom>("Run", 8));
                fsm.AddTransition("Idle", "FINISHED", "Hatched Amount");
                fsm.ChangeTransition("Hatched Amount", "CANCEL", "Idle");
                fsm.ChangeTransition("Anim End", "FINISHED", "Idle");

                StartCoroutine(SetStateToInit());

                isStationary = true;
            }
            else BuzzbosLair.Instance.Log(gameObject.name + "(" + this + ") is already stationary!");
        }
    }
}
