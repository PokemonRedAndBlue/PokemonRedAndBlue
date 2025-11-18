using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

public static class SoundEffectLibrary
{
    private static readonly Dictionary<SfxId, SoundEffect> _sounds = new();

    public static void Load(ContentManager content)
    {
        foreach (SfxId id in Enum.GetValues(typeof(SfxId)))
        {
            string name = "Audio/" + id.ToString();   // asset name in Pipeline
            _sounds[id] = content.Load<SoundEffect>(name);
        }
    }

    public static SoundEffect Get(SfxId id)
    {
        return _sounds[id];
    }
}
