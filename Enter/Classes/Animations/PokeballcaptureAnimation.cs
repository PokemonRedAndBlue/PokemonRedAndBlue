using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Animations;

public class PokeballCaptureAnimation
{
    private Texture2D _texture;

    private Vector2 position;

    //If active frame is 0 when doesnt animate
    private int activeFrame = 1;

    private int count;


    public PokeballCaptureAnimation(int x,int y)
    {
        //this._texture = texture;
        this.position = new Vector2(x, y);
        activeFrame = 1;
        count = 0;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("images/Pokeball");
    }

    public void Update(GameTime gameTime)
    {
        count++;
        if (count > 15)
        {
            count = 0;
            if (activeFrame == 3)
            {
                activeFrame = 0;
            }
            else
            { 
                activeFrame++;
            } 
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRect = new Rectangle(0, 24*activeFrame, 16, 24);
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

