using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Sprites;

/// Represents a complete tilemap with multiple layers
public class Tilemap
{
    public int TileWidth { get; set; }
    public int TileHeight { get; set; }
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
    
    public Dictionary<string, TilemapLayer> Layers { get; set; }
    public Dictionary<int, Tile> TileSet { get; set; }

    public Tilemap()
    {
        Layers = new Dictionary<string, TilemapLayer>();
        TileSet = new Dictionary<int, Tile>();
    }

    /// Draw all layers of the tilemap
    public void Draw(float scale = 1.0f)
    {
        foreach (var layer in Layers.Values)
        {
            layer.Draw(this, scale);
        }
    }

    /// Draw all layers of the tilemap, but only tiles that intersect the given world-space view rectangle
    public void DrawCropped(Rectangle viewRect, float scale = 1.0f)
    {
        foreach (var layer in Layers.Values)
        {
            layer.DrawCropped(this, viewRect, scale);
        }
    }

    /// Draw a specific layer
    public void DrawLayer(string layerName, float scale = 1.0f)
    {
        if (Layers.TryGetValue(layerName, out var layer))
        {
            layer.Draw(this, scale);
        }
    }

    /// Get tile ID at a specific position in a layer
    public int GetTileAt(string layerName, int x, int y)
    {
        if (Layers.TryGetValue(layerName, out var layer))
        {
            return layer.GetTileAt(x, y);
        }
        return 0;
    }

    /// Set tile ID at a specific position in a layer
    public void SetTileAt(string layerName, int x, int y, int tileId)
    {
        if (Layers.TryGetValue(layerName, out var layer))
        {
            layer.SetTileAt(x, y, tileId);
        }
    }
}

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

/// Loads tilemap from XML file
public static class TilemapLoader
{
    public static Tilemap LoadTilemap(string xmlPath)
    {
        var tilemap = new Tilemap();

        try
        {
            using (var stream = TitleContainer.OpenStream(xmlPath))
            {
                var doc = XDocument.Load(stream);
                var root = doc.Root;

                if (root == null)
                {
                    Console.WriteLine($"TilemapLoader: XML root was null for '{xmlPath}'");
                    return tilemap;
                }

                // Load properties
                var properties = root.Element("Properties");
                if (properties != null)
                {
                    tilemap.TileWidth = int.Parse(properties.Element("TileWidth")?.Value ?? "16");
                    tilemap.TileHeight = int.Parse(properties.Element("TileHeight")?.Value ?? "16");
                    tilemap.MapWidth = int.Parse(properties.Element("MapWidth")?.Value ?? "10");
                    tilemap.MapHeight = int.Parse(properties.Element("MapHeight")?.Value ?? "10");

                    // Load tile definitions
                    string tileDefsPath = properties.Element("TileDefinitions")?.Value;
                    if (!string.IsNullOrEmpty(tileDefsPath))
                    {
                        var tiles = TileLoader.LoadTiles($"Content/{tileDefsPath}");
                        foreach (var tile in tiles)
                        {
                            tilemap.TileSet[tile.Id] = tile;
                        }
                    }
                }

                // Load layers
                var layers = root.Elements("Layer");
                foreach (var layerElement in layers)
                {
                    string layerName = layerElement.Attribute("name")?.Value ?? "Unnamed";
                    var layer = new TilemapLayer(layerName, tilemap.MapWidth, tilemap.MapHeight);

                    var rows = layerElement.Elements("Row").ToList();
                    for (int y = 0; y < rows.Count && y < tilemap.MapHeight; y++)
                    {
                        string rowData = rows[y].Value;
                        string[] tiles = rowData.Split(',');

                        for (int x = 0; x < tiles.Length && x < tilemap.MapWidth; x++)
                        {
                            if (int.TryParse(tiles[x].Trim(), out int tileId))
                            {
                                layer.Data[y, x] = tileId;
                            }
                        }
                    }

                    tilemap.Layers[layerName] = layer;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TilemapLoader: Exception loading '{xmlPath}': {ex.Message}");
        }

        return tilemap;
    }
}
