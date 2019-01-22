using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp
{
    public class Player
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int width { get; private set; }
        public int lives { get; set; }

        public int bulletDelay { get; set; }
        public int maxBulletDelay { get; set;  }

        public BulletManager bulletManager { get; set; }

        //constructor
        public Player()
        {
            x = 400;
            y = 500;
            width = 50;
            lives = 3;

            bulletDelay = 0;
            maxBulletDelay = 40;

            bulletManager = new BulletManager();

  
        }

        public void Update()
        {
            MovePlayerToMousePos();
            ShootWhenMouseIsClicked();

        }

        private void MovePlayerToMousePos()
        {
            int mouseX = Mouse.GetState().X;

            if (mouseX >= 0 && mouseX <= GameRoot.windowWidth - this.width)
            {
                this.x = mouseX;
            }

            else
            {
                if (mouseX < 0) this.x = 0;

                if (mouseX > GameRoot.windowWidth)
                    this.x = GameRoot.windowWidth - this.width;        
            }
        }

        private void ShootWhenMouseIsClicked()
        {
            //Update Bullet Delay
            if (bulletDelay > 0)
            {
                bulletDelay--;
            }

            //If left mouse button is clicked
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                
                //and if no delay
                if (bulletDelay == 0)
                {
                    bulletManager.AddBullet(); //shoot
                    GameRoot.playerShot.Play();
                    bulletDelay = maxBulletDelay; //reset delay
                }
            }

            //Update bullet positions and check for collisions
            bulletManager.UpdateBullets();
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D playerTexture)
        {
            spriteBatch.Draw(playerTexture, new Vector2(this.x, this.y), Color.White);
            bulletManager.DrawBullets(spriteBatch, GameRoot.FFAtexture);
            spriteBatch.DrawString(GameRoot.font, "Lives: " + lives, new Vector2(730, 10), Color.Red);
        }
    }
}
