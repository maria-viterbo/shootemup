using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp
{
    public class BulletManager
    {

        List<Bullet> bulletList = new List<Bullet>(); 
      
        public void AddBullet()
        {
            bulletList.Add(new Bullet());
        }

        public void RemoveBullet(int bulletIndex)
        {
            bulletList.RemoveAt(bulletIndex);
        }

        public void ClearBullets()
        {
            bulletList.Clear();
        }

        //returns the number of bullets in bulletList
        public int GetBulletCount()
        {
            return bulletList.Count;
        }

        public void UpdateBullets()
        {
            //for each bullet
            for (int i=0; i<bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];

                //get enemy index
                int enemyIndex = GameRoot.enemyManager.EnemyIndexAt(bullet.x, bullet.y);

                //update bullet position
                bullet.Update();

                //remove offscreen bullets
                if (bullet.y < 0)
                {
                    RemoveBullet(i);
                    i--;
                }
                //bullet enemy collision
                else if (enemyIndex != -1) //if bullet hits enemy (-1 means it did not hit enemy)
                {
                    GameRoot.enemyManager.RemoveEnemy(enemyIndex);
                    RemoveBullet(i);
                    i--;
                }

                if (GameRoot.currentLevel == 5 && BossBulletCollision(bullet.x, bullet.y))
                {
                    if (GameRoot.boss.health > 0)
                    {
                        RemoveBullet(i);
                        i--;

                        if (BossHitBoxBulletCollision(bullet.x, bullet.y))
                        {
                            GameRoot.boss.health--;
                        }
                    }

                }
                
            }
        }

        private bool BossBulletCollision(int bulletX, int bulletY)
        {
            Boss boss = GameRoot.boss;
            Rectangle bossRect = new Rectangle(boss.x, boss.y, boss.width, boss.height);

            if (bossRect.Contains(bulletX, bulletY))
            {
                return true;
            }
            return false;
        }

        private bool BossHitBoxBulletCollision(int bulletX, int bulletY)
        {
            Boss boss = GameRoot.boss;
            Rectangle bossHitBoxRect = new Rectangle(boss.hitBoxX, boss.hitBoxY, boss.hitBoxWidth, boss.hitBoxHeight);

            if (bossHitBoxRect.Contains(bulletX, bulletY))
            {
                return true;
            }
            return false;
        }

        public void DrawBullets(SpriteBatch spritebatch, Texture2D texture)
        {
            foreach (Bullet bullet in bulletList)
            {
                bullet.Draw(spritebatch, texture);
            }
        }
    }
}