using System;
using System.Xml.Linq;
using System.Linq;

class PokemonGenerator
{
    public static PokemonInstance GenerateRandom()
    {
        var rand = new Random();
        int randomDex = rand.Next(1, 152);

        var xml = XDocument.Load("Content/pokemon_stats.xml");

        // Find the <Pokemon> element with matching dex
        var p = xml.Descendants("Pokemon")
                   .FirstOrDefault(e => int.Parse(e.Attribute("dex").Value) == randomDex);

        if (p == null)
        {
            throw new Exception($"No Pokémon found for Dex #{randomDex}");
        }

        // Create species data from XML
        var species = new PokemonSpecies
        {
            Dex = int.Parse(p.Attribute("dex").Value),
            Name = p.Attribute("name").Value,
            Type1 = p.Attribute("type1").Value,
            Type2 = p.Attribute("type2")?.Value ?? "",
            BaseStats = new BaseStats
            {
                HP = int.Parse(p.Attribute("hp").Value),
                Attack = int.Parse(p.Attribute("attack").Value),
                Defense = int.Parse(p.Attribute("defense").Value),
                SpAttack = int.Parse(p.Attribute("sp_attack").Value),
                SpDefense = int.Parse(p.Attribute("sp_defense").Value),
                Speed = int.Parse(p.Attribute("speed").Value)
            }
        };

        // Generate random IVs (0–31)
        var ivs = new IVSet
        {
            HP = rand.Next(0, 32),
            Attack = rand.Next(0, 32),
            Defense = rand.Next(0, 32),
            SpAttack = rand.Next(0, 32),
            SpDefense = rand.Next(0, 32),
            Speed = rand.Next(0, 32)
        };

        // Generate random level
        int randomLevel = rand.Next(1, 101);

        // Create Pokémon instance
        var instance = new PokemonInstance(species, ivs, randomLevel);
        instance.CalculateStats();

        return instance;
    }
}