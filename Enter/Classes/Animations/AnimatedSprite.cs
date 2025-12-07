using System;
using Microsoft.Xna.Framework;

namespace Enter.Classes.Animations;

public class AnimatedSprite : Sprites.Sprite
{
    private int _currentFrame;
    private TimeSpan _elapsed;
    private Animation _animation;

    /// <summary>
    /// Gets or Sets the animation for this animated sprite.
    /// </summary>
    public Animation Animation
    {
        get => _animation;
        set
        {
            _animation = value;
            // Reset playback state so new animation always starts from the first frame.
            _currentFrame = 0;
            _elapsed = TimeSpan.Zero;
            if (_animation != null && _animation.Frames.Count > 0)
            {
                Region = _animation.Frames[0];
            }
        }
    }

    /// <summary>
    /// Creates a new animated sprite.
    /// </summary>
    public AnimatedSprite() { }

    /// <summary>
    /// Creates a new animated sprite with the specified frames and delay.
    /// </summary>
    /// <param name="animation">The animation for this animated sprite.</param>
    public AnimatedSprite(Animation animation)
    {
        Animation = animation;
    }

    /// <summary>
    /// Updates this animated sprite.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game timing values provided by the framework.</param>
    public void Update(GameTime gameTime)
    {
        _elapsed += gameTime.ElapsedGameTime;

        if (_elapsed >= _animation.Delay)
        {
            _elapsed -= _animation.Delay;
            _currentFrame++;

            if (_currentFrame >= _animation.Frames.Count)
            {
                _currentFrame = 0;
            }

            Region = _animation.Frames[_currentFrame];
        }
    }
}
