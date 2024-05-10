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
                string name = self.list[i].GetComponent<JournalEntryStats>().convoName;

                switch (name)
                {
                    case "BEE_HATCHLING":
                        JournalEntryStats journal_entry = self.list[i].GetComponent<JournalEntryStats>();

                        Texture2D tex = BuzzbosLair.GetSprite(TextureStrings.HJ_SmallBee_Key).texture;
                        journal_entry.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), journal_entry.sprite.pixelsPerUnit / 1.4f);

                        break;
                    case "ZOM_HIVE":

                        self.list[i].GetComponent<JournalEntryStats>().sprite = Sprite.Create(BuzzbosLair.GetSprite(TextureStrings.HJ_HuskHive_Key).texture, new Rect(0, 0, BuzzbosLair.GetSprite(TextureStrings.HJ_SmallBee_Key).texture.width, BuzzbosLair.GetSprite(TextureStrings.HJ_SmallBee_Key).texture.height), new Vector2(0.5f, 0.5f), self.list[i].GetComponent<JournalEntryStats>().sprite.pixelsPerUnit / 1.4f);
                        break;
                }
            }
            orig(self);
        }
    }
}
