using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

public static class SoundEffectPlayer
{
    // Track currently playing instances by SfxId
    private static readonly Dictionary<SfxId, SoundEffectInstance> _activeInstances =
        new Dictionary<SfxId, SoundEffectInstance>();

    /// <summary>
    /// Play a sound effect once.
    /// If the same SFX is already playing, it will NOT restart.
    /// </summary>
    public static void Play(SfxId id, float volume = 1f)
    {
        // If this SFX is already playing, do nothing
        if (_activeInstances.TryGetValue(id, out var existing))
        {
            if (existing.State == SoundState.Playing)
            {
                return; // same sound still playing → skip
            }

            // If it finished or was stopped, clean it up
            existing.Dispose();
            _activeInstances.Remove(id);
        }

        // Create and play a new instance
        SoundEffect effect = SoundEffectLibrary.Get(id);
        SoundEffectInstance instance = effect.CreateInstance();

        instance.IsLooped = false;  // always play once
        instance.Volume = volume;
        instance.Play();

        // Remember this instance as "currently playing" for this SFX
        _activeInstances[id] = instance;
    }

    /// <summary>
    /// Stop a specific SFX if it is playing.
    /// </summary>
    public static void Stop(SfxId id)
    {
        if (_activeInstances.TryGetValue(id, out var instance))
        {
            if (instance.State != SoundState.Stopped)
            {
                instance.Stop();
            }

            instance.Dispose();
            _activeInstances.Remove(id);
        }
    }

    /// <summary>
    /// Stop all currently playing SFX.
    /// </summary>
    public static void StopAll()
    {
        foreach (var kvp in _activeInstances)
        {
            var instance = kvp.Value;
            if (instance.State != SoundState.Stopped)
            {
                instance.Stop();
            }
            instance.Dispose();
        }

        _activeInstances.Clear();
    }
}
