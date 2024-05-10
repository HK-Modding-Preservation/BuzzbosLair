using HutongGames.PlayMaker.Actions;
using Modding;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UObject = UnityEngine.Object;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace BuzzbosLair
{
    public class BuzzbosLair : Mod
    {
        internal static BuzzbosLair Instance;

        public static readonly Dictionary<string, GameObject> _gameObjects = new();

        private readonly Dictionary<string, ValueTuple<string, string>> _preloads = new()
        {
            /*["Soul Warrior"] = ("GG_Mage_Knight", "Mage Knight"),
            ["Mantis Lord"] = ("Fungus2_15_boss", "Mantis Battle/Battle Main/Mantis Lord"),
            ["Gruz Mother"] = ("GG_Gruz_Mother", "_Enemies/Giant Fly"),*/
            ["Honey Spike"] = ("Hive_05", "Battle Scene/Globs/Hive Knight Glob/Stingers/Stinger"),
            ["Spiny Husk"] = ("Fungus3_34", "Garden Zombie"),
            ["Husk Hive"] = ("Hive_01", "Zombie Hive"),
            ["Ambient Bee"] = ("Hive_01", "Bee Hatchling Ambient"),
            ["Grey Prince Zote"] = ("GG_Grey_Prince_Zote", "Grey Prince"),
        };

        public TextureStrings SpriteDict { get; private set; }

        public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

        public override string GetVersion() => "0.5.0-1";

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return _preloads.Values.ToList();
        }

        public BuzzbosLair() : base("Hallow Knight: Buzzbo's Lair")
        {
            //
            // Buzzbo's Lair as a sub-area?
            // Hive -> HIVE?
            // HIVE Research Center:
            //   * HIVE Institution Vitality Enhancements Research Center
            //   * HoneyIchor Vitality Enhancements Research Center
            // alt. simply HIVE, HIVE Institution for Vitality Enhancements
            // - Decide later if we want the recursive acronym or not
            //   (We do)
            //

            Instance = this;

            SpriteDict = new TextureStrings();
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            foreach (var (name, (scene, path)) in _preloads)
            {
                _gameObjects[name] = preloadedObjects[scene][path];
            }

            EnemyHandler.InitEnemies();
            ModHooks.OnEnableEnemyHook += EnemyHandler.EnemyEnabled;
            ModHooks.LanguageGetHook += LanguageHandler.LanguageGet;
            USceneManager.activeSceneChanged += SceneChanged;
            On.JournalList.BuildEnemyList += JournalList_BuildEnemyList;

            Log("Initialized");
        }

        private void JournalList_BuildEnemyList(On.JournalList.orig_BuildEnemyList orig, JournalList self)
        {
            Log("On.JournalList.BuildEnemyList hook called");
            for (int i = 0; i < self.list.Length; i++)
            {
                //Log(self.list[i].name);
                string name = self.list[i].GetComponent<JournalEntryStats>().convoName;
                
                switch (name)
                {
                    case "BEE_HATCHLING":
                        self.list[i].GetComponent<JournalEntryStats>().sprite = GetSprite(TextureStrings.BuzzboNormal_Key);
                        break;
                }
                    
            }
            orig(self);
        }

        private void SceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            switch(arg1.name)
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
            }
        }
    }
}