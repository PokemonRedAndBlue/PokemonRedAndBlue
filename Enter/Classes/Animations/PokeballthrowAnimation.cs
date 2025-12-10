using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Animations;

public class PokeballthrowAnimation
{
    private Texture2D _texture;

    private Vector2 position;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    public bool IsComplete { get; private set; }

    private float t;                // normalized progress 0..1
    private float step;             // how much t increases each update
    private float maxHeight;        // maximum arc height

    public PokeballthrowAnimation(int startX, int startY, Vector2 target)
    {
        startPosition = new Vector2(startX, startY);
        targetPosition = target;
        position = startPosition;

        t = 0f;
        step = 0.025f;   // smaller = smoother and slower
        maxHeight = 110;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("images/Pokeball");
    }

    public void Update(GameTime gameTime)
    {
        if (IsComplete) return;

        t += step;
        if (t >= 1f)
        {
            t = 1f;
            IsComplete = true;
        }

        float arc = MathF.Sin(t * MathF.PI) * maxHeight;
        position = Vector2.Lerp(startPosition, targetPosition, t) + new Vector2(0f, -arc);
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
