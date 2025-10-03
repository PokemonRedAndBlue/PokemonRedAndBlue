using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using ISprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;

namespace Enter.Classes.Sprites
{
    public static class TileLoader
    {
        // xmlPath default expects "Content/Tiles.xml"
        public static List<Tile> LoadTiles(string xmlPath = "Content/Tiles.xml")
        {
            var tiles = new List<Tile>();

            try
            {
                using (var stream = TitleContainer.OpenStream(xmlPath))
                {
                    var doc = XDocument.Load(stream);
                    var root = doc.Root;
                    if (root == null)
                    {
                        Console.WriteLine("TileLoader: XML root was null.");
                        return tiles;
                    }

                    // Determine texture asset from <Texture> element (if present)
                    string textureAsset = (string)root.Element("Texture") ?? (string)root.Element("texture");

                    Texture2D _texture = null;
                    if (!string.IsNullOrEmpty(textureAsset))
                    {
                        // Try to load the asset name exactly as given in the XML.
                        // Make sure the value in XML matches the Content pipeline asset name.
                        try
                        {
                            _texture = Core.Content.Load<Texture2D>(textureAsset);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"TileLoader: failed to load texture asset '{textureAsset}': {ex.Message}");
                        }
                    }

                    // Regions can be under <Regions><Region .../></Regions>
                    var regionsParent = root.Element("Regions");
                    IEnumerable<XElement> regionElements = null;

                    if (regionsParent != null)
                    {
                        regionElements = regionsParent.Elements("Region");
                    }
                    else
                    {
                        // fallback: look for <tile> or <Region> directly under root
                        regionElements = root.Elements("Region");
                        if (regionElements == null)
                            regionElements = root.Elements("tile");
                    }

                    foreach (var el in regionElements)
                    {
                        // parse attributes (id, name, x, y, width, height)
                        int id = int.Parse((string)el.Attribute("id"));
                        string name = (string)el.Attribute("name") ?? string.Empty;
                        int xSrc = int.Parse((string)el.Attribute("x"));
                        int ySrc = int.Parse((string)el.Attribute("y"));
                        int width = int.Parse((string)el.Attribute("width"));
                        int height = int.Parse((string)el.Attribute("height"));

                        /* If the XML lists a different texture per region, 
                        could add code to read a 'texture' attribute per region.
                        */
                        if (_texture == null)
                        {
                            Console.WriteLine($"TileLoader: no common texture loaded for tile id={id} (textureAsset was '{textureAsset}'). Skipping tile.");
                            continue;
                        }

                        var srcRect = new Rectangle(xSrc, ySrc, width, height);
                        tiles.Add(new Tile(_texture, srcRect, id));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TileLoader: exception loading '{xmlPath}': {ex.Message}");
            }

            return tiles;
        }
    }
}
