using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzbosLair
{
    internal class LanguageHandler
    {

        internal static string LanguageGet(string key, string sheetTitle, string orig)
        {

            switch (key)
            {
                case "HIVE_SUPER":
                    return key;
                case "HIVE_MAIN":
                    return key;
                case "HIVE_SUB":
                    return key;
                #region Boss
                case "HIVE_KNIGHT_SUPER":
                    return "Dual Blood";
                case "HIVE_KNIGHT_MAIN":
                    return "Buzzbo";
                case "HIVE_KNIGHT_SUB":
                    return "";
                case "GG_S_HIVEKNIGHT":
                    return "Unyielding god of self-enhancement";
                case "NAME_HIVE_KNIGHT":
                    return "Buzzbo";
                case "DESC_HIVE_KNIGHT":
                    return key;
                case "NOTE_HIVE_KNIGHT":
                    return key;
                #endregion
                #region In-combat Dream Nail messages
                #endregion
                #region Journal entries
                case "NAME_BEE_HATCHLING":
                    return key;
                case "DESC_BEE_HATCHLING":
                    return key;
                case "NOTE_BEE_HATCHLING":
                    return key;

                case "NAME_BEE":
                    return "Orbeetal Railgun";
                case "DESC_BEE":
                    return key;
                case "NOTE_BEE":
                    return key;

                case "NAME_BIG_BEE":
                    return "Kamikabee";
                case "DESC_BIG_BEE":
                    return key;
                case "NOTE_BIG_BEE":
                    return key;

                case "NAME_ZOM_HIVE":
                    return key;
                case "DESC_ZOM_HIVE":
                    return key;
                case "NOTE_ZOM_HIVE":
                    return key;

                #endregion
                #region Queen
                case "GH_HIVEQUEEN_NC_SUPER":
                case "GH_HIVEQUEEN_C_SUPER":
                    return key;
                case "GH_HIVEQUEEN_NC_MAIN":
                case "GH_HIVEQUEEN_C_MAIN":
                    return key;
                case "GH_HIVEQUEEN_NC_SUB":
                case "GH_HIVEQUEEN_C_SUB":
                    return key;
                case "HIVEQUEEN_EXTRA":
                    return key;//"¡Ay!<page>Fonsi, DY<page>Oh, oh no, oh no (oh)<page>Hey yeah<page>Diridiri, dirididi Daddy<page>Go!";
                case "HIVEQUEEN_TALK":
                    return key;
                case "HIVEQUEEN_REPEAT":
                    return key;
                #endregion
            }

            return key;//orig;
        }
    }
}
