using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Enter.Classes.Sprites;

namespace Enter.Classes.Physics
{
    public static class Collision
    {
        // Build a solid-tile index set (Check TileCollisionProfile.cs to set solid tiles)
        public static HashSet<Point> BuildSolidIndexSet(Tilemap map, string layerName, System.Func<int, bool> isSolid)
        {
            var set = new HashSet<Point>();
            if (map == null || !map.Layers.TryGetValue(layerName, out var layer)) return set;

            for (int y = 0; y < map.MapHeight; y++)
            {
                for (int x = 0; x < map.MapWidth; x++)
                {
                    int id = layer.GetTileAt(x, y);
                    if (isSolid(id))
                        set.Add(new Point(x, y));
                }
            }
            return set;
        }

        // Which grid cells could intersect during the horizontal pass
        public static List<Point> GetIntersectingTilesHorizontal(Rectangle target, int tileSize)
        {
            List<Point> intersections = new();

            int widthInTiles  = (target.Width  - (target.Width  % tileSize)) / tileSize;
            int heightInTiles = (target.Height - (target.Height % tileSize)) / tileSize;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersections.Add(new Point(
                        (target.X + x * tileSize) / tileSize,
                        (target.Y + y * (tileSize - 1)) / tileSize
                    ));
                }
            }
            return intersections;
        }

        // Which grid cells could intersect during the vertical pass
        public static List<Point> GetIntersectingTilesVertical(Rectangle target, int tileSize)
        {
            List<Point> intersections = new();

            int widthInTiles  = (target.Width  - (target.Width  % tileSize)) / tileSize;
            int heightInTiles = (target.Height - (target.Height % tileSize)) / tileSize;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersections.Add(new Point(
                        (target.X + x * (tileSize - 1)) / tileSize,
                        (target.Y + y * tileSize) / tileSize
                    ));
                }
            }
            return intersections;
        }
    }
}
