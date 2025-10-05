using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Enter.Classes.Sprites;

public class TrainerSpriteFactory
{

    public List<Rectangle> Sprites { get => _sprites; }
    public static int MaxIndexOfSprites { get; } = 8;   // *** Make sure to update this ***

    private static readonly int[] _index =  // int constants for X positions of each row in the sprite sheet
    [
        9, 26, 43,  // Face Down
        60, 77, 94, // Face Up
        111, 128,   // Face Left
        145, 162,   // Face Right
    ];
    private const int Width = 16, Height = 16;  // arguments for creating rectagles

    private static readonly int[] _spriteRows = // For the Y position in the sprite sheet
    [   // not all characters in the sprite sheet added for now
        // Moving Characters (0..8)
        85, 102, 136, 153, 170, 187, 204, 221, 238
        // Stationary Characters
        // 68, 204, 221, 238, 
    ];

    private readonly List<Rectangle> _sprites = [];

    public TrainerSpriteFactory(int spriteIndex)
    {
        for (int i = 0; i < _index.Length; i++)
        {
            _sprites.Add(new Rectangle(_index[i], _spriteRows[spriteIndex], Width, Height));
        }
    }

}
