using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Enter.Classes.Sprites;

namespace Enter.Classes.Physics
{
    /// <summary>
    /// Minimal axis-separated tile collision (vimichael-style) + world clamp.
    /// </summary>
    public static class TileCollision
    {
        // Horizontal movement + collision
        public static void MoveHorizontal(ref Rectangle rect, int dx, HashSet<Point> solidTiles, int tileSize)
        {
            rect.X += dx;

            var tiles = Collision.GetIntersectingTilesHorizontal(rect, tileSize);

            foreach (var tile in tiles)
            {
                if (!solidTiles.Contains(tile))
                    continue;

                Rectangle solid = new Rectangle(tile.X * tileSize, tile.Y * tileSize, tileSize, tileSize);

                if (!rect.Intersects(solid)) continue;

                if (dx > 0)      rect.X = solid.Left - rect.Width;  // moving right
                else if (dx < 0) rect.X = solid.Right;              // moving left
            }
        }

        // Vertical movement + collision
        public static void MoveVertical(ref Rectangle rect, int dy, HashSet<Point> solidTiles, int tileSize)
        {
            rect.Y += dy;

            var tiles = Collision.GetIntersectingTilesVertical(rect, tileSize);

            foreach (var tile in tiles)
            {
                if (!solidTiles.Contains(tile))
                    continue;

                Rectangle solid = new Rectangle(tile.X * tileSize, tile.Y * tileSize, tileSize, tileSize);

                if (!rect.Intersects(solid)) continue;

                if (dy > 0)      rect.Y = solid.Top - rect.Height;  // down
                else if (dy < 0) rect.Y = solid.Bottom;             // up
            }
        }

        /// <summary>
        /// Clamp a rectangle so it stays fully inside the map’s pixel bounds.
        /// Call this after both axis passes.
        /// </summary>
        public static void ClampToWorld(ref Rectangle rect, Tilemap map)
        {
            if (map == null) return;

            int worldW = map.MapWidth  * map.TileWidth;
            int worldH = map.MapHeight * map.TileHeight;

            // Ensure the whole rect remains in bounds
            rect.X = MathHelper.Clamp(rect.X, 0, System.Math.Max(0, worldW - rect.Width));
            rect.Y = MathHelper.Clamp(rect.Y, 0, System.Math.Max(0, worldH - rect.Height));
        }
    }
}
