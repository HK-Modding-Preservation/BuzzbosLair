using SFCore.Utils;
using UnityEngine;

namespace BuzzbosLair
{
    internal class JournalHandler
    {
        internal static void JournalList_BuildEnemyList(On.JournalList.orig_BuildEnemyList orig, JournalList self)
        {
            for (int i = 0; i < self.list.Length; i++)
            {
                JournalEntryStats journal_entry = self.list[i].GetComponent<JournalEntryStats>();
                string name = journal_entry.convoName;

                switch (name)
                {
                    case "BEE_HATCHLING":
                        ReplaceMainSprite(journal_entry, BuzzbosLair.GetSprite(TextureStrings.HJ_SmallBee_Key));
                        ReplaceListSprite(journal_entry, BuzzbosLair.GetSprite(TextureStrings.HJ_SmallBee_Key));
                        break;
                    case "ZOM_HIVE":
                        ReplaceMainSprite(journal_entry, BuzzbosLair.GetSprite(TextureStrings.HJ_HuskHive_Key));
                        ReplaceListSprite(journal_entry, BuzzbosLair.GetSprite(TextureStrings.HJ_HuskHive_Icon_Key));
                        break;
                    case "HIVE_KNIGHT":
                        ReplaceMainSprite(journal_entry, BuzzbosLair.GetSprite(TextureStrings.HJ_Buzzbo_Key));
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

        private static void ReplaceListSprite(JournalEntryStats journal_entry, Sprite sprite)
        {
            Texture2D tex = sprite.texture;
            journal_entry.gameObject.Find("Portrait").GetComponent<SpriteRenderer>().sprite = 
                Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), journal_entry.gameObject.Find("Portrait").GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
    }
}
