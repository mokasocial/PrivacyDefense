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
    enum MEDIA_ID
    {
        TILE_0,
        TILE_1,
        TILE_2,
        TILE_3,
        MAP_0,
        CREEP_0
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

        private static string[] assetNames = new string[]{"tile0", "tile1", "tile2", "tile3", "winphonemap", "creep0"};

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