using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SafeAndFree.Data;
using SafeAndFree.Helpers;

namespace SafeAndFree
{
    class Creep : Actor
    {
        private Vector2 CenterPosition;

        public MEDIA_ID TextureID;

        private int _speed = 2;
        private int _path = 0;
        private int _nextWaypoint = 0;

        public override Vector2 Position
        {
            get
            {
                return new Vector2(CenterPosition.X - Board.TileCenter.X, CenterPosition.Y - Board.TileCenter.Y);
            }
        }

        public int Path
        {
            get
            {
                return _path;
            }
        }

        public Creep()
        {
        }

        public Creep(Vector2 position, MEDIA_ID textureID)
        {
            this.CenterPosition = new Vector2(this.Position.X + Board.TileCenter.X, this.Position.Y + Board.TileCenter.Y);
            this.TextureID = textureID;
        }

        public Creep(Vector2 position, MEDIA_ID textureID, int path, int startingWaypoint) : this(position, textureID)
        {
            this._path = path;
            this._nextWaypoint = startingWaypoint;
        }

        public bool Update(Vector2[][] paths)
        {
            Vector2[] ourPath = paths[_path];

            if (Math.Abs(this.CenterPosition.X - ourPath[_nextWaypoint].X) > _speed)
            {
                if (ourPath[_nextWaypoint].X > this.CenterPosition.X)
                {
                    this.CenterPosition.X += _speed;
                }
                else if (ourPath[_nextWaypoint].X < this.CenterPosition.X)
                {
                    this.CenterPosition.X -= _speed;
                }
            }
            else if (Math.Abs(this.CenterPosition.Y - ourPath[_nextWaypoint].Y) > _speed)
            {
                if (ourPath[_nextWaypoint].Y > this.CenterPosition.Y)
                {
                    this.CenterPosition.Y += _speed;
                }
                else if (ourPath[_nextWaypoint].Y < this.CenterPosition.Y)
                {
                    this.CenterPosition.Y -= _speed;
                }
            }
            else
            {
                _nextWaypoint++;

                if (ourPath.Length == _nextWaypoint)
                {
                    // We have reached the end.
                    return true;
                }
            }

            return false;
        }
    }
}
