using FriendCore;
using HutongGames.PlayMaker.Actions;
using Modding;
using SFCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuzzbosLair
{
    internal class Buzzbo : MonoBehaviour
    {

        private bool awakened;

        private int awakening_tracker = 0;

        //private HealthManager _hm;
        private AlterHealthManager _alter_hm;
        private AlterInfectedEnemyEffects _alter_blood;

        private Recoil _recoil;

        private PlayMakerFSM _control;
        private PlayMakerFSM _stun_control;

        void Awake()
        {
            _alter_hm = gameObject.AddComponent<AlterHealthManager>();
            _alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();

            _recoil = gameObject.GetComponent<Recoil>();

            _control = gameObject.LocateMyFSM("Control");
            _stun_control = gameObject.LocateMyFSM("Stun Control");

        }

        void Start()
        {

            SetAwakened(false);

            //_hm.hp 
            _alter_hm.SetRegen(0.5f, 0.1f, 1);

            _recoil.enabled = false;

            InitFSM();
        }

        private void InitFSM() {

            _stun_control.enabled = false;

            #region Create Awakened states

            // Slash Chain (SChain)
                #region Slash Chain
            _control.CopyState("TeleOut 1", "SChain Init");
            _control.CopyState("TeleOut 2", "SChain Out");
            _control.CopyState("Tele Pause", "SChain Pause");
            _control.CopyState("Tele Pos", "SChain Pos");
            _control.CopyState("Aim L", "SChain L");
            _control.CopyState("Aim R", "SChain R");
            _control.CopyState("TeleIn 1", "SChain In 1");
            _control.CopyState("TeleIn 2", "SChain In 2");
            _control.CopyState("Slash 1", "SChain Slash 1");
            _control.CopyState("Slash 2", "SChain Slash 2");
            _control.CopyState("Slash Recover", "SChain Recover");

            _control.AddState("SChain Repeat Check");

            _control.ChangeTransition("SChain Init", "FINISHED", "SChain Out");
            _control.ChangeTransition("SChain Out", "FINISHED", "SChain Pause");
            _control.ChangeTransition("SChain Pause", "FINISHED", "SChain Pos");
            _control.ChangeTransition("SChain Pos", "L", "SChain L");
            _control.ChangeTransition("SChain Pos", "R", "SChain R");
            _control.ChangeTransition("SChain L", "R", "SChain R");
            _control.ChangeTransition("SChain R", "L", "SChain L");
            _control.ChangeTransition("SChain L", "FINISHED", "SChain In 1");
            _control.ChangeTransition("SChain R", "FINISHED", "SChain In 1");
            _control.ChangeTransition("SChain In 1", "FINISHED", "SChain In 2");
            _control.ChangeTransition("SChain In 2", "FINISHED", "SChain Slash 1");
            _control.ChangeTransition("SChain Slash 1", "FINISHED", "SChain Slash 2");
            _control.ChangeTransition("SChain Slash 2", "FINISHED", "SChain Recover");
            _control.ChangeTransition("SChain Recover", "FINISHED", "SChain Repeat Check");

            _control.GetState("SChain Repeat Check").AddMethod(() =>
            {

            });

            //_control.GetAction<Wait>("SChain Pause", )

                #endregion

            // Spike Spam (SSpam)
                #region Spike Spam
            _control.CopyState("TeleOut 1", "SSpam Init");
            _control.CopyState("TeleOut 2", "SSpam Out");
            _control.CopyState("Tele Pos", "SSpam Pos");
            _control.CopyState("TeleIn 1", "SSpam In 1");
            _control.CopyState("Slash 1", "SSpam Slash");
            _control.CopyState("TeleIn 2", "SSpam End");
                #endregion

            // Dash-Teleport (Awakened Dash)

            // Scream-Jump Chain (JChain)

            // 

            #region Awakened attack select

            _control.AddState("Awakened Attack Select");
            _control.AddTransition("Awakened Attack Select", "SLASH CHAIN", "SChain Init");
            _control.GetState("Awakened Attack Select").AddMethod(() =>
            {
                _control.SendEvent("SLASH CHAIN");
            });

                #endregion

            #endregion

            #region Awakening states
            _control.AddState("Awaken");
            _control.GetState("Awaken").AddFsmTransition("AWAKENED ATTACKS", "Awakened Attack Select");

            _control.AddState("Check Awakening");
            _control.GetState("Check Awakening").AddFsmTransition("AWAKEN", "Awaken");
            _control.GetState("Check Awakening").AddFsmTransition("AWAKENED ATTACKS", "Awakened Attack Select");
            _control.GetState("Check Awakening").AddFsmTransition("FINISHED", "Phase Check");

            _control.GetState("Idle").ChangeFsmTransition("FINISHED", "Check Awakening");
            _control.GetState("Idle").ChangeFsmTransition("TOOK DAMAGE", "Check Awakening");

            _control.GetState("Check Awakening").AddMethod(() =>
            {
                awakening_tracker -= 1;

                if (awakening_tracker <= 0)
                {
                    if (awakened) SetAwakened(false);

                    if (120 + (awakening_tracker * 5) < UnityEngine.Random.Range(1, 100))
                    {
                        _control.SendEvent("AWAKEN");
                        awakening_tracker = 5;
                    }
                }
                else
                {
                    _control.SendEvent("AWAKENED ATTACKS");
                }

            });
            _control.GetState("Awaken").AddMethod(() =>
            {
                SetAwakened(true);
                _control.SendEvent("AWAKENED ATTACKS");
            });
            #endregion

            _control.AddState("TeleIn Spikes");
            _control.GetState("TeleIn Spikes").AddFsmTransition("FINISHED", "TeleIn 2");
            _control.GetState("TeleIn Spikes").AddMethod(() => { StartCoroutine(TeleportSpikes()); });

            _control.GetState("TeleIn 1").ChangeFsmTransition("FINISHED", "TeleIn Spikes");

        }

        public void SetAwakened(bool toAwakened)
        {

            if (awakened == toAwakened) return;

            if (toAwakened)
            {
                gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = BuzzbosLair.GetSprite(TextureStrings.BuzzboAwakened_Key).texture;
                _alter_blood.SetColor(Presets.Colors.lifeblood);
            }
            else
            {
                gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = BuzzbosLair.GetSprite(TextureStrings.BuzzboNormal_Key).texture;
                _alter_blood.SetColor(Presets.Colors.hiveblood);
            }

            awakened = toAwakened;

        }



        #region Coroutines

        IEnumerator TeleportSpikes()
        {
            yield return new WaitForSeconds(0.2f);

            GameObject[] Spikes = new GameObject[7];

            for (int i = 0; i < 7; i++)
            {
                Vector3 spikePos;
                float spikeRot = 0;

                switch (gameObject.transform.localScale.x)
                {
                    case -1: // Facing right
                        spikeRot = 45 - i * 22.5f;
                        break;
                    case 1: // Facing left
                        spikeRot = 135 + i * 22.5f;
                        break;
                }
                spikePos = this.transform.position;// + new Vector3(Mathf.Cos(spikeRot * 0.0174532924f), Mathf.Sin(spikeRot * 0.0174532924f))*4;

                Spikes[i] = SpawnHoneySpike(spikePos, spikeRot);

                ReflectionHelper.SetField<HiveKnightStinger, float>(Spikes[i].GetComponent<HiveKnightStinger>(), "speed", 0.80f * 25f);
            }

            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < 7; i++)
            {
                ReflectionHelper.SetField<HiveKnightStinger, float>(Spikes[i].GetComponent<HiveKnightStinger>(), "speed", 0f);
            }

            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < 7; i++)
            {
                ReflectionHelper.SetField<HiveKnightStinger, float>(Spikes[i].GetComponent<HiveKnightStinger>(), "speed", 30f);
            }

        }

        #endregion

        internal static GameObject SpawnHoneySpike(Vector3 pos, float rot)
        {
            GameObject spike = GameObject.Instantiate(BuzzbosLair._gameObjects["Honey Spike"]);
            spike.SetActive(true);
            spike.transform.localPosition = pos;
            spike.transform.localRotation = Quaternion.Euler(0, 0, rot);
            spike.GetComponent<HiveKnightStinger>().direction = rot;

            return spike;
        }

        internal static GameObject SpawnTargetedHoneySpike(Vector3 pos, Vector3 target)
        {
            float rot = Mathf.Atan2(target.y - pos.y, target.x - pos.x) * (180 / Mathf.PI);
            return SpawnHoneySpike(pos, rot);

        }


    }
}
