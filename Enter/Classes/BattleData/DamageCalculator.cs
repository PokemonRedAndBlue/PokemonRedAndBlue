using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

public static class DamageCalculator
{
    private static readonly string typeChartPath = "Content/type_chart.xml";

    public static int CalculateDamage(
        PokemonInstance attacker,
        PokemonInstance defender,
        Move move
    )
    {
        int power = move.Power;

        // Determine if physical or special
        bool isPhysical = move.Category == MoveCategory.Physical;

        int attackStat = isPhysical ? attacker.FinalAttack : attacker.FinalSpAttack;
        int defenseStat = isPhysical ? defender.FinalDefense : defender.FinalSpDefense;

        // Base damage
        double baseDamage =
            ((2.0 * attacker.Level / 5.0) + 2) * power * ((double)attackStat / defenseStat) / 50.0 + 2;

        //Modifier (STAB * TypeEffectiveness * Random)
        double modifier =
            GetSTAB(attacker, move.Type) *
            GetTypeEffectiveness(move.Type, defender.Species.Type1, defender.Species.Type2) *
            GetRandom();

        int finalDamage = (int)(baseDamage * modifier);

        if (finalDamage < 1)
            finalDamage = 1;

        return finalDamage;
    }


    private static double GetSTAB(PokemonInstance attacker, string moveType)
    {
        if (attacker.Species.Type1 == moveType || attacker.Species.Type2 == moveType)
            return 1.5;

        return 1.0;
    }

    private static double GetTypeEffectiveness(string moveType, string defType1, string defType2)
    {
        double multiplier = 1.0;

        XDocument chart = XDocument.Load(typeChartPath);

        var attackTypeNode = chart
            .Root
            .Elements("Attack")
            .FirstOrDefault(t => t.Attribute("type").Value == moveType);

        if (attackTypeNode == null)
            return multiplier;

        foreach (var eff in attackTypeNode.Elements("Effectiveness"))
        {
            string target = eff.Attribute("target").Value;
            double mod = double.Parse(eff.Attribute("multiplier").Value);

            if (target == defType1) multiplier *= mod;
            if (target == defType2) multiplier *= mod;
        }

        return multiplier;
    }

    private static double GetRandom()
    {
        Random rand = new Random();
        return rand.Next(85, 101) / 100.0;
    }
}
