using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;



class PokemonGenerator
{
    public static readonly string filePath = "Content/pokemon_stats.xml";

    public static Random rand = new Random();

    public static PokemonInstance GenerateWildPokemon()
    {
        int randomDex = rand.Next(1, 20); // 001–003 range

        var species = GenerateSpecies(randomDex);

        // Generate random IVs (0–31)
        var ivs = RandomIV();

        // Random level between 1–100
        int randomLevel = rand.Next(1, 13);

        // Create the Pokémon instance
        var instance = new PokemonInstance(species, ivs, randomLevel);
        instance.CalculateStats();

        return instance;
    }

    public static PokemonInstance GenerateTrainerPokemon(int dex, int level)
    {
        var species = GenerateSpecies(dex);

        // Generate random IVs (0–31)
        var ivs = RandomIV();

        // Create the Pokémon instance
        var instance = new PokemonInstance(species, ivs, level);
        instance.CalculateStats();

        return instance;
    }

    public static IVSet RandomIV()
    {
        var ivs = new IVSet
        {
            HP = rand.Next(0, 32),
            Attack = rand.Next(0, 32),
            Defense = rand.Next(0, 32),
            SpAttack = rand.Next(0, 32),
            SpDefense = rand.Next(0, 32),
            Speed = rand.Next(0, 32)
        };

        return ivs;
    }

    public static PokemonSpecies GenerateSpecies(int dex)
    {

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Pokémon file not found: {filePath}");
        }

        // Load that single Pokémon's XML
        var xml = XDocument.Load(filePath);
        
        var p = xml.Descendants("Pokemon")
           .FirstOrDefault(e => int.Parse(e.Element("Dex").Value) == dex);

        if (p == null)
            throw new Exception($"No <Pokemon> element found");
        
        var moves = p.Element("Moves")
                 .Elements("Move")
                 .Select(m => m.Value)
                 .ToList();
                 
        var species = new PokemonSpecies
        {
            Dex = int.Parse(p.Element("Dex").Value),
            Name = p.Element("Name").Value,
            Type1 = p.Element("Type1").Value,
            Type2 = p.Element("Type2")?.Value ?? "",
            BaseStats = new BaseStats
            {
                HP = int.Parse(p.Element("BaseStats").Element("HP").Value),
                Attack = int.Parse(p.Element("BaseStats").Element("Attack").Value),
                Defense = int.Parse(p.Element("BaseStats").Element("Defense").Value),
                SpAttack = int.Parse(p.Element("BaseStats").Element("SpAttack").Value),
                SpDefense = int.Parse(p.Element("BaseStats").Element("SpDefense").Value),
                Speed = int.Parse(p.Element("BaseStats").Element("Speed").Value)
            },
            Moves = moves
        };

        return species;
    }
}
