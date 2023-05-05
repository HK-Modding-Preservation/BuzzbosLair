using FriendCore;
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

            
            _control.AddState("Awaken");
            _control.GetState("Awaken").AddFsmTransition("PHASE CHECK", "Phase Check");

            _control.AddState("Check Awakening");
            _control.GetState("Check Awakening").AddFsmTransition("AWAKEN", "Awaken");
            _control.GetState("Check Awakening").AddFsmTransition("FINISHED", "Phase Check");

            _control.GetState("Idle").ChangeFsmTransition("FINISHED", "Check Awakening");
            _control.GetState("Idle").ChangeFsmTransition("TOOK DAMAGE", "Check Awakening");

            _control.GetState("Check Awakening").AddMethod(() =>
            {
                if (awakened) SetAwakened(false);
                else _control.SendEvent("AWAKEN");
            });
            _control.GetState("Awaken").AddMethod(() =>
            {
                SetAwakened(true);
                _control.SendEvent("PHASE CHECK");
            });

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
