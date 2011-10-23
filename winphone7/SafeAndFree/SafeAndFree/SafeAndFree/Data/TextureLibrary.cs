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
        PROJECTILE_0,
        TILE_SELCT,
        PROJECTILE_1,
        MENU_LEFT,
        TITLESCREEN,
        WINSCREEN,
        LOSESCREEN
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

        private static string[] assetNames = new string[] { "creep_dataminer", "creep_heli", "creep_defcon", "creep_corporate", "SafeAndFreeMap", "Judge", "TestProjectile", "tileSelect", "TestProjectile2", "menu", "titlescreen", "losescreen", "winscreen" };

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
            else
            {
                Texture2D newTexture = Content.Load<Texture2D>(assetNames[(int)mediaId]);
                textures.Add(mediaId, newTexture);

                return newTexture;
            }
        }
    }
}