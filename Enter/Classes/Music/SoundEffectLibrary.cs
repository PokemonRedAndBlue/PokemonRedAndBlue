using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

public static class SoundEffectLibrary
{
    private static readonly Dictionary<SfxId, SoundEffect> _sounds = new();

    public static void Load(ContentManager content)
    {
        try
        {
            foreach (SfxId id in Enum.GetValues(typeof(SfxId)))
            {
                string name = "Audio/" + id.ToString();   // asset name in Pipeline
                _sounds[id] = content.Load<SoundEffect>(name);
            }
        }
        catch
        {
            // Audio files not available - continue without sound effects
        }
    }

    public static SoundEffect Get(SfxId id)
    {
        if (_sounds.TryGetValue(id, out var effect))
        {
            return effect;
        }
        return null;
    }
}
