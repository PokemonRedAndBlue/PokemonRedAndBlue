using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;

public static class CatchCalculator
{
    private static readonly Random rand = new Random();

    public static bool isCaught(PokemonInstance target, int currentHP)
    {
        int maxHP = target.FinalHP;
        int catchRate = target.Species.CatchRate;   // make sure Species has this field

        // Safety
        if (currentHP < 1) currentHP = 1;
        if (currentHP > maxHP) currentHP = maxHP;

        // HP factor: lower HP = higher chance (0–1)
        double hpFactor = (3.0 * maxHP - 2.0 * currentHP) / (3.0 * maxHP);
        if (hpFactor < 0) hpFactor = 0;
        if (hpFactor > 1) hpFactor = 1;

        // Base chance using catch rate (0–1)
        double baseChance = (catchRate / 255.0) * hpFactor;

        // Clamp to a reasonable max (optional)
        if (baseChance > 0.95) baseChance = 0.95;
        if (baseChance < 0.01) baseChance = 0.01;

        // Roll
        double roll = rand.NextDouble(); // 0.0–1.0

        return roll < baseChance;
    }
}
