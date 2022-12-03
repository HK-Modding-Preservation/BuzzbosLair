using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuzzbosLair
{
    internal class Hiveblood : MonoBehaviour
    {

        HealthManager _hm;
        private int _maxHp;

        private float _pauseTime = 1f;
        private float _rate = 0.5f;
        private int _perTick = 1;

        private int regenPausers = 0;


        public void Awake()
        {
            On.HealthManager.TakeDamage += TookDamage;

            _hm = GetComponent<HealthManager>();
            if (!_hm)
            {
                Destroy(this);
            }
            _maxHp = _hm.hp;
        }

        public void Start()
        {
            StartCoroutine(Regeneration());
        }

        public void setMaxHp (int maxHp)
        {
            _maxHp = maxHp;
        }

        public void setPauseTime (float t)
        {
            _pauseTime = t;
        }

        public void setTickRate (float r)
        {
            _rate = r;
        }

        public void setHpPerTick (int i)
        {
            _perTick = i;
        }

        private void TookDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            orig(self, hitInstance);
            if (self == _hm)
            {
                StartCoroutine(PauseRegen(_pauseTime));
            }
        }


        IEnumerator Regeneration ()
        {
            while(true)
            {
                yield return new WaitForSeconds(_rate);
                if (!(regenPausers > 0))
                {
                    _hm.hp += _perTick;
                    if (_hm.hp > _maxHp)
                        _hm.hp = _maxHp;
                }
            }
        }

        IEnumerator PauseRegen (float t)
        {
            regenPausers += 1;
            yield return new WaitForSeconds (t);
            regenPausers -= 1;
        }

    }
}
