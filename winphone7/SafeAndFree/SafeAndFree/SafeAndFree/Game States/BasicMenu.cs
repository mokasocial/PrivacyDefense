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

        Vector2 tapToGoPosition = new Vector2(294, 367);

        int textFade = 255;
        int fadeSpeed = 3;
        bool isFading = false;

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
            spriteBatch.Draw(TextureLibrary.GetTexture(textureId), new Rectangle(0, 0, 800, 480), Color.White);

            if (isFading)
            {
                if (textFade - fadeSpeed <= 0)
                {
                    textFade = 0;
                    isFading = false;
                }
                else
                {
                    textFade -= fadeSpeed;
                }
            }
            else
            {
                if (textFade + fadeSpeed >= 255)
                {
                    textFade = 255;
                    isFading = true;
                }
                else
                {
                    textFade += fadeSpeed;
                }
            }

            spriteBatch.DrawString(TextureLibrary.GetFont(FONT_ID.TAPTOGO), "Tap to continue.", tapToGoPosition, new Color(0, 0, 0, (byte)textFade));
        }
    }
}
