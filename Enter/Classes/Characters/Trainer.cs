using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Enter.Classes.Characters;

public class Trainer
{

    public Vector2 Position { get; set; }
    private float _visionRange = 3f;
    private const float InteractRange = 1.5f;

    public Trainer(Vector2 Pos)
    {
        Position = Pos;
    }

    public Trainer(Vector2 Pos, int visionRange)
    {
        Position = Pos;
        _visionRange = visionRange;
    }

    private bool IsVisible(Player player)
    {
        // Might add vision blocking mechanisms later
        return Vector2.Distance(Position, player.Position) <= _visionRange;
    }

    private void GoToPlayer(Player player)
    {
        Vector2 diff = player.Position - Position;
        while (Vector2.Distance(Position, player.Position) > InteractRange)
        {
            if (diff.X > 0) ; // TODO: MoveRight()
            else; // TODO: MoveLeft()

            if (diff.Y > 0) ; // TODO: MoveUp()
            else; // TODO: MoveDown()
        }
    }

    private void Idle() { }

    public void StopPlayer(Player player)
    {
        if (IsVisible(player))
        {
            // TODO: Deactivate Player commands
            GoToPlayer(player);
        }
        else Idle();
    }

}
