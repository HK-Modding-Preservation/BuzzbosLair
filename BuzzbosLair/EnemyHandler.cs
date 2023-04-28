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

            return isAlreadyDead;
        }

    }
}
