using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp
{
    public class Boss
    {
        public int x { get; private set;  }
        public int y { get; private set; }
        public int speed { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public int health { get; set; }
        public int hitBoxX { get; set; }
        public int hitBoxY { get; set; }
        public int hitBoxWidth { get; set; }
        public int hitBoxHeight { get; set; }


        private bool isVisible;

        public Boss()
        {
            x = 10;
            y = 50;
            speed = 2;
            width = 300;
            height = 230;
            health = 100;

            hitBoxWidth = 40;
            hitBoxHeight = 40;
            hitBoxX = x + width / 2 - hitBoxWidth / 2;
            hitBoxY = y + height - hitBoxHeight;
            
            isVisible = true;
        }

        public void Update()
        {
            if (health == 0) isVisible = false;
            if (!isVisible) return;

            //update boss
            x += speed;

            //update hitbox location
            hitBoxX = x + width / 2 - hitBoxWidth / 2;
            hitBoxY = y + height - hitBoxHeight;

            //wall collision and moving down
            if  (x <= 0 || x + width >= GameRoot.windowWidth)
            {
                speed *= -1;
                y += 30;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (!isVisible) return;
            //Boss
            spriteBatch.Draw(GameRoot.FFAtexture, new Rectangle(x, y, width, height), Color.White);

            //Boss hit box
            spriteBatch.Draw(GameRoot.FFAtexture, new Rectangle(hitBoxX, hitBoxY, hitBoxWidth, hitBoxHeight), Color.Red);

            //Boss Health
            spriteBatch.DrawString(GameRoot.font, "Boss: " + health, new Vector2(10, 570), Color.White);

        }
    }
}
