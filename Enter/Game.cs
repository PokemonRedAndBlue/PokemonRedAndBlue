using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;
using System.Threading.Tasks;

namespace GameFile;

public class Game1 : Core
{
    private TextureAtlas _PokemonBackAtlas;
    private double elapsedTime = 0;    
    private int regionsToDraw = 0;     


    public Game1() : base("PokemonRedAndBlue", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Load the atlas texture using the content manager from the XML configuration file
        _PokemonBackAtlas = TextureAtlas.FromFile(Content, "atlas-definition.xml");
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        if (elapsedTime >= 0.5 && regionsToDraw < _PokemonBackAtlas._regions.Count)
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

            region.Value.Draw(SpriteBatch, new Vector2(640, 360), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            i++;
        }

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
