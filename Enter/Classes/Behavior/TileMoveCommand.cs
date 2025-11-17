using Microsoft.Xna.Framework;

namespace Enter.Classes.Behavior;

/// <summary>
/// Represents a single movement between two adjacent tiles.
/// The command tracks its start/target tiles and drives the pixel interpolation.
/// </summary>
public sealed class TileMoveCommand
{
    public Point StartTile { get; }
    public Point TargetTile { get; }
    public Vector2 StartPixel { get; }
    public Vector2 TargetPixel { get; }

    public TileMoveCommand(Point startTile, Point targetTile, Vector2 startPixel, Vector2 targetPixel)
    {
        StartTile = startTile;
        TargetTile = targetTile;
        StartPixel = startPixel;
        TargetPixel = targetPixel;
    }

    public Vector2 Advance(Vector2 currentPixel, float speed, float dt)
    {
        Vector2 toTarget = TargetPixel - currentPixel;

        Vector2 stepDir = Vector2.Zero;
        if (toTarget.X != 0) stepDir.X = System.MathF.Sign(toTarget.X);
        if (toTarget.Y != 0) stepDir.Y = System.MathF.Sign(toTarget.Y);

        Vector2 delta = stepDir * speed * dt;
        Vector2 newPos = currentPixel + delta;

        if ((stepDir.X > 0 && newPos.X > TargetPixel.X) || (stepDir.X < 0 && newPos.X < TargetPixel.X))
            newPos.X = TargetPixel.X;
        if ((stepDir.Y > 0 && newPos.Y > TargetPixel.Y) || (stepDir.Y < 0 && newPos.Y < TargetPixel.Y))
            newPos.Y = TargetPixel.Y;

        return newPos;
    }

    public bool HasArrived(Vector2 currentPixel) => currentPixel == TargetPixel;
}
