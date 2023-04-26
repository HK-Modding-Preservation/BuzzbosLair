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
    internal class Railgun : MonoBehaviour
    {

        private PlayMakerFSM _control;

        private AlterHealthManager alter_hm;
        private AlterInfectedEnemyEffects alter_blood;

        private GameObject _firing_mouth;

        public void Awake()
        {
            _control = gameObject.LocateMyFSM("Bee Stinger");

            alter_hm = gameObject.AddComponent<AlterHealthManager>();
            alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();

            _firing_mouth = new GameObject();
            _firing_mouth.transform.SetParent(gameObject.transform);
            _firing_mouth.transform.localPosition = new Vector3(0, -2, 0);

        }

        public void Start()
        {

            alter_hm.SetRegen(0.5f, 0.25f, 1);
            alter_blood.SetColor(new Color(0.957f, 0.608f, 0.212f));

            _control.GetAction<SetVelocityAsAngle>("Zing", 3).speed = 30f;
            _control.GetAction<ChaseObjectV2>("Zing", 5).accelerationForce = 100f;

            _control.AddFsmState("Firing");
            _control.AddFsmTransition("Firing", "FINISHED", "Hit");
            _control.InsertMethod("Firing", () =>
            {
                StartCoroutine(RapidFire());
                StartCoroutine(SteadyAim());
            }, 0);

            _control.AddState("Choose Move");
            _control.AddFsmTransition("Choose Move", "FIRE", "Firing");
            _control.AddFsmTransition("Choose Move", "DRILL", "Zing");
            _control.InsertMethod("Choose Move", () =>
            {
                if (UnityEngine.Random.Range(0f, 100f) < 50)
                    _control.SendEvent("FIRE");
                else _control.SendEvent("DRILL");
            }, 0);

            _control.ChangeFsmTransition("Rear Back", "FINISHED", "Choose Move");

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
                yield return new WaitForSeconds(0.05f);//0.075f);
                float rot = transform.rotation.eulerAngles.z;//Mathf.Atan2(HeroController.instance.transform.position.y - transform.position.y, HeroController.instance.transform.position.x - transform.position.x) * (180 / Mathf.PI);
                rot += UnityEngine.Random.Range(-15, 15) - 90;

                GameObject spike =  //BuzzbosLair.SpawnHoneySpike(_firing_mouth.transform.position, rot);
                    GameObject.Instantiate(
                    BuzzbosLair._gameObjects["Spiny Husk"].
                    LocateMyFSM("Attack").GetAction<FlingObjectsFromGlobalPool>("Fire", 0).gameObject.Value
                    );

                spike.SetActive(true);
                spike.transform.localPosition = _firing_mouth.transform.position;
                spike.transform.localRotation = Quaternion.Euler(0, 0, rot);

                spike.GetComponent<Rigidbody2D>().velocity = new Vector2(35f * Mathf.Cos(rot * 0.017453292f), 35f * Mathf.Sin(rot * 0.017453292f));

                //spike.AddComponent<HiveKnightStinger>();
                //spike.GetComponent<HiveKnightStinger>().direction = rot;

                //ReflectionHelper.SetField<HiveKnightStinger, float>(spike.GetComponent<HiveKnightStinger>(), "speed", 35f);
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
