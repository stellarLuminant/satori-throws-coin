using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TestMonoGame
{
    public class Coin
    {
        private static Random _rand = new Random();

        public const float MIN_ROTATION = MathHelper.Pi / 2;
        public const float MAX_ROTATION = MathHelper.Pi * 4;
        public const float GRAVITY = 1080f;
        public const float SPEED = 720;
        public const float DURATION = 10;

        // The time that the coin was first tossed.
        public float tossTime;

        public float rotationVelocity;
        public float rotation;
        public Vector2 velocity;
        public Vector2 position;
        public int coinType;

        private float nextRotationVelocity()
        {
            var diff = MAX_ROTATION - MIN_ROTATION;
            return MIN_ROTATION + (float)(_rand.NextDouble() * diff);
        }

        private Vector2 angleToVec(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public Coin(float tossTime, Vector2 position, float tossAngle, int coinType)
        {
            this.tossTime = tossTime;
            
            this.rotation = tossAngle;
            this.rotationVelocity = nextRotationVelocity();

            this.position = position;
            this.velocity = SPEED * angleToVec(tossAngle);

            this.coinType = coinType;
        }

        public void Update(float deltaTime)
        {
            velocity.Y += GRAVITY * deltaTime;
            position.X += velocity.X * deltaTime;
            position.Y += velocity.Y * deltaTime;
            rotation += rotationVelocity * deltaTime;
        }

        public void Draw(SpriteBatch sb, Texture2D[] textures)
        {
            drawInternal(sb, textures[coinType]);
        }

        private void drawInternal(SpriteBatch sb, Texture2D texture)
        {
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            sb.Draw(texture, position, null, Color.White, rotation, origin, Vector2.One, SpriteEffects.None, 0);
        }
    }
}
