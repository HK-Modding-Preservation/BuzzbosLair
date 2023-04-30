using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuzzbosLair
{
    public class TextureStrings
    {

        public const string BuzzboNormal_Key = "BuzzboNormal";
        private const string BuzzboNormal_File = "BuzzbosLair.Resources.BuzzboNormal.png";
        public const string BuzzboAwakened_Key = "BuzzboAwakened";
        private const string BuzzboAwakened_File = "BuzzbosLair.Resources.BuzzboAwakened.png";

        private readonly Dictionary<string, Sprite> _dict;

        public TextureStrings()
        {

            Modding.Logger.Log("Start TextureStrings constructor");

            Assembly asm = Assembly.GetExecutingAssembly();
            _dict = new Dictionary<string, Sprite>();
            var tmpTextures = new Dictionary<string, string>();
            tmpTextures.Add(BuzzboNormal_Key, BuzzboNormal_File);
            tmpTextures.Add(BuzzboAwakened_Key, BuzzboAwakened_File);

            foreach (var pair in tmpTextures)
            {
                using (Stream s = asm.GetManifestResourceStream(pair.Value))
                {
                    if (s != null)
                    {

                        byte[] buffer = new byte[s.Length];
                        s.Read(buffer, 0, buffer.Length);
                        s.Dispose();

                        //Create texture from bytes
                        var tex = new Texture2D(2, 2);

                        tex.LoadImage(buffer, true);

                        // Create sprite from texture
                        _dict.Add(pair.Key, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
                    }
                }
            }
        }
        
        public Sprite Get(string key)
        {
            return _dict[key];
        }

    }
}
