using System.Collections.Generic;

namespace Enter.Classes.Physics
{
    public static class SolidTileCollision
    {
        public static readonly HashSet<int> SolidIds = new()
        {
            2,  3,                  // cylinder, tree
            8, 10, 11, 12,          // fences/gate
            15,                      // signpost
            74, 80, 81, 101,              // gym-water, podium, statue, elevated-water
            20, 21, 22, 36,                // water, cylinder, tree, water-top-edge (city)
            25, 26, 27, 29, 30, 31, 32, 33, 34, // roofs, windows for npc housing (city)
            41, 42, 47, 54, 55, 56,              // fences (city)
            49, 50, 51,                           // bollards, signpost, sapling (city)
            48, 58, 67, 68, 69, 70, 82, 83, 84, 85, 86, 87, 88,  // gym structure (city)
            59, 60, 61, 62, 63, 64, 65, 66, 89, 90, 91, 92, 93, 94, 100 // mart structure (city)
        };

        public static bool IsSolid(int id) => id != 0 && SolidIds.Contains(id);
    }
}
