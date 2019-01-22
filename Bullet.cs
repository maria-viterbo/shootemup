using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp
{
    class Bullet
    {
        public int x { get; private set; }
        public int y { get; private set; }

        int width;
        int height;
        int speed;
        
        public Bullet()
        {

            width = 5;
            height = 5;
            speed = 5;

            //Set bullet origin
            Player player = GameRoot.player;
            x = player.x + player.width / 2 - width / 2;
            y = player.y;

        }

        public void Update()
        {
            y -= speed;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
                spriteBatch.Draw(texture, new Rectangle(x, y, width, height), Color.Yellow);

        }

    }
}
