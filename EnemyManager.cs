using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp
{
    public class EnemyManager
    {
        List<Enemy> enemyList;
        int[] enemyCount;
        Random random;

        public EnemyManager()
        {
            enemyList = new List<Enemy>();
            enemyCount = new int[] { 0, 10, 20, 50, 80 };
            random = new Random();
        }

        public void SetUpEnemiesFor(int currentLevel)
        {
            enemyList.Clear();
            if (currentLevel <= 4) //if not boss level
            {
                for (int i = 0; i < enemyCount[currentLevel]; i++)
                {
                    AddEnemyAtRandomPos();
                }
            }
        }

        public void AddEnemyAtRandomPos()
        {
            //make new enemy
            Enemy enemy = new Enemy(0, 0);

            //Get random coords that aren't on current enemies
            int randomX;
            int randomY;

            do
            {
                randomX = random.Next(30, GameRoot.windowWidth - enemy.width - 30);
                randomY = random.Next(-1000, 0);

            } while (!NewEnemyCanBePlacedAt(randomX, randomY)); //do until a free spot is found for new enemy

            //when free spot is found, give new enemy the random coords
            enemy.x = randomX;
            enemy.y = randomY;

            //If level 2 or ahead
            if (GameRoot.currentLevel >= 3)
            {
                //20% chance 
                if (random.NextDouble() < 0.2)
                {
                    //Make the enemy smaller and red
                    enemy.Width = 20;
                    enemy.Height = 20;
                    enemy.color = Color.Red;
                }
                //else don't change enemy size and colour 
            }


            //add enemy to list so it can be drawn and updated
            enemyList.Add(enemy);
        }

        //Check if new enemy can be placed at given coords
        private bool NewEnemyCanBePlacedAt(int newEnemyX, int newEnemyY)
        {
            //for each enemy
            foreach (Enemy enemy in enemyList)
            {
                Rectangle currentEnemyRect = enemy.rect;
                int margin = 2;

                //Make new rectangle around the enemy with a margin of 2
                Rectangle rectAroundCurrentEnemy = new Rectangle(
                    currentEnemyRect.X - margin, 
                    currentEnemyRect.Y - margin,
                    40 + margin * 2, 
                    40 + margin * 2
                );

                //if new enemy's corners overlap with the rectangle around current enemy, return false
                if (rectAroundCurrentEnemy.Contains(newEnemyX, newEnemyY) // top left corner
                    || rectAroundCurrentEnemy.Contains(newEnemyX + 40, newEnemyY)  // top right corner
                    || rectAroundCurrentEnemy.Contains(newEnemyX, newEnemyY + 40)  // bottom left corner
                    || rectAroundCurrentEnemy.Contains(newEnemyX + 40, newEnemyY + 40)) // bottom right corner
                {
                    return false;
                }
            }

            //Else, return true if new enemy can be placed here
            return true;
        }

        //returns the number of bullets in bulletList
        public void RemoveEnemy(int enemyIndex)
        {
            enemyList.RemoveAt(enemyIndex);
        }

        //return index of enemy at given coords
        //If no enemy at given coords, return -1
        public int EnemyIndexAt(int x, int y)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                Enemy enemy = enemyList[i];

                if (enemy.rect.Contains(x, y))
                {
                    return i;
                }
            }
            return -1;
        }

        //return number of enemies
        public int GetEnemyCount()
        {
            return enemyList.Count;
        }

        public void UpdateEnemies()
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                Enemy enemy = enemyList[i];
                enemy.Update();

                //If enemy is off screen and player has lives, remove enemy and take life from player
                if (enemy.y > GameRoot.windowHeight && GameRoot.player.lives > 0)
                {
                    RemoveEnemy(i);
                    GameRoot.player.lives--;
                }
            }
        }

        //draw each enemy
        public void DrawEnemies(SpriteBatch spritebatch, Texture2D texture)
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Draw(spritebatch, texture);
            }
        }
    }
}