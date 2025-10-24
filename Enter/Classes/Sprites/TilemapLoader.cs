using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace Enter.Classes.Sprites;

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