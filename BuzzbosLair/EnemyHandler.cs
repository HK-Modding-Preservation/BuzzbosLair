using FriendCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                enemy.AddComponent<Kamikabee>();
            }

            if (enemy.name.Contains("Bee Stinger"))
            {
                enemy.AddComponent<Railgun>();
            }

            if (enemy.name.Contains("Bee Hatchling Ambient") || enemy.name.Contains("Hiveling Spawner"))
            {
                enemy.AddComponent<SmallBee>();
            }

            if (enemy.name == "Hive Knight")
            {
                enemy.AddComponent<Buzzbo>();
            }

            if (enemy.name.Contains("Zombie Hive"))
            {
                //enemy.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = BuzzbosLair.GetSprite(TextureStrings.HuskHive_Key).texture;
                GameObject little_bee = GameObject.Instantiate(BuzzbosLair._gameObjects["Ambient Bee"], enemy.transform);
                little_bee.LocateMyFSM("Bee").enabled = false;
                little_bee.LocateMyFSM("flyer_receive_direction_msg").enabled = false;
                GameObject.Destroy(little_bee.GetComponent<Rigidbody2D>());
                GameObject.Destroy(little_bee.GetComponent<HealthManager>());
                GameObject.Destroy(little_bee.GetComponent<DamageHero>());
                GameObject.Destroy(little_bee.GetComponent<SetZ>());
                GameObject.Destroy(little_bee.GetComponent<BoxCollider2D>());
                GameObject.Destroy(little_bee.GetComponent<Recoil>());
                GameObject.Destroy(little_bee.GetComponent<EnemyDreamnailReaction>());
                little_bee.transform.localPosition = new Vector3(0, 0, 0.01f);
                little_bee.SetActive(true);

            }

            return isAlreadyDead;
        }

    }
}
