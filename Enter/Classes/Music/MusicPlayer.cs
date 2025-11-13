using Microsoft.Xna.Framework.Media;

public static class MusicPlayer
{
    private static SongId? _currentId = null;
    private static bool _isPlaying = false;

    public static void Play(SongId id, bool loop = true)
    {
        if (_currentId == id && _isPlaying && MediaPlayer.IsRepeating == loop)
            return;

        Song song = MusicLibrary.GetSong(id);

        MediaPlayer.IsRepeating = loop;
        MediaPlayer.Play(song);

        _currentId = id;
        _isPlaying = true;
    }

    public static void Stop()
    {
        MediaPlayer.Stop();
        _isPlaying = false;
    }

    public static void Pause()
    {
        MediaPlayer.Pause();
        _isPlaying = false;
    }

    public static void Resume()
    {
        MediaPlayer.Resume();
        _isPlaying = true;
    }

    public static SongId? CurrentSongId => _currentId;
}
