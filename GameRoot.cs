using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootEmUp
{
    public class GameRoot : Game
    {
        //static variable
        public static Player player { get; private set; }
        public static EnemyManager enemyManager { get; private set; }
        public static Texture2D FFAtexture { get; private set; }
        public static SpriteFont font { get; private set; }
        public static SpriteFont largeFont { get; private set; }
        public static int windowWidth { get; private set; }
        public static int windowHeight { get; private set; }
        public static int currentLevel { get; private set; }
        public static Boss boss { get; private set; }
        public static SoundEffect playerShot { get; private set; }
        


        Texture2D background;
        Texture2D playerTexture;
        bool showLevelEndScreen;
        bool showStartScreen;
        bool showGameOver;
        bool showGameEndScreen;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            SetUpWindowSize();
        
            //create instances
            player = new Player();
            enemyManager = new EnemyManager();
            boss = new Boss();

            //set up variables
            currentLevel = 1;
            showLevelEndScreen = false;
            showStartScreen = true;
            showGameEndScreen = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            FFAtexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            FFAtexture.SetData(new Color[] { Color.White });

            background = Content.Load<Texture2D>("space");
            playerTexture = Content.Load<Texture2D>("player");
            font = Content.Load<SpriteFont>("font");
            largeFont = Content.Load<SpriteFont>("largeFont");

            playerShot = Content.Load<SoundEffect>("gun");

        }

        protected override void UnloadContent()
        {
            playerShot.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (showStartScreen)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    enemyManager.SetUpEnemiesFor(currentLevel);
                    showStartScreen = false;
                }
            }
            else
            {
                if (player.lives > 0)
                {
                    player.Update();
                }

                else
                {
                    GameOverScreen();
                }

                if (currentLevel == 5)
                {
                    boss.Update();

                    if (boss.health == 0)
                    {
                        showGameEndScreen = true;
                    }

                    if (boss.y + boss.height >= player.y)
                    {
                        player.lives = 0;
                    }
                }

                enemyManager.UpdateEnemies();
                CheckIfLevelComplete();
            }

             base.Update(gameTime);
        }

        private void GameOverScreen()
        {
            showGameOver = true;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                showGameOver = false;
                player.lives = 3;
                enemyManager.SetUpEnemiesFor(currentLevel);
                player.bulletManager.ClearBullets();

                if (currentLevel == 5)
                {
                    boss = new Boss();
                }
            }
        }

        private void CheckIfLevelComplete()
        {
            if (player.lives > 0 && enemyManager.GetEnemyCount() == 0 && currentLevel <= 4)
            {
                showLevelEndScreen = true;

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    showLevelEndScreen = false;
                    player.lives = 3;
                    currentLevel++;
                    enemyManager.SetUpEnemiesFor(currentLevel);

                    if (currentLevel == 3)
                    {
                        player.maxBulletDelay = 10;
                    }
                }
            }
        }
    
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);
            if (showStartScreen)
            {
                DrawStartScreen();
            }
            else
            {
                player.Draw(spriteBatch, playerTexture);
                enemyManager.DrawEnemies(spriteBatch, FFAtexture);
                DrawLevelIndicator();

                //level end screen
                if (showLevelEndScreen)
                {
                    DrawLevelEndScreen();
                }

                if (showGameOver)
                {
                    DrawGameOverScreen();
                }

                if (currentLevel == 5)
                {
                    boss.Draw(spriteBatch, FFAtexture);

                    if (showGameEndScreen)
                    {
                        DrawGameEndScreen();
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGameEndScreen()
        {
            spriteBatch.DrawString(largeFont, "MISSION ACCOMPLISHED!", new Vector2(350, 250), Color.LimeGreen);

        }

        private void DrawLevelEndScreen()
        {
            spriteBatch.DrawString(largeFont, "Level " + currentLevel + " Completed", new Vector2(350, 250), Color.White);
            spriteBatch.DrawString(largeFont, "Tap Space to Continue", new Vector2(336, 350), Color.White);

            if (currentLevel == 2)
            {
                spriteBatch.DrawString(largeFont, "Ability Unlocked: Rapid Fire!", new Vector2(312, 270), Color.Yellow);
            }
        }

        private void DrawLevelIndicator()
        {
            spriteBatch.DrawString(font, "Level " + currentLevel, new Vector2(10, 10), Color.White);
        }

        private void SetUpWindowSize()
        {
            windowWidth = 800;
            windowHeight = 600;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        private void DrawStartScreen()
        {
            spriteBatch.DrawString(largeFont, "Shoot 'Em Up", new Vector2(350, 250), Color.White);
            spriteBatch.DrawString(largeFont, "Tap Space to Start", new Vector2(336, 270), Color.White);
        }

        private void DrawGameOverScreen()
        {
            spriteBatch.DrawString(largeFont, "GAME OVER", new Vector2(357, 250), Color.Red);
            spriteBatch.DrawString(largeFont, "Tap Space to Restart", new Vector2(334, 270), Color.White);
        }
    }
}