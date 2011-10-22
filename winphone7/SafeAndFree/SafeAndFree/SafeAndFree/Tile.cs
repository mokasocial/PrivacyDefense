using Microsoft.Xna.Framework;
using SafeAndFree.Data;

namespace SafeAndFree
{
    class Tile : Actor
    {
        // This won't be needed when blitting the bitmap.
        public MEDIA_ID TextureID;

        public Tile()
        {

        }

        public Tile(Vector2 position, MEDIA_ID textureID)
        {
            this._position = position;
            this.TextureID = textureID;
        }
    }
}
