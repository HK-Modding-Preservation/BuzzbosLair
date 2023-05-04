using FriendCore;
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

        private PlayMakerFSM _control;
        private PlayMakerFSM _stun_control;

        void Awake()
        {
            _alter_hm = gameObject.AddComponent<AlterHealthManager>();
            _alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();

        }

        void Start()
        {

            SetAwakened(false);
            StartCoroutine(TestTimer());

            //_hm.hp 
            _alter_hm.SetRegen(0.5f, 0.1f, 1);

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

        IEnumerator TestTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(3);
                SetAwakened(!awakened);
            }
        }
    }
}
