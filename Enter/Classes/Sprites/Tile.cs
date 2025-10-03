using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;

namespace ISprite
{
    public class Tile : ISprite
    {
        private readonly Texture2D texture;
        private readonly Rectangle sourceRect;
        private readonly int width;
        private readonly int height;

        // public id in case, need to reference specific tile
        public int Id { get; }

        public Tile(Texture2D texture, Rectangle sourceRect, int id)
        {
            this.texture = texture;
            this.sourceRect = sourceRect;
            this.width = sourceRect.Width;
            this.height = sourceRect.Height;
            this.Id = id;
        }

        public void Update()
        {

        }

        public void Draw(int x, int y, SpriteEffects facing)
        {
            Core.SpriteBatch.Draw(
                texture,
                new Rectangle(x, y, width, height),
                sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                facing,
                0f
            );
        }

        public void Draw(int x, int y, float scale, SpriteEffects facing)
        {
            var destRect = new Rectangle(
                x,
                y,
                (int)(width * scale),
                (int)(height * scale)
            );

            Core.SpriteBatch.Draw(
                texture,
                destRect,
                sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                facing,
                0f
            );
        }

        public int GetWidth() => width;
        public int GetHeight() => height;
    }
}
