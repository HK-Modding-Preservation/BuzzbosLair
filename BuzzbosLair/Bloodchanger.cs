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

        public static Color color_hiveblood = new Color(0.957f, 0.608f, 0.212f);
        public static Color color_lifeblood = new Color(0f, 0.584f, 1f);

        private Color bloodcolor = Color.white;

        private void InfectedEnemyEffects_RecieveHitEffect(On.InfectedEnemyEffects.orig_RecieveHitEffect orig, InfectedEnemyEffects self, float attackDirection)
        {
            GameObject enemy = self.gameObject;
            if (enemy == this.gameObject)
            {
                if (ReflectionHelper.GetField<InfectedEnemyEffects, bool>(self, "didFireThisFrame"))
                {
                    return;
                }

                SpriteFlash spriteFlash = ReflectionHelper.GetField<InfectedEnemyEffects, SpriteFlash>(self, "spriteFlash");
                AudioEvent impactAudio = ReflectionHelper.GetField<InfectedEnemyEffects, AudioEvent>(self, "impactAudio");
                AudioSource audioSourcePrefab = ReflectionHelper.GetField<InfectedEnemyEffects, AudioSource>(self, "audioSourcePrefab");
                Vector3 effectOrigin = ReflectionHelper.GetField<InfectedEnemyEffects, Vector3>(self, "effectOrigin");

                if (spriteFlash != null)
                {
                    spriteFlash.flash(bloodcolor, 0.9f, 0.01f, 0.01f, 0.25f);
                }

                impactAudio.SpawnAndPlayOneShot(audioSourcePrefab, base.transform.position);

                switch (DirectionUtils.GetCardinalDirection(attackDirection))
                {
                    case 0:
                        GlobalPrefabDefaults.Instance.SpawnBlood(base.transform.position + effectOrigin, 3, 4, 10f, 15f, 120f, 150f, bloodcolor);
                        GlobalPrefabDefaults.Instance.SpawnBlood(base.transform.position + effectOrigin, 8, 15, 10f, 25f, 30f, 60f, bloodcolor);
                        break;
                    case 1:
                        GlobalPrefabDefaults.Instance.SpawnBlood(base.transform.position + effectOrigin, 8, 10, 20f, 30f, 80f, 100f, bloodcolor);
                        break;
                    case 2:
                        GlobalPrefabDefaults.Instance.SpawnBlood(base.transform.position + effectOrigin, 3, 4, 10f, 15f, 30f, 60f, bloodcolor);
                        GlobalPrefabDefaults.Instance.SpawnBlood(base.transform.position + effectOrigin, 8, 10, 15f, 25f, 120f, 150f, bloodcolor);
                        break;
                    case 3:
                        GlobalPrefabDefaults.Instance.SpawnBlood(base.transform.position + effectOrigin, 4, 5, 15f, 25f, 140f, 180f, bloodcolor);
                        GlobalPrefabDefaults.Instance.SpawnBlood(base.transform.position + effectOrigin, 4, 5, 15f, 25f, 360f, 400f, bloodcolor);
                        break;
                }

                ReflectionHelper.SetField<InfectedEnemyEffects, bool>(self, "didFireThisFrame", true);

                return;
            }

            orig(self, attackDirection);
        }

        private void EnemyDeathEffects_EmitEffects(On.EnemyDeathEffects.orig_EmitEffects orig, EnemyDeathEffects self)
        {
            GameObject enemy = self.gameObject;
            if (enemy == this.gameObject)
            {
                return;
            }
            orig(self);
        }

        public void SetBloodColor (Color newbloodcolor)
        {
            bloodcolor = newbloodcolor;
        }

        public void Awake()
        {
            On.InfectedEnemyEffects.RecieveHitEffect += InfectedEnemyEffects_RecieveHitEffect;
            On.EnemyDeathEffects.EmitEffects += EnemyDeathEffects_EmitEffects;
        }

        public void OnDestroy()
        {
            
            On.InfectedEnemyEffects.RecieveHitEffect -= InfectedEnemyEffects_RecieveHitEffect;
            On.EnemyDeathEffects.EmitEffects -= EnemyDeathEffects_EmitEffects;
        }

    }
}
