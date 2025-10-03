using System.Collections.Generic;

namespace ISprite
{
    public class TileCycler
    {
        private readonly List<Tile> _tiles;
        private int _currentIndex;

        public TileCycler(List<Tile> tiles)
        {
            _tiles = tiles ?? new List<Tile>();
            _currentIndex = (_tiles.Count > 0) ? 0 : -1;
        }

        // Advance to next tile (wrap-around)
        public void Next()
        {
            if (_tiles == null || _tiles.Count == 0) return;
            _currentIndex = (_currentIndex + 1) % _tiles.Count;
        }

        // Go to previous tile (wrap-around)
        public void Prev()
        {
            if (_tiles == null || _tiles.Count == 0) return;
            _currentIndex = (_currentIndex - 1 + _tiles.Count) % _tiles.Count;
        }

        // Get currently selected tile (null if none)
        public Tile GetCurrent()
        {
            if (_tiles == null || _currentIndex < 0 || _currentIndex >= _tiles.Count) return null;
            return _tiles[_currentIndex];
        }

        // Reset to the first tile
        public void Reset()
        {
            _currentIndex = (_tiles.Count > 0) ? 0 : -1;
        }
        
    }
}
