using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SafeAndFree.Data;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using SafeAndFree.InputHandlers;

namespace SafeAndFree.Game_States
{
    class BasicMenu : Screen
    {
        MEDIA_ID textureId;

        Screens next;

        public BasicMenu(MEDIA_ID textureId, Screens next)
        {
            this.textureId = textureId;
            this.next = next;
        }

        public override void Update()
        {
            if (TouchHandler.IsClicked)
            {
                GameEngine.RunningEngine.Load(next);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureLibrary.GetTexture(textureId), new Vector2(0, 0), Color.White);
        }
    }
}
