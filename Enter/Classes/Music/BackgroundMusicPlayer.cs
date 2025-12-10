using Microsoft.Xna.Framework.Media;

public static class BackgroundMusicPlayer
{
    private static SongId? _currentId = null;
    private static bool _isPlaying = false;

    public static void Play(SongId id, bool loop = true)
    {
        // If same song is already playing, do nothing
        if (_currentId == id && _isPlaying)
            return;

        Song song = BackgroundMusicLibrary.GetSong(id);
        
        // If song is null (audio not loaded), skip playback
        if (song == null)
            return;

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
