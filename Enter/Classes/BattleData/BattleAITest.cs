using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

public static class BattleAITest
{
    public static void Run()
    {
        Console.WriteLine("=== BATTLE AI TEST ===");

        // Load species
        var charmander = PokemonGenerator.GenerateTrainerPokemon(2, 10); // Charmander
        var bulbasaur = PokemonGenerator.GenerateTrainerPokemon(1, 10); // Bulbasaur

        //-----------------------------
        // TEST 1: Trainer AI (best move)
        //-----------------------------
        Console.WriteLine("\n--- TEST 1: Trainer AI chooses best move ---");

        Move bestMove = BattleAI.ChooseBestMove(charmander, bulbasaur);
        Console.WriteLine($"Best move for Charmander vs Bulbasaur: {bestMove.Name}");

        // Show expected damage values for all moves
        Console.WriteLine("\nDamage per move:");
        foreach (string moveName in charmander.Species.Moves)
        {
            Move move = MoveDatabase.Get(moveName);
            int dmg = DamageCalculator.CalculateDamage(charmander, bulbasaur, move);
            Console.WriteLine($" {move.Name}: {dmg} damage");
        }

        //-----------------------------
        // TEST 2: Wild Pokémon random AI
        //-----------------------------
        Console.WriteLine("\n--- TEST 2: Wild AI chooses random moves ---");

        for (int i = 1; i <= 10; i++)
        {
            Move randMove = BattleAI.ChooseRandomMove(bulbasaur);
            Console.WriteLine($"Random roll {i}: Bulbasaur used {randMove.Name}");
        }

        Console.WriteLine("\n=== END OF TEST ===");
    }
}
