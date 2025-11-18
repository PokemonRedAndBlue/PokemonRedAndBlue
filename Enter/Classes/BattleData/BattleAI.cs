using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

public static class BattleAI
{
    public static Move ChooseBestMove(PokemonInstance attacker, PokemonInstance defender)
    {
        Move bestMove = null;
        int bestDamage = -1;

        // The attacker.Species.Moves contains the move NAMES from your XML
        foreach (string moveName in attacker.Moves)
        {
            Move move = MoveDatabase.Get(moveName);

            // Simulate damage using your real damage calculator
            int dmg = DamageCalculator.CalculateDamage(attacker, defender, move);

            // Track strongest move
            if (dmg > bestDamage)
            {
                bestDamage = dmg;
                bestMove = move;
            }
        }

        return bestMove;
    }
    public static Move ChooseRandomMove(PokemonInstance attacker)
        {
        var moves = attacker.Species.Moves;  // List<string> of move names

        // Random index 0–3
        int index = Random.Shared.Next(moves.Count);

        // Convert the name to a full Move object
        return MoveDatabase.Get(moves[index]);
    }
}
