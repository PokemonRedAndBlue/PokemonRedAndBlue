using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;
public static class CatchTest
{
    public static void Run()
    {
        Console.WriteLine("=== CATCH RATE TEST ===");

        var bulb = PokemonGenerator.GenerateTrainerPokemon(1, 10);

        Console.WriteLine($"Testing catch rate for: {bulb.Species.Name}");
        Console.WriteLine($"Catch Rate: {bulb.Species.CatchRate}");
        Console.WriteLine();

        int success = 0;
        int attempts = 1000; // simulate 1000 Poké Balls

        for (int i = 0; i < attempts; i++)
        {
            if (CatchCalculator.isCaught(bulb, 10))
                success++;
        }

        double percent = (success / (double)attempts) * 100.0;

        Console.WriteLine($"Out of {attempts} attempts, caught {success}");
        Console.WriteLine($"Catch Percentage: {percent:0.00}%");
        Console.WriteLine("==============================\n");
    }
}
