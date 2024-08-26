
namespace BuzzbosLair
{
    internal class LanguageHandler
    {

        internal static string LanguageGet(string key, string sheetTitle, string orig)
        {

            switch (key)
            {
                case "HIVE_SUPER":
                    return "";
                case "HIVE_MAIN":
                    return "The H.I.V.E";
                case "HIVE_SUB":
                    return "";
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
                    return "Interim leader of the H.I.V.E. Experiments on himself more than on his test subjects.";
                case "NOTE_HIVE_KNIGHT":
                    return "To modify one's body to such a degree...it is an unsettling thought, but this creature's determination to 'reach eternity' is praiseworthy.";
                #endregion
                #region In-combat Dream Nail messages
                case "HIVE_KNIGHT_1":
                    return "Are you watching, Fonsi?";
                case "HIVE_KNIGHT_2":
                    return "Have faith in me, Luis!";
                case "HIVE_KNIGHT_3":
                    return "oof";
                case "BUZZBO_GG_1":
                    return "You slaughtered my people...and me.";
                case "BUZZBO_GG_2":
                    return "We were meant to be eternal! We perfected life! How did we die?";
                case "BUZZBO_GG_3":
                    return "I'll end you like you ended our H.I.V.E!";
                #endregion
                #region Journal entries
                case "NAME_BEE_HATCHLING":
                    return "Bullet Beel";
                case "DESC_BEE_HATCHLING":
                    return "Recently born, this creature has ungodly amounts of Windex coursing through its veins.";
                case "NOTE_BEE_HATCHLING":
                    return "As they grow older, their natural Honey Ichor dilutes the Windex in their bodies; as newborn, however, they are blue bullets of fury.";

                case "NAME_BEE":
                    return "Orbeetal Railgun";
                case "DESC_BEE":
                    return "A creature that regenerates its spines even as it fires them out from its body.";
                case "NOTE_BEE":
                    return "Honey Ichor...such a marvelous substance.";

                case "NAME_BIG_BEE":
                    return "Kamikabee";
                case "DESC_BIG_BEE":
                    return "A titanic mass of destructive force, this creature cares not for the wreckage it leaves behind in its own facility.";
                case "NOTE_BIG_BEE":
                    return "Though their Honey Ichor gave them long-term regenerative abilities, the creatures of the H.I.V.E experimented with Windex, and enahnced their constitution tenfold. I wonder if it would have worked for me?";

                case "NAME_ZOM_HIVE":
                    return "Windex Pod";
                case "DESC_ZOM_HIVE":
                    return "An cloner-incubator, filled with Windex.";
                case "NOTE_ZOM_HIVE":
                    return "The creatures of the H.I.V.E had a fierce obsession with 'perfecting life', and 'achieving eternity'. It is fascinating how far it took their sciences.";

                #endregion
                #region Queen
                case "GH_HIVEQUEEN_NC_SUPER":
                case "GH_HIVEQUEEN_C_SUPER":
                    return "Luis";
                case "GH_HIVEQUEEN_NC_MAIN":
                case "GH_HIVEQUEEN_C_MAIN":
                    return "\"Vespacito\"";
                case "GH_HIVEQUEEN_NC_SUB":
                case "GH_HIVEQUEEN_C_SUB":
                    return "Fonsi";
                case "HIVEQUEEN_EXTRA":
                    return "Ay, ¡Fonsi! ¡D.Y.!<page>Ooh, oh, no, oh, no, oh<page>¡Hey, yeah!<page>Dididiri Daddy, go!<page>Sí, sabes que ya llevo un rato mirándote<page>Tengo que bailar contigo hoy (¡D.Y.!)<page>Vi que tu mirada ya estaba llamándome<page>Muéstrame el camino que yo voy<page>¡Oh! Tú, tú eres el imán y yo soy el metal<page>Me voy acercando y voy armando el plan<page>Sólo con pensarlo se acelera el pulso (¡Oh, yeah!)<page>Ya, ya me está gustando más de lo normal<page>Todos mis sentidos van pidiendo más<page>Esto hay que tomarlo sin ningún apuro";
                case "HIVEQUEEN_TALK":
                    return "Despacito<page>Quiero respirar tu cuello despacito<page>Deja que te diga cosas al oído<page>Para que te acuerdes si no estás conmigo<page>Despacito<page>Quiero desnudarte a besos despacito<page>Firmar las paredes de tu laberinto<page>Y hacer de tu cuerpo todo un manuscrito<page>(Sube, sube, sube; sube, sube)<page>Quiero ver bailar tu pelo, quiero ser tu ritmo (Woh, eh)<page>Que le enseñes a mi boca (Woh, eh)<page>Tus lugares favoritos (Favorito', favorito', baby)<page>Déjame sobrepasar tus zonas de peligro (Eh; woh, eh)<page>Hasta provocar tus gritos (Woh, eh)<page>Y que olvides tu apellido (Rrr; D.Y.)";
                case "DESPACITO_2":
                    return "Si te pido un beso, ven, dámelo, yo sé que estás pensándolo (Eh)<page>Llevo tiempo intentándolo (Eh), mami, esto es dando y dándolo<page>Sabes que tu corazón conmigo te hace bam-bam<page>Sabes que esa beba está buscando de mi bam-bam<page>Ven, prueba de mi boca para ver cómo te sabe (Eh-eh; ¡plo!)<page>Quiero, quiero, quiero ver cuánto amor a ti te cabe<page>Yo no tengo prisa, yo me quiero dar el viaje<page>Empezamos lento, después salvaje";
                case "DESPACITO_3":
                    return "Pasito a pasito, suave suavecito<page>Nos vamo' pegando, poquito a poquito<page>Cuando tú me besas con esa destreza<page>Veo que eres malicia con delicadeza<page>Pasito a pasito, suave suavecito<page>Nos vamo' pegando, poquito a poquito (Oh-oh)<page>Y es que esa belleza es un rompecabezas (No, no)<page>Pero pa' montarlo aquí tengo la pieza<page>¡Oye! (Yo', yo'; ¡plo!)";
                case "DESPACITO_4":
                    return "Despacito<page>Quiero respirar tu cuello despacito (Yo')<page>Deja que te diga cosas al oído (Yo')<page>Para que te acuerdes si no estás conmigo (Plo, plo)<page>Despacito (¡Plo!)<page>Quiero desnudarte a besos despacito (Yeah-eh)<page>Firmar las paredes de tu laberinto<page>Y hacer de tu cuerpo todo un manuscrito<page>(Sube, sube, sube, sube, sube)<page>Quiero ver bailar tu pelo, quiero ser tu ritmo (Eh, woh, eh)<page>Que le enseñes a mi boca (Woh, eh)<page>Tus lugares favoritos (Favorito', favorito', baby)<page>Déjame sobrepasar tus zonas de peligro (Eh, woh, eh)<page>Hasta provocar tus gritos (Woh, eh)<page>Y que olvides tu apellido";
                case "DESPACITO_5":
                    return "Despacito<page>Vamo' a hacerlo en una playa en Puerto Rico<page>Hasta que las olas griten: \"¡Ay, Bendito!\"<page>Para que mi sello se quede contigo<page>¡Báilalo!<page>Pasito a pasito, suave suavecito (Oh, yeah-yeah)<page>Nos vamos pegando, poquito a poquito (No, no; oh)<page>Que le enseñes a mi boca<page>Tus lugares favoritos<page>Favorito', favorito', baby (Ooh)<page>Pasito a pasito, suave suavecito<page>Nos vamos pegando, poquito a poquito<page>Hasta provocar tus gritos (Fonsi)<page>Y que olvides tu apellido (D.Y.)<page>Despacito";
                case "HIVEQUEEN_REPEAT":
                    return "Pasito a pasito, suave suavecito<page>Nos vamos pegando, poquito a poquito<page>¡Ay, ay!<page>¡Ay, ay!";
                #endregion
            }

            return orig;//key;//
        }
    }
}
