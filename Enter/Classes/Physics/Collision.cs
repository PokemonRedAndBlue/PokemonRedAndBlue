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
    }
}
