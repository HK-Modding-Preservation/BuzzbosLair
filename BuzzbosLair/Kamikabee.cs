using FriendCore;
using SFCore.Utils;
using UnityEngine;

namespace BuzzbosLair
{
    internal class Kamikabee : MonoBehaviour
    {

        private PlayMakerFSM _control;

        private HealthManager _hm;
        private AlterHealthManager _alter_hm;
        private AlterInfectedEnemyEffects _alter_blood;

        void Awake()
        {
            _control = gameObject.LocateMyFSM("Big Bee");

            _hm = gameObject.GetComponent<HealthManager>();
            _alter_hm = gameObject.AddComponent<AlterHealthManager>();
            _alter_blood = gameObject.AddComponent<AlterInfectedEnemyEffects>();
        }

        void Start()
        {

            //_control.GetAction<SetIntValue>("Charge Antic", 0).intValue = 100;

            _control.GetState("Hit Vertical").AddMethod(() => {
                _control.GetFloatVariable("Angle").Value += (UnityEngine.Random.Range(0, 15) - UnityEngine.Random.Range(0, 15));
            });
            _control.GetState("Hit Horizontal").AddMethod(() => {
                _control.GetFloatVariable("Angle").Value += (UnityEngine.Random.Range(0, 15) - UnityEngine.Random.Range(0, 15));
            });

            //_control.RemoveAction("Hit Vertical", 1);
            //_control.RemoveAction("Hit Horizontal", 1);
            _control.RemoveTransition("Hit Vertical", "END");
            _control.RemoveTransition("Hit Horizontal", "END");

            _control.AddState("Random Dir");
            _control.AddTransition("Random Dir", "UP", "Hit Up");
            _control.AddTransition("Random Dir", "DOWN", "Hit Down");
            _control.AddTransition("Random Dir", "RIGHT", "Hit Right");
            _control.AddTransition("Random Dir", "LEFT", "Hit Left");
            _control.AddMethod("Random Dir", () => {
                int dirNum = UnityEngine.Random.Range(0, 4);
                switch (dirNum) {
                    case 0:
                        _control.SendEvent("UP");
                        break;
                    case 1:
                        _control.SendEvent("DOWN");
                        break;
                    case 2:
                        _control.SendEvent("RIGHT");
                        break;
                    case 3:
                        _control.SendEvent("LEFT");
                        break;
                }
            });

            _control.ChangeTransition("Check Dir", "FINISHED", "Random Dir");
            /*_control.GetAction<FloatCompare>("Check Dir", 4).tolerance = 0.5f;
            _control.GetAction<FloatCompare>("Check Dir", 5).tolerance = 0.5f;
            _control.GetAction<FloatCompare>("Check Dir", 6).tolerance = 0.5f;
            _control.GetAction<FloatCompare>("Check Dir", 7).tolerance = 0.5f;*/

            _hm.hp += 70;
            _hm.SetGeoSmall(0);
            _hm.SetGeoMedium(0);
            _hm.SetGeoLarge(0);
            _alter_hm.SetMaxHp(_hm.hp);
            _alter_hm.SetRegen(1f, 1f, 5);
            _alter_blood.SetColor(Presets.Colors.hiveblood);
        }

    }
}
