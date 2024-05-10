using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuzzbosLair
{
    internal class JournalHandler
    {
        internal static void JournalList_BuildEnemyList(On.JournalList.orig_BuildEnemyList orig, JournalList self)
        {
            BuzzbosLair.Instance.Log("On.JournalList.BuildEnemyList hook called");
            for (int i = 0; i < self.list.Length; i++)
            {
                //Log(self.list[i].name);
                JournalEntryStats journal_entry = self.list[i].GetComponent<JournalEntryStats>();
                string name = journal_entry.convoName;

                switch (name)
                {
                    case "BEE_HATCHLING":
                        Texture2D tex = BuzzbosLair.GetSprite(TextureStrings.HJ_SmallBee_Key).texture;
                        journal_entry.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), journal_entry.sprite.pixelsPerUnit);

                        break;
                    case "ZOM_HIVE":
                        ReplaceMainSprite(journal_entry, BuzzbosLair.GetSprite(TextureStrings.HJ_HuskHive_Key));
                        break;
                }
            }
            orig(self);
        }

        private static void ReplaceMainSprite(JournalEntryStats journal_entry, Sprite sprite)
        {
            Texture2D tex = sprite.texture;
            journal_entry.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), journal_entry.sprite.pixelsPerUnit);
        }
    }
}
