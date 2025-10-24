using Microsoft.Xna.Framework.Input;
using Enter.Classes.Characters;
using Enter.Classes.Input;
using Microsoft.Xna.Framework;
using Enter.Classes.Cameras;

namespace Enter.Classes.Behavior;

public static class Command
{

    public static void UpdateCommands(Game1 game, GameTime gameTime, KeyboardController keyboard, Camera Cam, Player player, Trainer trainer)
    {
        /*** Special checks ***/
        // Escape => exit game
        if (keyboard.IsNewlyDown(Keys.Escape))
            game.Exit();

        // R => reset the game
        game.ResetRequested = keyboard.IsNewlyDown(Keys.R);

        // // Y => next tile
        // if (keyboard.IsNewlyDown(Keys.Y))
        //     if (game?.TileCycler != null) game.TileCycler.Next();

        // // T => previous tile
        // if (keyboard.IsNewlyDown(Keys.T))
        //     if (game?.TileCycler != null) game.TileCycler.Prev();

        // O => previous trainer sprite
        if (keyboard.IsNewlyDown(Keys.O)) trainer.PrevSprite();

        // P => next trainer sprite
        if (keyboard.IsNewlyDown(Keys.P)) trainer.NextSprite();


        /*** General updates ***/
        // characters
        player.Update(gameTime, keyboard, Cam);
        trainer.Update(gameTime, player);   // Might be a list to loop through later
    }

}
