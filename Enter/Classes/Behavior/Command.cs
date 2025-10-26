using Microsoft.Xna.Framework.Input;
using Enter.Classes.Characters;
using Enter.Classes.Input;
using Microsoft.Xna.Framework;
using Enter.Classes.Cameras;
using Enter.Classes.Scenes;

namespace Enter.Classes.Behavior;

public static class Command
{

    public static void UpdateCommands(Game1 game, GameTime gameTime, KeyboardController keyboard, Camera Cam, Player player, Trainer trainer, SceneManager _sceneManager)
    {
        /*** Special checks ***/
        // Escape => exit game
        if (keyboard.IsNewlyDown(Keys.Escape))
            game.Exit();

        // R => reset the game
        game.ResetRequested = keyboard.IsNewlyDown(Keys.R);

        // O => previous trainer sprite
        if (keyboard.IsNewlyDown(Keys.O)) trainer.PrevSprite();

        // P => next trainer sprite
        if (keyboard.IsNewlyDown(Keys.P)) trainer.NextSprite();

        // check for wild encounter key
        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
        {
            _sceneManager.TransitionTo("wild");
        }

        /*** General updates ***/
        // characters
        player.Update(gameTime, keyboard, Cam);
        trainer.Update(gameTime, player);   // Might be a list to loop through later
    }

}
