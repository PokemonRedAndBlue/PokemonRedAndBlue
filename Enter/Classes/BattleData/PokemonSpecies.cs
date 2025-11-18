using System;
using System.Collections.Generic;

public class PokemonSpecies
{
    public int Dex;
    public string Name;
    public string Type1;
    public string Type2;
    public BaseStats BaseStats;
    public List<string> Moves { get; set; }
    public int CatchRate;
    
}

public class BaseStats
{
    public int HP, Attack, Defense, SpAttack, SpDefense, Speed;
}

public class IVSet
{
    public int HP, Attack, Defense, SpAttack, SpDefense, Speed;
}

public class PokemonInstance
{
    public PokemonSpecies Species;
    public IVSet IVs;
    public int Level;
    public int FinalHP, FinalAttack, FinalDefense, FinalSpAttack, FinalSpDefense, FinalSpeed;
    public List<string> Moves { get; set; }

    public PokemonInstance(PokemonSpecies species, IVSet ivs, int level)
    {
        Species = species;
        IVs = ivs;
        Level = level;
        Moves = [.. species.Moves];
    }

    public void CalculateStats()
    {
        // Stat formulas
        FinalHP = (int)(((2 * Species.BaseStats.HP + IVs.HP) * Level / 100) + Level + 10);
        FinalAttack = (int)(((2 * Species.BaseStats.Attack + IVs.Attack) * Level / 100) + 5);
        FinalDefense = (int)(((2 * Species.BaseStats.Defense + IVs.Defense) * Level / 100) + 5);
        FinalSpAttack = (int)(((2 * Species.BaseStats.SpAttack + IVs.SpAttack) * Level / 100) + 5);
        FinalSpDefense = (int)(((2 * Species.BaseStats.SpDefense + IVs.SpDefense) * Level / 100) + 5);
        FinalSpeed = (int)(((2 * Species.BaseStats.Speed + IVs.Speed) * Level / 100) + 5);
    }

    public void PrintSummary()
    {
        Console.WriteLine($"{Species.Name} (Lv. {Level})");
        Console.WriteLine($"Type: {Species.Type1}" + (Species.Type2 != "" ? $"/{Species.Type2}" : ""));
        Console.WriteLine($"IVs: HP {IVs.HP}, Atk {IVs.Attack}, Def {IVs.Defense}, SpA {IVs.SpAttack}, SpD {IVs.SpDefense}, Spe {IVs.Speed}");
        Console.WriteLine($"Stats: HP {FinalHP}, Atk {FinalAttack}, Def {FinalDefense}, SpA {FinalSpAttack}, SpD {FinalSpDefense}, Spe {FinalSpeed}");
    }
}
