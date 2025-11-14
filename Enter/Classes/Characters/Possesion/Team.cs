using PokemonGame;
using System;

public class Team
{
    public Pokemon[] Pokemons { get; private set; }

    public Team()
    {
        Pokemons = new Pokemon[6];
    }

    public Team(Pokemon[] pokemons)
    {
        if (pokemons.Length > 6)
        {
            throw new ArgumentException("A team must consist of exactly no more than 6 Pokemon.");
        } else if (pokemons.Length < 6)
        {
            for(int i = pokemons.Length; i <= 6; i++)
            {
                pokemons[i] = new Pokemon("I am just a placeholder", 999);
            }
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