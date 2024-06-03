
using HutongGames.PlayMaker.Actions;
using Modding.Utils;
using SFCore.Utils;
using System;
using UnityEngine;

namespace BuzzbosLair
{
    internal class SceneHandler
    {
        private static GameObject spike_hitbox = BuzzbosLair._gameObjects["Hive Floor Spike Hitbox"];
        private static GameObject spike_sprite = BuzzbosLair._gameObjects["Hive Floor Spike Sprite"];
        private static GameObject spike_shadow = BuzzbosLair._gameObjects["Hive Floor Spike Shadow"];

        private static ValueTuple<GameObject, Vector3, Quaternion, Vector3>[] spike_related_spawns =
        {
            (spike_hitbox, new Vector3(150.7f, 89.7f), spike_hitbox.transform.rotation, new Vector3(-1f, -0.8f, 1f)),
            (spike_hitbox, new Vector3(171.7f, 89.7f), spike_hitbox.transform.rotation, new Vector3(-0.5f, -0.75f, 1f)),
            //(spike_hitbox, new Vector3(192.7f, 89.7f), spike_hitbox.transform.rotation, new Vector3(-0.5f, 1f, 1f)),

            (BuzzbosLair._gameObjects["Hazard Respawn Trigger"], new Vector3(143f, 114f), Quaternion.Euler(Vector3.zero), Vector3.one),
            (BuzzbosLair._gameObjects["Hazard Respawn Trigger"], new Vector3(205f, 103f), Quaternion.Euler(Vector3.zero), Vector3.one),

            // Left block spikes
            (spike_sprite, new Vector3(158.8f, 91f, -0.2f), Quaternion.Euler(new Vector3(0,0, 10f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(150.7f, 89.7f, -1f), spike_sprite.transform.rotation, spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(152.5f, 89.7f, -1f), Quaternion.Euler(new Vector3(0,0, 5f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(148.7f, 90.2f, -1f), Quaternion.Euler(new Vector3(0,0, 340f)), new Vector3(-spike_sprite.transform.localScale.x, spike_sprite.transform.localScale.y, spike_sprite.transform.localScale.z)),
            (spike_sprite, new Vector3(155f, 89.7f, -1f), Quaternion.Euler(new Vector3(0,0, 20f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(154f, 89.7f, -0.2f), Quaternion.Euler(new Vector3(0,0, 10f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(155.2f, 89.2f, -1f), Quaternion.Euler(new Vector3(0,0, 20f)), new Vector3(-spike_sprite.transform.localScale.x, spike_sprite.transform.localScale.y, spike_sprite.transform.localScale.z)),
            (spike_sprite, new Vector3(159f, 90.5f), Quaternion.Euler(Vector3.zero), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(138.3f, 91.7f, -1f), Quaternion.Euler(new Vector3(0,0, 310f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(139f, 91f, -0.5f), Quaternion.Euler(new Vector3(0,0, 320f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(140.7f, 91f, -1f), Quaternion.Euler(Vector3.zero), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(146.7f, 90.2f, -1f), Quaternion.Euler(new Vector3(0,0,4f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(144.8f, 90.2f, -1f), Quaternion.Euler(new Vector3(0,0,356f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(142.7f, 90.6f, -1f), Quaternion.Euler(new Vector3(0,0,348f)), spike_sprite.transform.localScale),

            // Left block shadows
            (spike_shadow, new Vector3(137.7f, 92f, -5.08f), Quaternion.Euler(new Vector3(0,0,310f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(139.2f, 90.5f, -5.08f), Quaternion.Euler(new Vector3(0,0,320f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(138.2f, 92.4f, -3f), Quaternion.Euler(new Vector3(0,0,300f)), new Vector3(1.6f, 1.8f, spike_shadow.transform.localScale.z)),
            (spike_shadow, new Vector3(140.7f, 91f, -4f), Quaternion.Euler(new Vector3(0,0,350f)), new Vector3(1.7f, 1.7f, spike_shadow.transform.localScale.z)),
            (spike_shadow, new Vector3(139.7f, 91f, -5.08f), Quaternion.Euler(new Vector3(0,0,340f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(155f, 89.8f, -5.08f), Quaternion.Euler(new Vector3(0,0,15f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(157f, 89.8f, -5.08f), Quaternion.Euler(new Vector3(0,0,20f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(158.5f, 90.7f, -5.08f), Quaternion.Euler(Vector3.zero), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(160.8f, 90.2f, -5.08f), Quaternion.Euler(new Vector3(0,0,350f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(153f, 89.6f, -5.08f), Quaternion.Euler(new Vector3(0,0,5f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(150.7f, 89.7f, -5.08f), Quaternion.Euler(new Vector3(0,0,358f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(149.2f, 89.7f, -5.08f), Quaternion.Euler(new Vector3(0,0,349f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(147f, 90.1f, -5.08f), Quaternion.Euler(new Vector3(0,0,3f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(145f, 90f, -5.08f), Quaternion.Euler(new Vector3(0,0,355f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(143.3f, 90.6f, -5.08f), Quaternion.Euler(new Vector3(0,0,350f)), spike_shadow.transform.localScale),
            (spike_shadow, new Vector3(145.5f, 88f, -5.08f), Quaternion.Euler(new Vector3(0,0,270)), new Vector3(spike_shadow.transform.localScale.x, 4f, spike_shadow.transform.localScale.z)),

            // Middle block sprites
            (spike_sprite, new Vector3(162f, 89.6f, -0.2f), Quaternion.Euler(new Vector3(0,0, 5f)), new Vector3(-spike_sprite.transform.localScale.x, spike_sprite.transform.localScale.y, spike_sprite.transform.localScale.z)),
            (spike_sprite, new Vector3(164f, 89.5f, -0.2f), Quaternion.Euler(new Vector3(0,0,350f)), new Vector3(-spike_sprite.transform.localScale.x, spike_sprite.transform.localScale.y, spike_sprite.transform.localScale.z)),
            (spike_sprite, new Vector3(166.2f, 89.4f, -0.2f), Quaternion.Euler(new Vector3(0,0, 355f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(167.45f, 89.7f, -0.2f), Quaternion.Euler(new Vector3(0,0,355f)), new Vector3(-spike_sprite.transform.localScale.x, spike_sprite.transform.localScale.y, spike_sprite.transform.localScale.z)),
            (spike_sprite, new Vector3(169.4f, 89.3f, -0.2f), Quaternion.Euler(new Vector3(0,0, 350f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(171.2f, 89.1f, -0.2f), Quaternion.Euler(new Vector3(0,0, 10f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(173f, 89.1f, -0.2f), Quaternion.Euler(new Vector3(0,0, 348f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(175f, 89.5f, -0.2f), Quaternion.Euler(new Vector3(0,0, 10f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(177f, 89.6f, -0.2f), Quaternion.Euler(new Vector3(0,0, 20f)), spike_sprite.transform.localScale),

        };

        internal static void SceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            switch (arg1.name)
            {
                case "Hive_05":
                    GameObject _battleScene = arg1.Find("Battle Scene");
                    PlayMakerFSM _battleSceneControl = _battleScene.LocateMyFSM("Control");

                    _battleSceneControl.ChangeFsmTransition("Start Pause", "FINISHED", "Hive Knight");

                    GameObject _hivequeen = _battleScene.Find("Vespa NPC");
                    PlayMakerFSM _hivequeen_dialogue_fsm = _hivequeen.LocateMyFSM("Conversation Control");
                    const string CONVO_FINISH = "CONVO_FINISH";

                    _hivequeen_dialogue_fsm.CopyState("Talk Extra", "Talk 2");
                    _hivequeen_dialogue_fsm.CopyState("Talk Extra", "Talk 3");
                    _hivequeen_dialogue_fsm.CopyState("Talk Extra", "Talk 4");
                    _hivequeen_dialogue_fsm.CopyState("Talk Extra", "Talk 5");

                    _hivequeen_dialogue_fsm.ChangeTransition("Talk 2", CONVO_FINISH, "Talk 3");
                    _hivequeen_dialogue_fsm.GetAction<CallMethodProper>("Talk 2", 0).parameters[0].SetValue("DESPACITO_2");

                    _hivequeen_dialogue_fsm.ChangeTransition("Talk 3", CONVO_FINISH, "Talk 4");
                    _hivequeen_dialogue_fsm.GetAction<CallMethodProper>("Talk 3", 0).parameters[0].SetValue("DESPACITO_3");

                    _hivequeen_dialogue_fsm.ChangeTransition("Talk 4", CONVO_FINISH, "Talk 5");
                    _hivequeen_dialogue_fsm.GetAction<CallMethodProper>("Talk 4", 0).parameters[0].SetValue("DESPACITO_4");

                    _hivequeen_dialogue_fsm.ChangeTransition("Talk 5", CONVO_FINISH, "Talk Finish");
                    _hivequeen_dialogue_fsm.GetAction<CallMethodProper>("Talk 5", 0).parameters[0].SetValue("DESPACITO_5");

                    _hivequeen_dialogue_fsm.ChangeTransition("Talk", CONVO_FINISH, "Talk 2");

                    break;
                case "Hive_04":

                    foreach (var (go, pos, rot, scale) in spike_related_spawns)
                    {
                        GameObject spawned = GameObject.Instantiate(go, pos, rot);
                        spawned.transform.localScale = scale;
                        spawned.SetActive(true);
                    }

                    GameObject static_pod = GameObject.Find("Zombie Hive (6)");
                    var pod_component = static_pod.GetOrAddComponent<WindexPod>();
                    pod_component.MakeStationary();
                    static_pod.transform.position = new Vector3(206f, 96f);
                    static_pod.transform.SetScaleY(-1);
                    static_pod.Find("Bee Hatchling Ambient(Clone)(Clone)").transform.localScale = new Vector3(1, -1, 1.5f);

                    break;
            }
        }
    }
}
