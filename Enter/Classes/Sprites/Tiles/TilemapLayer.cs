using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Sprites;

/// Represents a single layer in a tilemap
public class TilemapLayer
{
    public string Name { get; set; }
    public int[,] Data { get; set; }
    public bool Visible { get; set; } = true;

    public TilemapLayer(string name, int width, int height)
    {
        Name = name;
        Data = new int[height, width];
    }

    public int GetTileAt(int x, int y)
    {
        if (x >= 0 && x < Data.GetLength(1) && y >= 0 && y < Data.GetLength(0))
        {
            return Data[y, x];
        }
        return 0;
    }

    public void SetTileAt(int x, int y, int tileId)
    {
        if (x >= 0 && x < Data.GetLength(1) && y >= 0 && y < Data.GetLength(0))
        {
            Data[y, x] = tileId;
        }
    }

    public void Draw(Tilemap tilemap, float scale)
    {
        if (!Visible) return;

        int rows = Data.GetLength(0);
        int cols = Data.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int tileId = Data[y, x];
                
                // 0 means empty tile
                if (tileId == 0) continue;

                if (tilemap.TileSet.TryGetValue(tileId, out var tile))
                {
                    int drawX = (int)(x * tilemap.TileWidth * scale);
                    int drawY = (int)(y * tilemap.TileHeight * scale);
                    tile.Draw(drawX, drawY, scale, SpriteEffects.None);
                }
            }
        }
    }

    /// Draw only tiles that intersect the given world-space view rectangle
    public void DrawCropped(Tilemap tilemap, Rectangle viewRect, float scale)
    {
        if (!Visible) return;

        // Work in scaled pixel units so spacing matches tile render size
        int tileW = (int)(tilemap.TileWidth * scale);
        int tileH = (int)(tilemap.TileHeight * scale);

        int cols = Data.GetLength(1);
        int rows = Data.GetLength(0);

        int x0 = Math.Max(0, viewRect.Left / tileW);
        int y0 = Math.Max(0, viewRect.Top / tileH);
        int x1 = Math.Min(cols - 1, (viewRect.Right - 1) / tileW);
        int y1 = Math.Min(rows - 1, (viewRect.Bottom - 1) / tileH);

        for (int y = y0; y <= y1; y++)
        {
            for (int x = x0; x <= x1; x++)
            {
                int tileId = Data[y, x];
                if (tileId == 0) continue;

                if (tilemap.TileSet.TryGetValue(tileId, out var tile))
                {
                    int drawX = x * tileW;
                    int drawY = y * tileH;
                    tile.Draw(drawX, drawY, scale, SpriteEffects.None);
                }
            }
        }
    }
}