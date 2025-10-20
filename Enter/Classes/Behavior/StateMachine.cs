/**
 * A generic, reusable Finite State Machine (FSM).
 */
class StateMachine {

    constructor(initialState, states, context = {}) {
        this.currentState = null;
        this.states = states;
        this.context = context; // The 'this' for the state methods (e.g., the Pokemon object)
        this.transitionTo(initialState);
    }

    /**
     * Transitions from the current state to a new state.
     */
    transitionTo(newStateName, ...args) {
        if (!this.states[newStateName]) {
            console.error(`Error: State '${newStateName}' not found.`);
            return;
        }

        // 1. Call the exit method of the current state, if it exists
        if (this.currentState && this.currentState.onExit) {
            this.currentState.onExit.call(this.context);
        }

        // 2. Update to the new state
        this.currentState = this.states[newStateName];
        this.currentState.name = newStateName;

        // 3. Call the enter method of the new state, if it exists
        if (this.currentState.onEnter) {
            this.currentState.onEnter.call(this.context, ...args);
        }
    }

    /**
     * Calls the onUpdate method of the current state.
     */
    update(...args) {
        if (this.currentState && this.currentState.onUpdate) {
            this.currentState.onUpdate.call(this.context, ...args);
        }
    }

    /**
     * Returns the name of the current state.
     */
    getCurrentState() {
        return this.currentState ? this.currentState.name : null;
    }
}

// --- State Definitions ---

// 1. POKEMON STATES
const pokemonStates = {
    idle: {
        onEnter() {
            console.log(`${this.name} is now idle.`);
            this.playAnimation('idle');
        },
        onUpdate() {
            // Logic for idle state, like a subtle breathing animation loop
        },
    },
    attacking: {
        onEnter(move, target) {
            console.log(`${this.name} is using ${move.name} on ${target.name}!`);
            this.playAnimation('attack');
            
            // In a real game, you'd calculate damage here
            const damage = Math.floor(Math.random() * 10) + 5; // Placeholder damage
            target.takeDamage(damage);

            // After attack animation, transition back to idle
            setTimeout(() => this.stateMachine.transitionTo('idle'), 1000);
        },
    },
    hurt: {
        onEnter() {
            console.log(`${this.name} took damage!`);
            this.playAnimation('hurt');

            if (this.hp <= 0) {
                this.stateMachine.transitionTo('dying');
            } else {
                // Transition back to idle after the hurt animation plays
                setTimeout(() => this.stateMachine.transitionTo('idle'), 500);
            }
        },
    },
    dying: {
        onEnter() {
            console.log(`${this.name} is fainting!`);
            this.playAnimation('faint');
            // After faint animation, transition to the 'dead' state
            setTimeout(() => this.stateMachine.transitionTo('dead'), 1500);
        },
    },
    dead: {
        onEnter() {
            console.log(`${this.name} has fainted.`);
            this.hideSprite();
        },
    },
    deploying: {
        onEnter() {
            console.log(`Go, ${this.name}!`);
            this.showSprite();
            this.playAnimation('deploy'); // e.g., pokeball opening animation
            // After animation, transition to idle
            setTimeout(() => this.stateMachine.transitionTo('idle'), 1200);
        },
    },
    retrieving: {
        onEnter() {
            console.log(`Come back, ${this.name}!`);
            this.playAnimation('retrieve'); // e.g., beam-of-light animation
            // After animation, hide the sprite
            setTimeout(() => this.hideSprite(), 1000);
        },
    },
};

// 2. PLAYER STATES (in the overworld)
const playerStates = {
    present: {
        onEnter() {
            console.log("Player is present and controllable.");
            // e.g., this.enableMovement();
        },
        onUpdate(deltaTime) {
            // e.g., this.handleInput(deltaTime);
        },
        onExit() {
            // e.g., this.stopAnimation();
        }
    },
    nonPresent: {
        onEnter() {
            console.log("Player is not present (e.g., inside a building).");
            // e.g., this.hideSprite();
            // this.disableMovement();
        },
    },
    cutscene: {
        onEnter(targetPosition) {
            console.log("Player is in a cutscene.");
            // e.g., this.disableMovement();
            // this.moveTo(targetPosition);
        },
        onUpdate() {
            // Logic for automated movement or waiting
        },
        onExit() {
            console.log("Player cutscene finished.");
            // e.g., this.enableMovement();
        },
    },
};

// 3. GAME SCENE STATES
const gameStates = {
    overworld: {
        onEnter() {
            console.log("Game entered Overworld scene.");
            // e.g., this.playMusic('overworld_theme');
            // this.player.stateMachine.transitionTo('present');
        },
        onUpdate(deltaTime) {
            // e.g., this.player.update(deltaTime);
            // Check for random encounters or trainer triggers
        },
    },
    wildEncounter: {
        onEnter(wildPokemon) {
            console.log(`A wild ${wildPokemon.name} appeared!`);
            // e.g., this.startBattleTransition();
            // this.playMusic('battle_theme');
            // this.player.stateMachine.transitionTo('cutscene'); // or a new 'battling' state
        },
        onExit() {
            console.log("Wild encounter ended.");
            // e.g., this.playMusic('overworld_theme');
        }
    },
    trainerBattle1: {
        onEnter(trainer) {
            console.log(`You are challenged by ${trainer.name}!`);
            // e.g., this.startBattleTransition();
            // this.playMusic('trainer_battle_theme');
        },
    },
    trainerBattle2: {
        onEnter(trainer) {
            console.log(`You are challenged by ${trainer.name}!`);
            // e.g., this.startBattleTransition();
            // this.playMusic('gym_leader_theme');
        },
    }
};