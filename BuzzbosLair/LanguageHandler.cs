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
                    return "Bullet Beel";
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
                    return key; //"¡Ay!<page>Fonsi, DY<page>Oh, oh no, oh no (oh)<page>Hey yeah<page>Diridiri, dirididi Daddy<page>Go!";
                case "HIVEQUEEN_TALK":
                    return key; //"Sí, sabes que ya llevo un rato mirándote<page>Tengo que bailar contigo hoy (DY)<page>Vi que tu mirada ya estaba llamándome<page>Muéstrame el camino que yo voy<page>Oh, tú, tú eres el imán y yo soy el metal<page>Me voy acercando y voy armando el plan<page>Solo con pensarlo se acelera el pulso (oh yeah)<page>Ya, ya me estás gustando más de lo normal<page>Todos mis sentidos van pidiendo más<page>Esto hay que tomarlo sin ningún apuro<page>Despacito<page>Quiero respirar tu cuello despacito<page>Deja que te diga cosas al oído<page>Para que te acuerdes si no estás conmigo<page>Despacito<page>Quiero desnudarte a besos despacito<page>Firmar las paredes de tu laberinto<page>Y hacer de tu cuerpo todo un manuscrito (sube, sube, sube)<page>(Sube, sube) Oh<page>Quiero ver bailar tu pelo<page>Quiero ser tu ritmo (uh oh, uh oh)<page>Que le enseñes a mi boca (uh oh, uh oh)<page>Tus lugares favoritos (favoritos, favoritos baby)<page>Déjame sobrepasar tus zonas de peligro (uh oh, uh oh)<page>Hasta provocar tus gritos (uh oh, uh oh)<page>Y que olvides tu apellido (dirididi Daddy)<page>Yo sé que estás pensándolo (eh)<page>Llevo tiempo intentándolo (eh)<page>Mami, esto es dando y dándolo<page>Sabes que tu corazón conmigo te hace bam bam<page>Sabe que esa beba 'tá buscando de mi bam bam<page>Ven prueba de mi boca para ver cómo te sabe<page>Quiero, quiero, quiero ver cuánto amor a ti te cabe<page>Yo no tengo prisa, yo me quiero dar el viaje<page>Empezamo' lento, después salvaje<page>Pasito a pasito, suave suavecito<page>Nos vamos pegando poquito a poquito<page>Cuando tú me besas con esa destreza<page>Veo que eres malicia con delicadeza<page>Pasito a pasito, suave suavecito<page>Nos vamos pegando, poquito a poquito (oh oh)<page>Y es que esa belleza es un rompecabezas (oh no)<page>Pero pa' montarlo aquí tengo la pieza (slow, oh yeah)<page>Despacito (yeh, yo)<page>Quiero respirar tu cuello despacito (yo)<page>Deja que te diga cosas al oído (yo)<page>Para que te acuerdes si no estás conmigo<page>Despacito<page>Quiero desnudarte a besos despacito (yeh)<page>Firmar las paredes de tu laberinto<page>Y hacer de tu cuerpo todo un manuscrito (sube, sube, sube)<page>(Sube, sube) Oh<page>Quiero ver bailar tu pelo<page>Quiero ser tu ritmo (uh oh, uh oh)<page>Que le enseñes a mi boca (uh oh, uh oh)<page>Tus lugares favoritos (favoritos, favoritos baby)<page>Déjame sobrepasar tus zonas de peligro (uh oh, uh oh)<page>Hasta provocar tus gritos (uh oh, uh oh)<page>Y que olvides tu apellido<page>Despacito<page>Vamo' a hacerlo en una playa en Puerto Rico<page>Hasta que las olas griten \"Ay, bendito\"<page>Para que mi sello se quede contigo (báilalo)<page>Pasito a pasito, suave suavecito (hey yeah, yeah)<page>Nos vamos pegando, poquito a poquito (oh no)<page>Que le enseñes a mi boca (uh oh, uh oh)<page>Tus lugares favoritos (favoritos, favoritos baby)<page>Pasito a pasito, suave suavecito<page>Nos vamos pegando, poquito a poquito<page>Hasta provocar tus gritos (eh-oh) (Fonsi)<page>Y que olvides tu apellido (DY)<page>Despacito";
                case "HIVEQUEEN_REPEAT":
                    return key;
                #endregion
            }

            return orig;//key;//
        }
    }
}
