using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Enter.Classes.Animation
{
    public class PokeballthrowAnimation
    {
        private Texture2D _texture;

        private Vector2 position;

        //If active frame is 0 when doesnt animate

        private float initialPostionX;  //inital postion of x
        private float initialPostionY;  //inital position of y
        private float xParam;            // radian x value
        private float step;              // how much x increases each update
        private float maxHeight;               // maximum height of sin function

 

        public PokeballthrowAnimation(int x, int y)
        {
            this.initialPostionX = x;
            this.initialPostionY = y;
            this.position = new Vector2(x, y);

            xParam = 0f;
            step = 0.03f;    // smaller = smoother and slower
            maxHeight = 100;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("images/Pokeball");
        }

        public void Update(GameTime gameTime)
        {
            xParam += step;
            if (xParam >= (3f * MathF.PI / 4f))
            {
                xParam = 0f;
                //return;
            }
            float px = initialPostionX + xParam * 100;
            float py = initialPostionY - maxHeight * MathF.Sin(xParam);

            position.X = px;
            position.Y = py;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(0, 48, 16, 24);
            Vector2 origin = new Vector2(sourceRect.Width / 2f, sourceRect.Height / 2f);

            float scale = 4f; //scale of object

            spriteBatch.Draw(
                _texture,
                position,            
                sourceRect,     // which frame to draw
                Color.White,
                0f,             // rotation
                origin,         // origin (so it scales from center)
                scale,          // scale factor
                SpriteEffects.None,
                0f              // layer depth
            );
            
        }
    }
}
