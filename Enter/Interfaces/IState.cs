using Microsoft.Xna.Framework.Graphics;

namespace Enter.Interfaces;

public interface IState
{
        void Enter(object owner, params object[] args);
        void Update(GameTime gameTime);
        void Exit();
    }