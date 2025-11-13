using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

//Source of Music: www.zophar.net/music/gameboy-gbs/pokemon-red
public static class MusicLibrary
{
    private static readonly Dictionary<SongId, Song> _songs = new Dictionary<SongId, Song>();

    public static void Load(ContentManager content)
    {
        _songs[SongId.OpeningPart1] = content.Load<Song>("Audio/01 Opening (part 1)");
        _songs[SongId.OpeningPart2] = content.Load<Song>("Audio/02 Opening (part 2)");
        _songs[SongId.ToBillsOriginFromCerulean] = content.Load<Song>("Audio/03 To Bill's Origin ~ From Cerulean");
        _songs[SongId.PalletTownTheme] = content.Load<Song>("Audio/04 Pallet Town's Theme");
        _songs[SongId.PokemonCenter] = content.Load<Song>("Audio/05 Pokemon Center");
        _songs[SongId.PokemonGym] = content.Load<Song>("Audio/06 Pokemon Gym");
        _songs[SongId.PewterCityTheme] = content.Load<Song>("Audio/07 Pewter City's Theme");
        _songs[SongId.CeruleanCityTheme] = content.Load<Song>("Audio/08 Cerulean City's Theme");
        _songs[SongId.CeladonCityTheme] = content.Load<Song>("Audio/09 Celadon City's Theme");
        _songs[SongId.CinnabarIslandTheme] = content.Load<Song>("Audio/10 Cinnabar Island's Theme");
        _songs[SongId.VermilionCityTheme] = content.Load<Song>("Audio/11 Vermilion City's Theme");
        _songs[SongId.LavenderTownTheme] = content.Load<Song>("Audio/12 Lavender Town's Theme");
        _songs[SongId.StAnne] = content.Load<Song>("Audio/13 St. Anne");
        _songs[SongId.ProfessorOak] = content.Load<Song>("Audio/14 Professor Oak");
        _songs[SongId.RivalAppears] = content.Load<Song>("Audio/15 Rival Appears");
        _songs[SongId.Guide] = content.Load<Song>("Audio/16 Guide");
        _songs[SongId.Evolution] = content.Load<Song>("Audio/17 Evolution");
        _songs[SongId.RoadToViridianFromPallet] = content.Load<Song>("Audio/18 The Road to Viridian City ~ from Pallet");
        _songs[SongId.RoadToCeruleanFromMtMoon] = content.Load<Song>("Audio/19 The Road to Cerulean ~ from Mt. Moon");
        _songs[SongId.RoadToLavenderFromVermilion] = content.Load<Song>("Audio/20 The Road to Lavender Town ~ from Vermilion");
        _songs[SongId.LastRoad] = content.Load<Song>("Audio/21 The Last Road");
        _songs[SongId.BattleGymLeader] = content.Load<Song>("Audio/22 Battle (VS Gym Leader)");
        _songs[SongId.BattleTrainer] = content.Load<Song>("Audio/23 Battle (VS Trainer)");
        _songs[SongId.BattleWildPokemon] = content.Load<Song>("Audio/24 Battle (VS Wild Pokemon)");
        _songs[SongId.LastBattleRival] = content.Load<Song>("Audio/25 Last Battle (VS Rival)");
        _songs[SongId.VictoryTrainer] = content.Load<Song>("Audio/26 Victory (VS Trainer)");
        _songs[SongId.VictoryWildPokemon] = content.Load<Song>("Audio/27 Victory (VS Wild Pokemon)");
        _songs[SongId.VictoryGymLeader] = content.Load<Song>("Audio/28 Victory (VS Gym Leader)");
        _songs[SongId.Ending] = content.Load<Song>("Audio/29 Ending");
        _songs[SongId.IntoTheHall] = content.Load<Song>("Audio/30 Into the Hall");
        _songs[SongId.OakResearchLab] = content.Load<Song>("Audio/31 Oak Research Lab");
        _songs[SongId.JigglypuffsSong] = content.Load<Song>("Audio/32 Jigglypuff's Song");
        _songs[SongId.Cycling] = content.Load<Song>("Audio/33 Cycling");
        _songs[SongId.Ocean] = content.Load<Song>("Audio/34 Ocean");
        _songs[SongId.Casino] = content.Load<Song>("Audio/35 Casino");
        _songs[SongId.TeamRocketHideout] = content.Load<Song>("Audio/36 Team Rocket Hideout");
        _songs[SongId.ViridianForest] = content.Load<Song>("Audio/37 Viridian Forest");
        _songs[SongId.MtMoonCave] = content.Load<Song>("Audio/38 Mt. Moon Cave");
        _songs[SongId.PokemonMansion] = content.Load<Song>("Audio/39 Pokemon Mansion");
        _songs[SongId.PokemonTower] = content.Load<Song>("Audio/40 Pokemon Tower");
        _songs[SongId.SylphCompany] = content.Load<Song>("Audio/41 Sylph Company");
        _songs[SongId.TrainerAppearsBadGuy] = content.Load<Song>("Audio/42 Trainer Appears (Bad Guy Version)");
        _songs[SongId.TrainerAppearsGirl] = content.Load<Song>("Audio/43 Trainer Appears (Girl Version)");
        _songs[SongId.TrainerAppearsBoy] = content.Load<Song>("Audio/44 Trainer Appears (Boy Version)");
        _songs[SongId.PokemonRecoveryFanfare] = content.Load<Song>("Audio/45 Pokemon Recovery");
        _songs[SongId.ItemFanfare] = content.Load<Song>("Audio/46 Item Fanfare");
        _songs[SongId.PokemonReceivedFanfare] = content.Load<Song>("Audio/47 Pokemon Received Fanfare");
        _songs[SongId.LevelUpFanfare] = content.Load<Song>("Audio/48 Level Up Fanfare");
        _songs[SongId.PokemonCaptureFanfare] = content.Load<Song>("Audio/49 Pokemon Capture Fanfare");
        _songs[SongId.PokedexFanfare1] = content.Load<Song>("Audio/50 Pokedex Fanfare 1");
        _songs[SongId.PokedexFanfare2] = content.Load<Song>("Audio/51 Pokedex Fanfare 2");
    }

    public static Song GetSong(SongId id)
    {
        return _songs[id];
    }
}
