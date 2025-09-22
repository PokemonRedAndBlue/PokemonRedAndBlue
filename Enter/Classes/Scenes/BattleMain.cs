using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Graphics;

public class BattleMain
{
    private PokemonState.SpriteState _pokemonFrontState;
    private PokemonState.SpriteState _pokemonBackState;
    private AnimatedSprite _pokemonFront;
    private Sprite _pokemonBack;

    private List<AnimatedSprite> _pokemonFrontTeam;
    private List<Sprite> _pokemonBackTeam;

    public void LoadScene()
    {
        // load pokemon into idle state
        _pokemonFrontState = PokemonState.SpriteState.Idle;
        _pokemonBackState = PokemonState.SpriteState.Idle;

        // set initial positions
        Vector2 pokemonFrontPosition = new Vector2(600, 300);
        Vector2 pokemonBackPosition = new Vector2(300, 600);

    // load sprites using GetTeam helper
    var teamLoader = new GetTeam();
    //_pokemonFrontTeam = teamLoader.getFrontTeam();
    //_pokemonBackTeam = teamLoader.getBackTeam();
    }
}