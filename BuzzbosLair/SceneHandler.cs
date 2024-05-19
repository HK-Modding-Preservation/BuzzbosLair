
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuzzbosLair
{
    internal class SceneHandler
    {
        private static GameObject spike_hitbox = BuzzbosLair._gameObjects["Hive Floor Spike Hitbox"];
        private static GameObject spike_sprite = BuzzbosLair._gameObjects["Hive Floor Spike Sprite"];

        private static ValueTuple<GameObject, Vector3, Quaternion, Vector3>[] spike_positions =
        {
            (spike_hitbox, new Vector3(150.7f, 89.7f), spike_hitbox.transform.rotation, new Vector3(-1f, -0.8f, 1f)),
            (spike_hitbox, new Vector3(171.7f, 89.7f), spike_hitbox.transform.rotation, new Vector3(-0.5f, -0.75f, 1f)),
            (spike_hitbox, new Vector3(192.7f, 89.7f), spike_hitbox.transform.rotation, new Vector3(-0.5f, 1f, 1f)),

            (spike_sprite, new Vector3(158.8f, 91f, -0.2f), Quaternion.Euler(new Vector3(0,0, 10f)), spike_sprite.transform.localScale),
            (spike_sprite, new Vector3(150.7f, 89.7f), spike_sprite.transform.rotation, spike_sprite.transform.localScale),
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
                    foreach (var (go, pos, rot, scale) in spike_positions)
                    {
                        GameObject spawned = GameObject.Instantiate(go, pos, rot);
                        spawned.transform.localScale = scale;
                        spawned.SetActive(true);
                    }

                    GameObject respawn_trigger = GameObject.Instantiate(BuzzbosLair._gameObjects["Hazard Respawn Trigger"]);
                    respawn_trigger.SetActive(true);

                    break;
            }
        }
    }
}
