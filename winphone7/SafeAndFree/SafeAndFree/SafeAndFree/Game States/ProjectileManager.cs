using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SafeAndFree.Game_States
{
    public class ProjectileManager
    {
        public List<Projectile> Projectiles { get; private set; }
        public ProjectileManager() 
        {
            Projectiles = new List<Projectile>();
        }
        public void AddProjectile(Projectile newProjectile)
        {
            Projectiles.Add(newProjectile);
        }
        public void Update()
        {
            List<Projectile> removeList = new List<Projectile>();
            foreach (Projectile p in Projectiles)
            {
                if (p.Tick())
                {
                    removeList.Add(p);
                }
            }
            foreach (Projectile r in removeList)
            {
                Projectiles.Remove(r);
            }
        }
        public List<Vector2> GetProjectilePoints()
        {
            var retList = new List<Vector2>();
            Projectiles.ForEach(p => { retList.Add(p.Position); });
            return retList;
        }
    }
}
