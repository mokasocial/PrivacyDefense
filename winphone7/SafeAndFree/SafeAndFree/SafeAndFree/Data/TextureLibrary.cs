using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SafeAndFree.Exceptions;

namespace SafeAndFree.Data
{
    /// <summary>
    /// 
    /// </summary>
   public enum MEDIA_ID
   {
        CREEP_0,
        CREEP_1,
        CREEP_2,
        CREEP_3,
        MAP_0,
        TOWER_0,
        TOWER_1,
        TOWER_2,
        PROJECTILE_0,
        PROJECTILE_1,
        PROJECTILE_2,
        TILE_SELCT,
        MENU_LEFT,
        MENU_TOP,
        TITLESCREEN,
        WINSCREEN,
        LOSESCREEN
    }

   public enum BUTTON_MEDIA_ID
   {
       TEACHER,
       LAWYER,
       JUDGE,
       UPGRADE
   }

   public enum FONT_ID
   {
       HUDINFO,
       TAPTOGO
   }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// TODO: Think about how this type of functionality could be extended
    /// to having a library of other resources... such as sounds.
    /// </remarks>
    static class TextureLibrary
    {
        public static ContentManager Content = null;

        private static Dictionary<MEDIA_ID, Texture2D> textures = new Dictionary<MEDIA_ID, Texture2D>();

        private static string[] assetNames = new string[] { "creep_dataminer", "creep_heli", "creep_defcon", "creep_corporate", "SafeAndFreeMap", "Judges", "Lawyers", "Teachers", "GavelSS", "Scroll", "apple", "tileMarker", "menu", "tophud", "titlescreen", "winscreen", "losescreen" };

        private static Dictionary<FONT_ID, SpriteFont> fonts = new Dictionary<FONT_ID, SpriteFont>();
        private static string[] fontNames = new string[] { "DefaultFont", "TapToGo" };


        private static Dictionary<BUTTON_MEDIA_ID, Texture2D> btnTextures = new Dictionary<BUTTON_MEDIA_ID, Texture2D>();
        private static string[] buttonNames = new string[] { "ButtonTeacher","ButtonLawyer","ButtonJudge", "ButtonUpgrade"};

        public static SpriteFont GetFont(FONT_ID fontId)
        {
            if (null == Content)
            {
                throw new ContentNotDefinedException();
            }

            if (fonts.ContainsKey(fontId))
            {
                return fonts[fontId];
            }
            else
            {
                SpriteFont newFont = Content.Load<SpriteFont>(fontNames[(int)fontId]);
                fonts.Add(fontId, newFont);
                return newFont;
            }
        }

        public static Texture2D GetButtonTexture(BUTTON_MEDIA_ID mediaId)
        {
            if (null == Content)
            {
                throw new ContentNotDefinedException();
            }
            if (btnTextures.ContainsKey(mediaId))
            {
                return btnTextures[mediaId];
            }
            else
            {
                Texture2D newTexture = Content.Load<Texture2D>(buttonNames[(int)mediaId]);
                btnTextures.Add(mediaId, newTexture);
                return newTexture;
            }
        }

        public static Texture2D GetTexture(MEDIA_ID mediaId)
        {
            if(null == Content)
            {
                throw new ContentNotDefinedException();
            }

            if (textures.ContainsKey(mediaId))
            {
                return textures[mediaId];
            }

            Texture2D newTexture = Content.Load<Texture2D>(assetNames[(int)mediaId]);
            textures.Add(mediaId, newTexture);

            return newTexture;
        }
    }
}