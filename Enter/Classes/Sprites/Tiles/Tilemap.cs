using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enter.Classes.Sprites;

/// Represents a tilemap with multiple layers
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

    /// Draw all layers of tilemap
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