using Microsoft.Xna.Framework;
using SafeAndFree.Data;

namespace SafeAndFree
{
    class Tile
    {
        public Vector2 Position { get; set; }

        public MEDIA_ID TextureID;

        public Tile()
        {

        }

        public Tile(Vector2 position, MEDIA_ID textureID)
        {
            this.Position = position;
            this.TextureID = textureID;
        }
    }
}
