using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;
using System.Data;
using System.Threading.Tasks;

namespace GameFile;

public class Game1 : Core
{
    private TextureAtlas _PokemonBackAtlas;
    private TextureAtlas _PokemonFrontAtlas;
    private double elapsedTime = 0;    
    private int regionsToDraw = 0;     

    private AnimatedSprite _bulbasaurFront;
    private AnimatedSprite _ivysaurFront;
    private AnimatedSprite _venusaurFront;

    public Game1() : base("PokemonRedAndBlue", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        if (elapsedTime >= 0.2 && regionsToDraw < _PokemonBackAtlas._regions.Count)
        {
            regionsToDraw++;
            elapsedTime = 0; // reset timer
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        int i = 0;
        foreach (var region in _PokemonBackAtlas._regions)
        {
            if (i >= regionsToDraw)
                break;

            region.Value.Draw(SpriteBatch, new Vector2(308, 328), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            i++;
        }
            // Draw Bulbasaur Evolution Animated Sprites
            _bulbasaurFront.Draw(SpriteBatch, new Vector2(825, 456), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            _ivysaurFront.Draw(SpriteBatch, new Vector2(950, 456), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            _venusaurFront.Draw(SpriteBatch, new Vector2(1100, 456), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
