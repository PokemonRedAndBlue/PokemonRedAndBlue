using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PokemonGame.Engine;
using Enter.Classes.Animations;
using Enter.Classes.Sprites;
using Enter.Interfaces;

namespace PokemonGame
{
    public enum PokemonView { Front, Back }

    public class Pokemon
    {
        private readonly Dictionary<string, Action<Vector2>> _animationActions;
        public string Name { get; }
        public int Hp { get; private set; }
        public int MaxHp { get; }
        private int _level;
        public PokemonView View { get; }
        public StateMachine StateMachine { get; }
        public Sprite Sprite { get; private set; }
        public AnimatedSprite AnimatedSprite { get; private set; }
        public Vector2 Position { get; set; }
        public Sprite _sprite;

        public Pokemon(string name, int level)
        {
            Name = name;
            _level = level;
        }
        public Pokemon(string name, int level, PokemonView view, AnimatedSprite animatedSprite, Vector2 position)
        {
            Name = name;
            MaxHp = 100;
            Hp = MaxHp;
            View = view;
            AnimatedSprite = animatedSprite;
            Position = position;

            StateMachine = new StateMachine(this);
            StateMachine.AddState("idle", new IdleState());
            StateMachine.AddState("attacking", new AttackingState());
            StateMachine.AddState("hurt", new HurtState());
            StateMachine.AddState("dead", new DeadState());
            StateMachine.AddState("dying", new DyingState());
            StateMachine.AddState("deploying", new DeployingState());
            StateMachine.AddState("retreating", new RetreatingState());
            StateMachine.TransitionTo("idle");

            _animationActions = new Dictionary<string, Action<Vector2>>
            {
                { "attack", pos => UpdateAttackAnimation(pos) },
                { "hurt",   pos => UpdateHurtAnimation(pos) },
                { "dying",  pos => UpdateDeathAnimation(pos) },
                { "idle",   pos => { } },
                { "dead",   pos => { } },
                { "deploying", pos => { } },
                { "retreating", pos => { } }
            };
        }

        public Pokemon(string name, int level, PokemonView view, Sprite sprite, Vector2 position)
        {
            Name = name;
            MaxHp = 100;
            Hp = MaxHp;
            View = view;
            _sprite = sprite;
            Position = position;

            StateMachine = new StateMachine(this);
            StateMachine.AddState("idle", new IdleState());
            StateMachine.AddState("attacking", new AttackingState());
            StateMachine.AddState("hurt", new HurtState());
            StateMachine.AddState("dead", new DeadState());
            StateMachine.AddState("dying", new DyingState());
            StateMachine.AddState("deploying", new DeployingState());
            StateMachine.AddState("retreating", new RetreatingState());
            StateMachine.TransitionTo("idle");

            _animationActions = new Dictionary<string, Action<Vector2>>
            {
                { "attack", pos => UpdateAttackAnimation(pos) },
                { "hurt",   pos => UpdateHurtAnimation(pos) },
                { "dying",  pos => UpdateDeathAnimation(pos) },
                { "idle",   pos => { } },
                { "dead",   pos => { } },
                { "deploying", pos => { } },
                { "retreating", pos => { } }
            };
        }

        public void TakeDamage(int damage)
        {
            Hp = Math.Max(0, Hp - damage);
            Console.WriteLine($"{Name}'s HP is now {Hp}/{MaxHp}");
            StateMachine.TransitionTo("hurt");
        }

        public void PlayAnimation(string animationName)
        {
            if (string.IsNullOrEmpty(animationName)) return;
            Console.WriteLine($"[Animation] {Name} is playing: {animationName}");

            if (_animationActions != null && _animationActions.TryGetValue(animationName, out var action))
            {
                action(Position);
            }
        }

        // --- simple stub animation handlers ---
        private void UpdateAttackAnimation(Vector2 position)
        {
            // TODO: trigger actual attack animation; currently a placeholder
            Console.WriteLine($"{Name} attack animation at {position}");
        }

        private void UpdateHurtAnimation(Vector2 position)
        {
            Console.WriteLine($"{Name} hurt animation at {position}");
        }

        private void UpdateDeathAnimation(Vector2 position)
        {
            Console.WriteLine($"{Name} death animation at {position}");
        }
    }

    // --- State Implementations ---
    public class IdleState : IState
    {
        private Pokemon _pokemon;
        public void Enter(object owner, params object[] args)
        {
            _pokemon = (Pokemon)owner;
            Console.WriteLine($"{_pokemon.Name} is now idle.");
            _pokemon.PlayAnimation("idle");
        }
        public void Update(GameTime gameTime) { }
        public void Exit() { }
    }

    public class AttackingState : IState
    {
        private Pokemon _pokemon;
        public void Enter(object owner, params object[] args)
        {
            _pokemon = (Pokemon)owner;

            // transition back to idle (replace with animation-complete logic later)
            _pokemon.StateMachine.TransitionTo("idle");
        }
        public void Update(GameTime gameTime) { }
        public void Exit() { }
    }

    public class HurtState : IState
    {
        private Pokemon _pokemon;
        public void Enter(object owner, params object[] args)
        {
            _pokemon = (Pokemon)owner;
            Console.WriteLine($"{_pokemon.Name} took damage!");
            _pokemon.PlayAnimation("hurt");
            if (_pokemon.Hp <= 0)
                _pokemon.StateMachine.TransitionTo("dying");
            else
                _pokemon.StateMachine.TransitionTo("idle");
        }
        public void Update(GameTime gameTime) { }
        public void Exit() { }
    }

    // Empty placeholder state classes referenced earlier
    public class DeadState : IState { public void Enter(object owner, params object[] args) { } public void Update(GameTime gameTime) { } public void Exit() { } }
    public class DyingState : IState { public void Enter(object owner, params object[] args) { } public void Update(GameTime gameTime) { } public void Exit() { } }
    public class DeployingState : IState { public void Enter(object owner, params object[] args) { } public void Update(GameTime gameTime) { } public void Exit() { } }
    public class RetreatingState : IState { public void Enter(object owner, params object[] args) { } public void Update(GameTime gameTime) { } public void Exit() { } }
}