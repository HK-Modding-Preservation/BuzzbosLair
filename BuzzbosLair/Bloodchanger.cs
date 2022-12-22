using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuzzbosLair
{

    internal class Bloodchanger : MonoBehaviour
    {

        private static Color color_hiveblood = new Color(0.957f, 0.608f, 0.212f);
        private static Color color_lifeblood = new Color(0f, 0.584f, 1f);

        private Color bloodcolor;

        private void InfectedEnemyEffects_RecieveHitEffect(On.InfectedEnemyEffects.orig_RecieveHitEffect orig, InfectedEnemyEffects self, float attackDirection)
        {
            GameObject enemy = self.gameObject;
            if (enemy == this.gameObject)
            {
                if (ReflectionHelper.GetField<InfectedEnemyEffects, bool>(self, "didFireThisFrame"))
                {
                    return;
                }


            }
        }

        public void SetBloodColor (Color newbloodcolor)
        {
            bloodcolor = newbloodcolor;
        }

        public void Awake()
        {
            On.InfectedEnemyEffects.RecieveHitEffect += InfectedEnemyEffects_RecieveHitEffect;
        }

    }
}
