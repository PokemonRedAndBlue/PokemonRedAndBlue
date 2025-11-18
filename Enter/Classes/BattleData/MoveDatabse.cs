using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

public static class MoveDatabase
{
    private static Dictionary<string, Move> moves =
        new Dictionary<string, Move>(StringComparer.OrdinalIgnoreCase);

    private static readonly string filePath = "Content/Moves.xml";

    // Automatically loads once
    static MoveDatabase()
    {
        LoadMoves();
    }

    private static void LoadMoves()
    {
        XDocument xml = XDocument.Load(filePath);

        foreach (var m in xml.Root.Elements("Move"))
        {
            string name = m.Attribute("name").Value;
            string type = m.Attribute("type").Value;
            int power = int.Parse(m.Attribute("power").Value);

            MoveCategory category = m.Attribute("category").Value == "Physical"
                ? MoveCategory.Physical
                : MoveCategory.Special;

            moves[name] = new Move
            {
                Name = name,
                Type = type,
                Power = power,
                Category = category
            };
        }
    }

    public static Move Get(string name)
    {
        if (!moves.TryGetValue(name, out var move))
            throw new Exception($"Move not found: {name}");

        return move;
    }
}