using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BuzzbosLair
{
    public class BuzzbosLair : Mod
    {
        internal static BuzzbosLair Instance;

        public static readonly Dictionary<string, GameObject> _gameObjects = new();

        private Dictionary<string, ValueTuple<string, string>> _preloads = new()
        {
            /*["Soul Warrior"] = ("GG_Mage_Knight", "Mage Knight"),
            ["Mantis Lord"] = ("Fungus2_15_boss", "Mantis Battle/Battle Main/Mantis Lord"),
            ["Gruz Mother"] = ("GG_Gruz_Mother", "_Enemies/Giant Fly"),*/
            ["Honey Spike"] = ("Hive_05", "Battle Scene/Globs/Hive Knight Glob/Stingers/Stinger"),
            ["Spiny Husk"] = ("Fungus3_34", "Garden Zombie"),
        };

        public TextureStrings SpriteDict { get; private set; }

        public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

        public override string GetVersion() => "0.3.2.0 : Awakened Contact Damage";

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

            ModHooks.OnEnableEnemyHook += EnemyHandler.EnemyEnabled;
            ModHooks.LanguageGetHook += LanguageHandler.LanguageGet;

            Log("Initialized");
        }

    }
}