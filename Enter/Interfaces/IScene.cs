using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Interfaces
{
    /// <summary>
    /// The interface all game scenes (Overworld, Battle, Menu) must implement.
    /// </summary>
    public interface IGameScene
    {
        /// <summary>
        /// Called when the scene is first loaded.
        /// Use this to load textures, fonts, sounds, etc.
        /// </summary>
        void LoadContent(ContentManager content);

        /// <summary>
        /// Called every frame. Contains all scene-specific game logic.
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// Called every frame. Contains all scene-specific draw calls.
        /// </summary>
        void Draw(SpriteBatch spriteBatch);
    }
}
