using FriendCore;
using HutongGames.PlayMaker;
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
using static UnityEngine.GraphicsBuffer;

namespace BuzzbosLair
{
    internal class Buzzbo : MonoBehaviour
    {

        private bool awakened;
        private int awakening_tracker = 0;
        private int slash_chain_tracker = 3;
        private bool spike_spamming = false;
        private bool barraging = false;

        //private HealthManager _hm;
        private AlterHealthManager _alter_hm;
        private AlterInfectedEnemyEffects _alter_blood;
        private AlterEnemyDreamnailReaction _alter_dnail_reaction;

        private Recoil _recoil;
        private tk2dSpriteAnimator _anim;

        private PlayMakerFSM _control;
        private PlayMakerFSM _stun_control;
        private PlayMakerFSM _corpse;

        private Dictionary<Damagers, GameObject> _damagers;
        private GameObject _roar_emitter;
        private GameObject _shadow_recharge;

        private static float shadow_recharge_time = 1f;

        private bool _hueshifter_exists = (ModHooks.GetMod("HueShifter") != null);
        private Dictionary<string, float> _hueshifter_prev_settings = new Dictionary<string, float>();


        void Awake()
        {
            //_hm = gameObject.GetComponent<HealthManager>();
            _alter_hm = gameObject.AddComponent<AlterHealthManager>();
            _alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();
            _alter_dnail_reaction = gameObject.AddComponent<AlterEnemyDreamnailReaction>();

            _recoil = gameObject.GetComponent<Recoil>();
            _anim = gameObject.GetComponent<tk2dSpriteAnimator>();

            _damagers = new Dictionary<Damagers, GameObject>()
            {
                { Damagers.Self, gameObject },
                { Damagers.Stab, transform.Find("Stab Hit").gameObject },
                { Damagers.Slash1, transform.Find("Slash 1").gameObject },
                { Damagers.Slash2, transform.Find("Slash 2").gameObject },
            };
            
            _shadow_recharge = Instantiate(HeroController.instance.gameObject.transform.Find("Effects/Shadow Recharge").gameObject, this.gameObject.transform);
            _shadow_recharge.transform.localScale *= 3;
            _shadow_recharge.LocateMyFSM("Recharge Effect").FsmVariables.GetFsmFloat("Shadow Recharge Time").Value = shadow_recharge_time;


            _control = gameObject.LocateMyFSM("Control");
            _stun_control = gameObject.LocateMyFSM("Stun Control");
            _corpse = gameObject.Find("Corpse Hive Knight(Clone)").LocateMyFSM("corpse");

            if (_hueshifter_exists)
            {
                HueShifter_RecordSettings();
            }

        }

        void Start()
        {

            SetAwakened(false);

            _alter_hm.hp = (_alter_hm.hp / 2) * 3 + 200;//_hm.hp = (_hm.hp / 2) * 3 + 200;
            _alter_hm.maxHp = _alter_hm.hp; // _alter_hm.SetMaxHp(_hm.hp);
            _alter_hm.SetRegen(0.5f, 0.1f, 1);

            if (gameObject.scene.name == "GG_Hive_Knight")
                _alter_dnail_reaction.SetConvoTitle("BUZZBO_GG");

            _recoil.enabled = false;

            InitFSM();

            _corpse.ChangeFsmTransition("Pause", "FINISHED", "Appear Pause");
            Destroy(gameObject.GetComponent<EnemyDeathEffects>());
            gameObject.AddComponent<EnemyDeathEffectsNoEffect>();
        }

        private void InitFSM() {

            _stun_control.enabled = false;

            #region Intro
            _control.GetState("Fall").InsertMethod(() =>
            {
                if (HeroController.instance.transform.position.x > 68.95f)
                {
                    transform.SetPositionX(63f);
                    transform.localScale = new Vector3(-1f, 1);
                }
            }, 0);

            _control.InsertAction("Intro", new SendEventByName()
            {
                eventTarget = new FsmEventTarget()
                {
                    target = FsmEventTarget.EventTarget.GameObject,
                    gameObject = new FsmOwnerDefault()
                    {
                        OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                        GameObject = HeroController.instance.gameObject
                    }
                },
                sendEvent = "ROAR ENTER",
                delay = 0f,
                everyFrame = false
            }, 2);
            _control.AddMethod("Intro", () => {
                _roar_emitter = GameObject.Instantiate(_control.FsmVariables.GetFsmGameObject("Roar Emitter").Value);
                _roar_emitter.SetActive(false);
            });
            _control.InsertAction("Intro End", new SendEventByName()
            {
                eventTarget = new FsmEventTarget()
                {
                    target = FsmEventTarget.EventTarget.GameObject,
                    gameObject = new FsmOwnerDefault()
                    {
                        OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                        GameObject = HeroController.instance.gameObject
                    }
                },
                sendEvent = "ROAR EXIT",
                delay = 0f,
                everyFrame = false
            }, 1);
            #endregion Roar stun

            #region Awakened attack states

                #region Slash Chain (SChain)

            // Create attack
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

            _control.AddTransition("SChain Repeat Check", "END", "Start Fall");
            _control.AddTransition("SChain Repeat Check", "CONTINUE", "SChain Out");

            _control.GetState("SChain Repeat Check").AddMethod(() =>
            {
                slash_chain_tracker -- ;

                if (slash_chain_tracker <= 0)
                {
                    slash_chain_tracker = 3;
                    _control.SendEvent("END");
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    _control.SendEvent("CONTINUE");
                }
            });

            // Edit values
            _control.GetAction<Wait>("SChain Pause", 0).time = 0.05f;
            _control.GetAction<Wait>("SChain In 2", 1).time = 0.3f;
            _control.GetAction<FloatOperator>("SChain Slash 1", 4).float1 = -54f;
            _control.GetAction<DecelerateXY>("SChain Recover", 2).decelerationX = 0.8f;

                #endregion

                #region Spike Spam (SSpam)
            _control.CopyState("TeleOut 1", "SSpam Init");
            _control.CopyState("TeleOut 2", "SSpam Out");
            _control.CopyState("Tele Pos", "SSpam Pos");
            _control.CopyState("TeleIn 1", "SSpam In");
            _control.CopyState("Slash 1", "SSpam Slash");
            _control.CopyState("TeleIn 2", "SSpam End");

            _control.ChangeTransition("SSpam Init", "FINISHED", "SSpam Out");
            _control.ChangeTransition("SSpam Out", "FINISHED", "SSpam Pos");
            _control.ChangeTransition("SSpam In", "FINISHED", "SSpam Slash");
            _control.ChangeTransition("SSpam End", "FINISHED", "SChain Slash 2");   

            _control.RemoveTransition("SSpam Pos", "L");
            _control.RemoveTransition("SSpam Pos", "R");
            _control.RemoveTransition("SSpam Slash", "FINISHED");

            _control.AddTransition("SSpam Pos", "POSITIONED", "SSpam In");
            _control.AddTransition("SSpam Slash", "CONTINUE", "SSpam Out");
            _control.AddTransition("SSpam Slash", "END", "SSpam End");

            _control.GetState("SSpam Init").AddMethod(() =>
            {
                StartCoroutine(SSpamTimer(7f));

            });
            _control.GetState("SSpam Pos").AddMethod(() =>
            {
                float _distance = 0;
                float xPos = 69.2f;
                float yPos = 32;
                while (_distance < 6.5f)
                {
                    xPos = UnityEngine.Random.Range(58.3f, 79.6f);
                    yPos = UnityEngine.Random.Range(27.3f, 40);
                    _distance = Mathf.Sqrt((xPos - HeroController.instance.transform.position.x) * (xPos - HeroController.instance.transform.position.x) + (yPos - HeroController.instance.transform.position.y) * (yPos - HeroController.instance.transform.position.y));
                }
                _control.FsmVariables.GetFsmFloat("X Pos").Value = xPos;
                _control.FsmVariables.GetFsmFloat("Y Pos").Value = yPos;
                _control.SendEvent("POSITIONED");
            });
            _control.GetState("SSpam Slash").AddMethod(() =>
            {
                StartCoroutine(SSpamSlash());
            });
            _control.GetAction<ActivateGameObject>("SSpam Slash", 2).activate = false;
            _control.GetAction<SetVelocity2d>("SSpam Slash", 5).x = 0;
            _control.GetState("SSpam End").RemoveAction(2);
            _control.GetState("SSpam End").RemoveAction(1);

                #endregion

                #region Dash-Teleport (Awakened Dash)

            // Create attack
            _control.CopyState("Dash Antic", "Awakened Dash Antic");
            _control.CopyState("Dash", "Awakened Dash");

            _control.AddState("Awakened Dash Tele");

            _control.ChangeTransition("Awakened Dash Antic", "FINISHED", "Awakened Dash");
            _control.ChangeTransition("Awakened Dash", "FINISHED", "Awakened Dash Tele");
            
            _control.AddTransition("Awakened Dash Tele", "TELEPORT", "SChain Out");

            _control.GetState("Awakened Dash Tele").AddMethod(() =>
            {
                slash_chain_tracker = 1;
                _damagers[Damagers.Stab].gameObject.SetActive(false);
                GetComponent<Rigidbody2D>().gravityScale = 0f;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                _control.SendEvent("TELEPORT");
            });

            // Edit values
            _control.GetState("Awakened Dash Antic").InsertMethod(() =>
            {
                GetComponent<tk2dSpriteAnimator>().Library.GetClipByName("Stab Antic").fps = 20;
            }, 0);
            _control.GetAction<FloatOperator>("Awakened Dash", 1).float1 = -75f;

            #endregion

                #region Barrage-Slash (Barrage)
            _control.CopyState("TeleOut 1", "Barrage Init");
            _control.CopyState("TeleOut 2", "Barrage Out");
            _control.CopyState("Tele Pos", "Barrage Pos");
            _control.CopyState("TeleIn 1", "Barrage In 1");
            _control.CopyState("TeleIn 2", "Barrage In 2");
            _control.CopyState("Slash 1", "Barrage Slash 1");
            _control.CopyState("Slash 2", "Barrage Slash 2");
            _control.CopyState("Slash Recover", "Barrage Slash Recover");

            _control.ChangeTransition("Barrage Init", "FINISHED", "Barrage Out");
            _control.ChangeTransition("Barrage Out", "FINISHED", "Barrage Pos");
            _control.ChangeTransition("Barrage In 1", "FINISHED", "Barrage In 2");
            _control.ChangeTransition("Barrage In 2", "FINISHED", "Barrage Slash 1");
            _control.ChangeTransition("Barrage Slash 1", "FINISHED", "Barrage Slash 2");
            _control.ChangeTransition("Barrage Slash 2", "FINISHED", "Barrage Slash Recover");

            _control.RemoveTransition("Barrage Pos", "L");
            _control.RemoveTransition("Barrage Pos", "R");
            _control.RemoveTransition("Barrage Slash Recover", "FINISHED");

            _control.AddTransition("Barrage Pos", "POSITIONED", "Barrage In 1");
            _control.AddTransition("Barrage Slash Recover", "CONTINUE", "Barrage Slash 1");
            _control.AddTransition("Barrage Slash Recover", "END", "SChain Out");

            _control.GetState("Barrage Init").AddMethod(() =>
            {
                StartCoroutine(BarrageTimer(3f));

            });
            _control.GetState("Barrage Pos").AddMethod(() =>
            {
                float _distance = 0;
                float xPos = 69.2f;
                float yPos = 32;
                while (_distance < 8.5f)
                {
                    xPos = UnityEngine.Random.Range(58.3f, 79.6f);
                    yPos = UnityEngine.Random.Range(27.3f, 40);
                    _distance = Mathf.Sqrt((xPos - HeroController.instance.transform.position.x) * (xPos - HeroController.instance.transform.position.x) + (yPos - HeroController.instance.transform.position.y) * (yPos - HeroController.instance.transform.position.y));
                }
                _control.FsmVariables.GetFsmFloat("X Pos").Value = xPos;
                _control.FsmVariables.GetFsmFloat("Y Pos").Value = yPos;
                _control.SendEvent("POSITIONED");
            });
            _control.GetState("Barrage Slash Recover").AddMethod(() =>
            {
                StartCoroutine(BarrageSlash(30f, 3));
            });

            _control.RemoveAction("Barrage Slash 1", 5);

            #endregion

                #region Awakened attack select
            /*_control.AddState("Awakened Attack Select");
            _control.AddTransition("Awakened Attack Select", "SLASH CHAIN", "SChain Init");
            _control.AddTransition("Awakened Attack Select", "DASH TELEPORT", "Awakened Dash Antic");
            _control.AddTransition("Awakened Attack Select", "SPIKE SPAM", "SSpam Init");

            _control.GetState("Awakened Attack Select").AddMethod(() =>
            {

                float roll = UnityEngine.Random.Range(0f, 100f);
                if (roll < 60)
                {
                    _control.SendEvent("DASH TELEPORT");
                }
                else if (roll < 90)
                {
                    _control.SendEvent("SLASH CHAIN");
                }
                else
                {
                    _control.SendEvent("SPIKE SPAM");
                }
                
            });*/

            _control.CopyState("Phase 3", "Awakened Attack Select");
            //_control.RemoveAction("Awakened Attack Select", 0);
            _control.ChangeFsmTransition("Awakened Attack Select", "DASH", "Awakened Dash Antic");
            _control.ChangeFsmTransition("Awakened Attack Select", "TELE", "SChain Init");
            _control.ChangeFsmTransition("Awakened Attack Select", "GLOB", "Barrage Init");
            _control.ChangeFsmTransition("Awakened Attack Select", "BEE ROAR", "SSpam Init");
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).weights[0] = 0.40f;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).weights[1] = 0.30f;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).weights[2] = 0.20f;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).weights[3] = 0.10f;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).eventMax[0] = 3;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).eventMax[1] = 2;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).eventMax[2] = 1;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).eventMax[3] = 1;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).missedMax[0] = 3;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).missedMax[1] = 3;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).missedMax[2] = 5;
            _control.GetAction<SendRandomEventV3>("Awakened Attack Select", 1).missedMax[3] = 7;
            _control.RemoveAction("Awakened Attack Select", 0);
            #endregion

            #endregion

            #region Awakening management states
            _control.AddState("Awaken");
            _control.GetState("Awaken").AddFsmTransition("AWAKENED ATTACKS", "Awakened Attack Select");

            _control.AddState("Check Awakening");
            _control.GetState("Check Awakening").AddFsmTransition("AWAKEN", "Awaken");
            _control.GetState("Check Awakening").AddFsmTransition("AWAKENED ATTACKS", "Awakened Attack Select");
            _control.GetState("Check Awakening").AddFsmTransition("FINISHED", "Phase Check");

            _control.GetState("Idle").ChangeFsmTransition("FINISHED", "Check Awakening");
            _control.GetState("Idle").ChangeFsmTransition("TOOK DAMAGE", "Check Awakening");
            _control.GetState("Land").AddFsmTransition("SKIP IDLE", "Check Awakening");
            _control.GetState("Land").AddMethod(() => { if (awakened) _control.SendEvent("SKIP IDLE"); });

            _control.GetState("Check Awakening").AddMethod(() =>
            {
                awakening_tracker -= 1;

                if (awakening_tracker <= 0)
                {
                    if (awakened) SetAwakened(false);

                    if (125 + (awakening_tracker * 5) < UnityEngine.Random.Range(1, 100))
                    {
                        _control.SendEvent("AWAKEN");
                    }
                }
                else
                {
                    _control.SendEvent("AWAKENED ATTACKS");
                }

            });
            _control.GetState("Awaken").AddMethod(() => { StartCoroutine(AwakeningSequence()); });
            #endregion

            #region Regular attack edits

                #region Teleport spikes
            _control.AddState("TeleIn Spikes");
            _control.GetState("TeleIn Spikes").AddFsmTransition("FINISHED", "TeleIn 2");
            _control.GetState("TeleIn Spikes").AddMethod(() => { StartCoroutine(TeleportSpikes()); });

            _control.GetState("TeleIn 1").ChangeFsmTransition("FINISHED", "TeleIn Spikes");
                #endregion

                #region Dash Spikes
            _control.AddState("Dash Spikes");
            _control.GetState("Dash Spikes").AddFsmTransition("FINISHED", "Dash");
            _control.GetState("Dash Spikes").AddMethod(() => { StartCoroutine(DashSpikes()); });

            _control.GetState("Dash Antic").InsertMethod(() =>
            {
                GetComponent<tk2dSpriteAnimator>().Library.GetClipByName("Stab Antic").fps = 15;
            }, 0);
            _control.GetState("Dash Antic").ChangeFsmTransition("FINISHED", "Dash Spikes");
                #endregion

                #region Jump Spikes
            _control.AddState("Jump Spikes");
            _control.GetState("Jump Spikes").AddFsmTransition("FINISHED", "In Air");
            _control.GetState("Jump Spikes").AddMethod(() => { StartCoroutine(JumpSpikes()); });

            _control.GetState("Jump").ChangeFsmTransition("FINISHED", "Jump Spikes");
                #endregion

                #region Floor Strike
            _control.GetState("Glob Antic 2").AddMethod(() =>
            {
                int count = 20;
                float rot_offset = UnityEngine.Random.Range(0, 360);
                for (int i = 0; i < count; i++)
                {
                    SpawnHoneySpike(gameObject.Find("Stinger Flash").transform.position, (360/count * i) + rot_offset);
                }
            });
            _control.GetState("Glob Strike").ChangeFsmTransition("FINISHED", "Jump Antic");
            _control.GetState("Glob Strike").RemoveAction(3);
            _control.GetState("Glob Strike").AddMethod(() =>
            {
                Vector3 pos = transform.position;

                bool[] facingRightBools = { false, true };
                foreach (bool @bool in facingRightBools)
                {
                    GameObject shockwave = Instantiate(BuzzbosLair._gameObjects["Grey Prince Zote"].LocateMyFSM("Control").GetAction<SpawnObjectFromGlobalPool>("Land Waves", 0).gameObject.Value);
                    PlayMakerFSM shockFSM = shockwave.LocateMyFSM("shockwave");
                    shockFSM.transform.localScale = new Vector2(1.2f, 1f);
                    shockFSM.FsmVariables.FindFsmBool("Facing Right").Value = @bool;
                    shockFSM.FsmVariables.FindFsmFloat("Speed").Value = 20f;
                    shockwave.AddComponent<DamageHero>().damageDealt = 1;

                    shockwave.SetActive(true);
                    shockwave.transform.SetPosition2D(new Vector2(pos.x, pos.y - 2.8f));
                }

            });
            #endregion

            #endregion

            _control.FsmVariables.FindFsmFloat("Idle Time").Value = 0.25f;
            _control.RemoveAction("Phase 2", 0);
            _control.ChangeFsmTransition("Phase 3", "BEE ROAR", "Phase 2");

        }

        public void SetAwakened(bool toAwakened)
        {

            //if (awakened == toAwakened) return;

            if (toAwakened)
            {
                foreach(GameObject damager in _damagers.Values)
                {
                    damager.GetComponent<DamageHero>().damageDealt = 2;
                }
                gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = BuzzbosLair.GetSprite(TextureStrings.BuzzboAwakened_Key).texture;
                _alter_blood.SetColor(Presets.Colors.lifeblood);
            }
            else
            {
                foreach (GameObject damager in _damagers.Values)
                {
                    damager.GetComponent<DamageHero>().damageDealt = 1;
                }
                gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = BuzzbosLair.GetSprite(TextureStrings.BuzzboNormal_Key).texture;
                _alter_blood.SetColor(Presets.Colors.hiveblood);
            }

            if (_hueshifter_exists)
            {
                HueShifter_AwakenToggle(toAwakened);
            }

            awakened = toAwakened;

        }

        #region Coroutines

        IEnumerator AwakeningSequence()
        {

            transform.GetComponent<BoxCollider2D>().enabled = false;
            transform.GetComponent<Rigidbody2D>().isKinematic = true;

            _anim.Play("Intro");

            GameObject roar = Instantiate(_roar_emitter);
            roar.transform.SetParent(transform);
            roar.transform.localPosition = Vector3.zero;
            roar.SetActive(true);

            _shadow_recharge.SetActive(true);
            yield return new WaitForSeconds(shadow_recharge_time);
            SetAwakened(true);
            awakening_tracker = UnityEngine.Random.Range(8, 11);

            yield return new WaitForSeconds(1f);

            roar.LocateMyFSM("emitter").SendEvent("END");
            _anim.Play("Recover");
            transform.GetComponent<BoxCollider2D>().enabled = true;
            transform.GetComponent<Rigidbody2D>().isKinematic = false;

            _control.SendEvent("AWAKENED ATTACKS");

        }

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

        IEnumerator DashSpikes()
        {
            bool odd = true;
            while (_control.ActiveStateName == "Dash Spikes" || _control.ActiveStateName == "Dash")
            {
                for (int i = 0; i < (odd?5:4); i++)
                {
                    float spikeRot = 0;
                    switch (gameObject.transform.localScale.x)
                    {
                        case 1: // Facing left
                            spikeRot = (odd?(45 - i * 22.5f):(33.75f - i * 22.5f));
                            break;
                        case -1: // Facing right
                            spikeRot = (odd?(135 + i * 22.5f):(146.25f + i * 22.5f));
                            break;
                    }
                    GameObject spike = SpawnHoneySpike(transform.position, spikeRot);
                    ReflectionHelper.SetField<HiveKnightStinger, float>(spike.GetComponent<HiveKnightStinger>(), "speed", 35f);
                }
                odd = !odd;
                yield return new WaitForSeconds(0.04f);
            }
        }

        IEnumerator JumpSpikes()
        {
            while (_control.ActiveStateName == "Jump Spikes" || _control.ActiveStateName == "In Air")
            {
                GameObject spike = SpawnTargetedHoneySpike(transform.position, HeroController.instance.transform.position);
                ReflectionHelper.SetField<HiveKnightStinger, float>(spike.GetComponent<HiveKnightStinger>(), "speed", 35f);
                yield return new WaitForSeconds(0.08f);
            }
        }

        IEnumerator SSpamTimer(float t)
        {
            _alter_hm.SetRegenEnabled(false);
            spike_spamming = true;
            yield return new WaitForSeconds(t);
            spike_spamming = false;
            _alter_hm.SetRegenEnabled(true);
        }

        IEnumerator SSpamSlash()
        {
            yield return new WaitForSeconds(0.02f);
            GameObject spike = SpawnTargetedHoneySpike(
                transform.position,
                HeroController.instance.transform.position
                );
            
            yield return new WaitForSeconds(0.03f);
            if(spike_spamming)
            {
                _control.SendEvent("CONTINUE");
            }
            else
            {
                slash_chain_tracker = 0;
                _control.SendEvent("END");
            }
        }

        IEnumerator BarrageTimer(float t)
        {
            barraging = true;
            yield return new WaitForSeconds(t);
            barraging = false;
        }

        IEnumerator BarrageSlash(float spread, int count)
        {
            yield return new WaitForSeconds(0.02f);
            for (int i = 0; i < count; i++)
            {
                GameObject spike = SpawnTargetedHoneySpike(
                transform.position,
                HeroController.instance.transform.position,
                spread
                );
            }
            yield return new WaitForSeconds(0.03f);
            if (barraging)
            {
                _control.SendEvent("CONTINUE");
            }
            else
            {
                slash_chain_tracker = 1;
                _control.SendEvent("END");
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

        internal static GameObject SpawnTargetedHoneySpike(Vector3 pos, Vector3 target, float deviation=0)
        {
            float rot = Mathf.Atan2(target.y - pos.y, target.x - pos.x) * (180 / Mathf.PI);
            rot += UnityEngine.Random.Range(-deviation, deviation);
            return SpawnHoneySpike(pos, rot);

        }

        private enum Damagers
        {
            Self,
            Stab,
            Slash1,
            Slash2,
        }

        
        private void HueShifter_RecordSettings ()
        {
            var current_settings = HueShifter.HueShifter.Instance.GS;
            _hueshifter_prev_settings.Add("Phase", current_settings.Phase);
            _hueshifter_prev_settings.Add("RandomPhase",
                current_settings.RandomPhase == HueShifter.RandomPhaseSetting.Fixed ? 0f : (
                current_settings.RandomPhase == HueShifter.RandomPhaseSetting.RandomPerMapArea ? 1f : 2f
                ));
            _hueshifter_prev_settings.Add("ShiftLighting", current_settings.ShiftLighting ? 1f : 0f);
            _hueshifter_prev_settings.Add("RespectLighting", current_settings.RespectLighting ? 1f : 0f);
            _hueshifter_prev_settings.Add("XFrequency", current_settings.XFrequency);
            _hueshifter_prev_settings.Add("YFrequency", current_settings.YFrequency);
            _hueshifter_prev_settings.Add("ZFrequency", current_settings.ZFrequency);
            _hueshifter_prev_settings.Add("TimeFrequency", current_settings.TimeFrequency);
            _hueshifter_prev_settings.Add("AllowVanillaPhase", current_settings.AllowVanillaPhase ? 1f : 0f);
        }

        private void HueShifter_AwakenToggle (bool awakened)
        {
            HueShifter.HueShifter _hs = HueShifter.HueShifter.Instance;
            _hs.GS.RandomPhase = HueShifter.RandomPhaseSetting.Fixed;
            _hs.GS.Phase = (awakened ? 180 : 0);
            _hs.GS.ShiftLighting = true;
            _hs.GS.RespectLighting = true;
            _hs.GS.XFrequency = 0;
            _hs.GS.YFrequency = 0;
            _hs.GS.ZFrequency = 0;
            _hs.GS.TimeFrequency = 0;
            ReflectionHelper.CallMethod(_hs, "SetAllTheShaders");
        }

        private void HueShifter_ResetSettings ()
        {
            HueShifter.HueShifter _hs = HueShifter.HueShifter.Instance;
            _hs.GS.RandomPhase = (_hueshifter_prev_settings["RandomPhase"] == 0 ? HueShifter.RandomPhaseSetting.Fixed:(
                _hueshifter_prev_settings["RandomPhase"] == 1f ? HueShifter.RandomPhaseSetting.RandomPerMapArea : HueShifter.RandomPhaseSetting.RandomPerRoom)) ;
            _hs.GS.Phase = _hueshifter_prev_settings["Phase"];
            _hs.GS.ShiftLighting = _hueshifter_prev_settings["ShiftLighting"] == 1f;
            _hs.GS.RespectLighting = _hueshifter_prev_settings["RespectLighting"] == 1f;
            _hs.GS.XFrequency = _hueshifter_prev_settings["XFrequency"];
            _hs.GS.YFrequency = _hueshifter_prev_settings["YFrequency"];
            _hs.GS.ZFrequency = _hueshifter_prev_settings["ZFrequency"];
            _hs.GS.TimeFrequency = _hueshifter_prev_settings["TimeFrequency"];
        }

        void OnDestroy()
        {
            SetAwakened(false);

            if (_hueshifter_exists)
            {
                HueShifter_ResetSettings();
            }

        }
    }
}
