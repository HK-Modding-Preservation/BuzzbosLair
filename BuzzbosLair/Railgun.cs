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
    internal class Railgun : MonoBehaviour
    {

        private PlayMakerFSM _control;

        public void Awake()
        {
            _control = gameObject.LocateMyFSM("Bee Stinger");
        }

        public void Start()
        {
            _control.AddFsmState("Firing");
            _control.AddFsmTransition("Firing", "FINISHED", "Hit");
            _control.ChangeFsmTransition("Rear Back", "FINISHED", "Firing");
            _control.InsertMethod("Firing", () =>
            {
                StartCoroutine(RapidFire());
                StartCoroutine(SteadyAim());
            }, 0);

            _control.GetAction<DistanceFly>("Chase", 1).distance = 14;
            _control.GetAction<DistanceFly>("Antic", 2).distance = 16;
            _control.GetAction<RandomFloat>("Pointing", 3).min = 2f;
            _control.GetAction<RandomFloat>("Pointing", 3).max = 10f;
            _control.GetAction<DistanceFly>("Pointing", 4).distance = 16;
        }

        IEnumerator RapidFire()
        {
            while (_control.ActiveStateName == "Firing" || _control.ActiveStateName == "Hit")
            {
                yield return new WaitForSeconds(0.075f);
                float rot = transform.rotation.eulerAngles.z;//Mathf.Atan2(HeroController.instance.transform.position.y - transform.position.y, HeroController.instance.transform.position.x - transform.position.x) * (180 / Mathf.PI);
                rot += UnityEngine.Random.Range(-15, 15) - 90;
                GameObject spike = BuzzbosLair.SpawnHoneySpike(transform.position, rot);
                ReflectionHelper.SetField<HiveKnightStinger, float>(spike.GetComponent<HiveKnightStinger>(), "speed", 35f);
            }
        }

        IEnumerator SteadyAim()
        {
            while (_control.ActiveStateName == "Firing" || _control.ActiveStateName == "Hit")
            {
                yield return new WaitForSeconds(0.04f);
                float currentAngle = transform.rotation.eulerAngles.z;
                float targetAngle = Mathf.Atan2(HeroController.instance.transform.position.y - transform.position.y, HeroController.instance.transform.position.x - transform.position.x) * (180 / Mathf.PI);
                float rot = Mathf.Clamp(((currentAngle - targetAngle + 180 + 90)%360) - 180, -5, 5);
                transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentAngle+rot));
            }
        }
    }
}
