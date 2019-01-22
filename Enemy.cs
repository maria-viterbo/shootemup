using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp
{
    public class Enemy
    {
        public int width = 40;
        public int height = 40;
        public Color color { get; set; }

        public int x;
        public int y;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }


        //return new enemy rectangle with the latest coords and dimensions
        public Rectangle rect
        {
            get { return new Rectangle(this.x, this.y, width, height); }
        } 

        //constructor
        public Enemy(int x, int y)
        {
            this.x = x;
            this.y = y;
            color = Color.White;
        }


        public void Update()
        {
            y += 1;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
                spriteBatch.Draw(texture, rect, color);     
        }

    }
}

