using Modding.Utils;
using UnityEngine;

namespace BuzzbosLair
{
    internal class EnemyHandler
    {

        internal static void InitEnemies()
        {
            BuzzbosLair._gameObjects["Husk Hive"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = BuzzbosLair.GetSprite(TextureStrings.HuskHive_Key).texture;
        }

        internal static bool EnemyEnabled(GameObject enemy, bool isAlreadyDead)
        {

            if (enemy.name.Contains("Big Bee"))
            {
                enemy.GetOrAddComponent<Kamikabee>();
            }

            if (enemy.name.Contains("Bee Stinger"))
            {
                enemy.GetOrAddComponent<Railgun>();
            }

            if (enemy.name.Contains("Bee Hatchling Ambient") || enemy.name.Contains("Hiveling Spawner"))
            {
                enemy.GetOrAddComponent<SmallBee>();
            }

            if (enemy.name == "Hive Knight")
            {
                enemy.GetOrAddComponent<Buzzbo>();
            }

            if (enemy.name.Contains("Zombie Hive"))
            {
                enemy.GetOrAddComponent<WindexPod>();
            }

            return isAlreadyDead;
        }

    }
}
