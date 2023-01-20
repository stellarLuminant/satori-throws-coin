using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMonoGame
{
    public class Game1 : Game
    {
        // satori
        private bool satoriFlip = false;
        private readonly float satoriSpeed = 120f;
        private Vector2 satoriPosition = new Vector2(150, 200);
        private Texture2D satoriTexture;
        private Vector2 satoriOrigin;

        // coin
        private const float COIN_COOLDOWN = 0.15f;
        private float lastCoinThrow;
        private List<Coin> coins;
        private Texture2D[] coinTextures;

        // store
        private Vector2 storePosition = new Vector2(600, 200);
        private Texture2D storeTexture;
        private Vector2 storeOrigin;

        // coin options
        private int coinType;

        // etc
        private KeyboardState lastKb;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            coins = new List<Coin>();
            lastKb = Keyboard.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            satoriTexture = Content.Load<Texture2D>("satori");
            satoriOrigin = new Vector2(satoriTexture.Width / 2f, satoriTexture.Height / 2f);
            
            storeTexture = Content.Load<Texture2D>("store");
            storeOrigin = new Vector2(storeTexture.Width / 2f, storeTexture.Height / 2f);

            var list = new List<Texture2D>();

            list.Add(Content.Load<Texture2D>("coin"));
            list.Add(Content.Load<Texture2D>("timmycoin"));
            list.Add(Content.Load<Texture2D>("toastcoin"));
            list.Add(Content.Load<Texture2D>("crabcoin"));

            coinTextures = list.ToArray();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kb = Keyboard.GetState();
            var mouse = Mouse.GetState();
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var mousePos = new Vector2(mouse.X, mouse.Y);

            // Inputs
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
                satoriPosition.Y -= satoriSpeed * deltaTime;
            
            if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
            {
                satoriPosition.X -= satoriSpeed * deltaTime;
                satoriFlip = false;
            }

            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
                satoriPosition.Y += satoriSpeed * deltaTime;

            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
            {
                satoriPosition.X += satoriSpeed * deltaTime;
                satoriFlip = true;
            }

            // Swap coin type
            if (kb.IsKeyDown(Keys.Space) && lastKb.IsKeyUp(Keys.Space))
            {
                coinType = (coinType + 1) % coinTextures.Length;
            }

            // Make coin
            if (mouse.LeftButton == ButtonState.Pressed && lastCoinThrow + COIN_COOLDOWN <= time)
            {
                // figure out the toss angle!
                var difference = mousePos - satoriPosition;
                var angle = difference == Vector2.Zero ? 0f : (float)Math.Atan2(difference.Y, difference.X);
                var coin = new Coin(time, satoriPosition, angle, coinType);
                coins.Add(coin);
                lastCoinThrow = time;
            }
            
            // Update coins
            foreach (var coin in coins)
                coin.Update(deltaTime);

            // Filter out old coins
            coins = coins.Where(coin => coin.tossTime + Coin.DURATION > time).ToList();

            lastKb = kb;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            SpriteEffects fx = satoriFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            _spriteBatch.Begin();
            _spriteBatch.Draw(storeTexture, storePosition, null, Color.White, 0, storeOrigin, Vector2.One, SpriteEffects.None, 0);
            
            _spriteBatch.Draw(satoriTexture, satoriPosition, null, Color.White, 0, satoriOrigin, Vector2.One, fx, 0);

            foreach (var coin in coins)
                coin.Draw(_spriteBatch, coinTextures);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}