using Microsoft.Xna.Framework;
using Enter.Classes.Textures;
using Enter.Classes.Sprites;
using PokemonGame;
using Microsoft.Xna.Framework.Graphics;
using System;
using Enter.Classes.Animations;
using System.ComponentModel.DataAnnotations;
using Microsoft.Xna.Framework.Input;

public class Team
{
    public Pokemon[] Pokemons { get; private set; }

    public Team(Pokemon[] pokemons)
    {
        if (pokemons.Length != 6)
        {
            throw new ArgumentException("A team must consist of exactly 6 Pokemon.");
        }
        Pokemons = pokemons;
    }

    public Team addPokemon(Pokemon newPokemon, int slot)
    {
        if (slot < 0 || slot >= 6)
        {
            throw new ArgumentOutOfRangeException("Slot must be between 0 and 5.");
        }

        Pokemon[] updatedPokemons = (Pokemon[])Pokemons.Clone();
        updatedPokemons[slot] = newPokemon;
        return new Team(updatedPokemons);
    }

    public Pokemon removePokemon(int slot)
    {
        if (slot < 0 || slot >= 6)
        {
            throw new ArgumentOutOfRangeException("Slot must be between 0 and 5.");
        }

        Pokemon removedPokemon = Pokemons[slot];
        Pokemon[] updatedPokemons = (Pokemon[])Pokemons.Clone();
        updatedPokemons[slot] = null;
        Pokemons = updatedPokemons;
        return removedPokemon;
    }

    public String GetPokemonState(int slot)
    {
        if (slot < 0 || slot >= 6)
        {
            throw new ArgumentOutOfRangeException("Slot must be between 0 and 5.");
        }
        if (Pokemons[slot] == null)
        {
            return "empty";
        }
        return "alive";
    }
}