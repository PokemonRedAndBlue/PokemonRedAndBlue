using System;
using Microsoft.Xna.Framework;
using PokemonGame.Engine;
using Enter.Classes.Animations;
using Enter.Classes.Sprites;

namespace PokemonGame
{
    public enum PokemonView { Front, Back }

    public class Pokemon
    {
        public string Name { get; }
        public int Hp { get; private set; }
        public int MaxHp { get; }
        public PokemonView View { get; }
        public StateMachine StateMachine { get; }
        public Sprite Sprite { get; private set; } 
        public AnimatedSprite AnimatedSprite { get; private set; }
        public Vector2 Position { get; set; }

        public Pokemon(string name, int level, PokemonView view, Sprite sprite, Vector2 position)
        {
            Name = name;
            MaxHp = 100; // Simplified for example
            Hp = MaxHp;
            View = view;
            Sprite = sprite;
            Position = position;
            
            // Setup the State Machine
            StateMachine = new StateMachine(this);
            StateMachine.AddState("idle", new IdleState());
            StateMachine.AddState("attacking", new AttackingState());
            StateMachine.AddState("hurt", new HurtState());
            StateMachine.AddState("dead", new DeadState());
            StateMachine.AddState("dying", new DyingState());
            StateMachine.AddState("deploying", new DeployingState());
            StateMachine.AddState("retreating", new RetreatingState());
            
            StateMachine.TransitionTo("idle");
        }

        public Pokemon(string name, int level, PokemonView view, AnimatedSprite animatedSprite)
        {
            Name = name;
            MaxHp = 100; // Simplified for example
            Hp = MaxHp;
            View = view;
            AnimatedSprite = animatedSprite;
        }

            
            // Setup the State Machine
            StateMachine = new StateMachine(this);
            StateMachine.AddState("idle", new IdleState());
            StateMachine.AddState("attacking", new AttackingState());
            StateMachine.AddState("hurt", new HurtState());
            StateMachine.AddState("dead", new DeadState());
            StateMachine.AddState("dying", new DyingState());
            StateMachine.AddState("deploying", new DeployingState());
            StateMachine.AddState("retreating", new RetreatingState());
            
            StateMachine.TransitionTo("idle");
        }

        public void TakeDamage(int damage)
        {
            Hp = Math.Max(0, Hp - damage);
            Console.WriteLine($"{Name}'s HP is now {Hp}/{MaxHp}");
            StateMachine.TransitionTo("hurt");
        }

        public void PlayAnimation(string animationName)
        {
            string animationKey = $"{animationName}_{View.ToString().ToLower()}";
            Console.WriteLine($"[Animation] {Name} is playing: {animationKey}");

            // call animation here
            switch (animationName)
            {
                case "idle":
                    // Play idle animation
                    break;
                case "attack":
                    // Play attack animation
                    UpdateAttackAnimation(_pokemon.Position);
                    break;
                case "hurt":
                    // Play hurt animation
                    UpdateHurtAnimation(_pokemon.Position);
                    break;
                case "dying":
                    // Play dying animation
                    UpdateDeathAnimation(_pokemon.Position);
                    break;
                case "dead":
                    // Play dead animation
                    break;
                case "deploying":
                    // Play deploying animation
                    break;
                case "retreating":
                    // Play retreating animation
                    break;
                default:
            }
        }
        
        // Other methods like ShowSprite(), HideSprite() will go here
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

        public void Update(GameTime gameTime) { /* Idle breathing animation logic might go here */ }
        public void Exit() { }
    }
    
    public class AttackingState : IState
    {
        private Pokemon _pokemon;

        public void Enter(object owner, params object[] args)
        {
            _pokemon = (Pokemon)owner;
            Move move = (Move)args[0];
            Pokemon target = (Pokemon)args[1];

            Console.WriteLine($"{_pokemon.Name} is using {move.Name} on {target.Name}!");
            this.StartAnimation(_pokemon.Position);
            _pokemon.PlayAnimation("attack");
            
            int damage = 15; // Placeholder
            target.TakeDamage(damage);

            // In final game, we will use a timer or animation-complete event
            // before transitioning back. For now, we transition immediately.
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
            {
                _pokemon.StateMachine.TransitionTo("dying");
            }
            else
            {
                // Similar to attack, wait for animation to finish then go idle
                _pokemon.StateMachine.TransitionTo("idle");
            }
        }
        
        public void Update(GameTime gameTime) { }
        public void Exit() { }
    }
}