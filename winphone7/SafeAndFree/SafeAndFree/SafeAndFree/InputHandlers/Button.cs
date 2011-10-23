using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SafeAndFree.Enumerations;
using SafeAndFree.Data;

namespace SafeAndFree.InputHandlers
{
    public class Button : Actor
    {
        public bool Visible{get; private set;}
        public BUTTON_MEDIA_ID id;
        public Button(bool vis)
        {
            Visible = vis;
            id = BUTTON_MEDIA_ID.UPGRADE;
        }
        public Button(bool vis, TowerTypes towerType)
        {
            Visible = vis;
            switch (towerType)
            {
                case TowerTypes.Normal:
                    id = BUTTON_MEDIA_ID.TEACHER;
                    break;
                case TowerTypes.Fast:
                    id = BUTTON_MEDIA_ID.LAWYER;
                    break;
                case TowerTypes.Slow:
                    id = BUTTON_MEDIA_ID.JUDGE;
                    break;
                default:
                    id = BUTTON_MEDIA_ID.TEACHER;
                    break;
            }
        }
        public void ToggleVisibility()
        {
           Visible =!Visible;
        }
        
        
    }
}
