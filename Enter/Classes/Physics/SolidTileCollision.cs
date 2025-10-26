using System.Collections.Generic;

namespace Enter.Classes.Physics
{
    public static class SolidTileCollision
    {
        public static readonly HashSet<int> SolidIds = new()
        {
            2,  3,                  // cylinder, tree
            8, 10, 11, 12,          // fences/gate
            15                      // signpost
        };

        public static bool IsSolid(int id) => id != 0 && SolidIds.Contains(id);
    }
}
