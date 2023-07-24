using FriendCore;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using System.Collections;
using UnityEngine;

namespace BuzzbosLair
{
    internal class Railgun : MonoBehaviour
    {

        private PlayMakerFSM _control;

        private HealthManager _hm;
        private AlterHealthManager _alter_hm;
        private AlterInfectedEnemyEffects _alter_blood;

        private GameObject _firing_mouth;

        void Awake()
        {
            _control = gameObject.LocateMyFSM("Bee Stinger");

            _hm = gameObject.GetComponent<HealthManager>();
            _alter_hm = gameObject.AddComponent<AlterHealthManager>();
            _alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();

            _firing_mouth = new GameObject();
            _firing_mouth.transform.SetParent(gameObject.transform);
            _firing_mouth.transform.localPosition = new Vector3(0, -2, 0);

        }

        void Start()
        {
            _hm.hp *= 2;
            _hm.SetGeoSmall(0);
            _hm.SetGeoMedium(0);
            _hm.SetGeoLarge(0);
            _alter_hm.SetMaxHp(_hm.hp);
            _alter_hm.SetRegen(0.5f, 0.25f, 1);
            _alter_blood.SetColor(Presets.Colors.hiveblood);

            _control.GetAction<SetVelocityAsAngle>("Zing", 3).speed = 30f;
            _control.GetAction<ChaseObjectV2>("Zing", 5).accelerationForce = 100f;

            _control.CopyFsmState("Hit", "Firing End");

            _control.AddFsmState("Firing");
            _control.AddFsmTransition("Firing", "FINISHED", "Firing End");
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

            _control.GetAction<WaitRandom>("Hit", 8).timeMin = 0.2f;
            _control.GetAction<WaitRandom>("Hit", 8).timeMax = 0.3f;

        }

        IEnumerator RapidFire()
        {
            while (_control.ActiveStateName == "Firing" || _control.ActiveStateName == "Firing End")
            {
                yield return new WaitForSeconds(0.05f);
                float rot = transform.rotation.eulerAngles.z;
                rot += UnityEngine.Random.Range(-15, 15) - 90;

                GameObject spike = 
                    GameObject.Instantiate(
                    BuzzbosLair._gameObjects["Spiny Husk"].
                    LocateMyFSM("Attack").GetAction<FlingObjectsFromGlobalPool>("Fire", 0).gameObject.Value
                    );

                spike.SetActive(true);
                spike.transform.localPosition = _firing_mouth.transform.position;
                spike.transform.localRotation = Quaternion.Euler(0, 0, rot);

                spike.GetComponent<Rigidbody2D>().velocity = 35f * new Vector2(Mathf.Cos(rot * 0.017453292f), Mathf.Sin(rot * 0.017453292f));

            }
        }

        IEnumerator SteadyAim()
        {
            while (_control.ActiveStateName == "Firing" || _control.ActiveStateName == "Firing End")
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
