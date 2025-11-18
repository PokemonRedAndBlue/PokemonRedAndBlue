using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

public static class DamageTest
{
    public static void Run()
    {
        Console.WriteLine("=== DAMAGE CALCULATOR TEST ===");

        // Create attacker species (Charmander Dex 2)
        var charmander = PokemonGenerator.GenerateTrainerPokemon(2, 10);

        // Create defender species (Bulbasaur Dex 1)
        var bulbasaur = PokemonGenerator.GenerateTrainerPokemon(1, 10);

        // Get the move object
        Move ember = MoveDatabase.Get("Ember");

        Console.WriteLine("Charmander uses Ember on Bulbasaur!");

        for (int i = 0; i < 10; i++)
        {
            int dmg = DamageCalculator.CalculateDamage(charmander, bulbasaur, ember);
            Console.WriteLine($"Damage roll {i + 1}: {dmg}");
        }
    }
}
